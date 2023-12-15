#region License
/*
Copyright 2022-2023 Dmitrii Evdokimov
Open source software

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System.Net.Mail;

using Diev.Extensions.Smtp;
using Diev.Portal5;
using Diev.Portal5.API;

using Microsoft.Extensions.Configuration;

namespace CryptoBot;

/*

20:30 AppServer3 ЗСК. Подготовка файлов для отправки в портал5: \scripts\ZSK_BIN\XXI_570_upload.bat
        source  Upload\KYCCL_7831001422_3194_20230814_000001.xml
        zip     Upload\KYCCL_7831001422_3194_20230814_000001.zip
        copy    G:\OD\FORMS\ZSK\OUT\20231212\KYCCL_7831001422_3194_20231212_000001.zip
        copy    \\192.165.72.12\ZSK_OUT\20231212\KYCCL_7831001422_3194_20231212_000001.zip



21:00 CryptoBot ЗСК. Отправка перечня клиентов в портал5: /usr/local/zsk_bin/upload.bat
        source  C:\FORMS\ZSK\OUT\20231212\KYCCL_7831001422_3194_20231212_000001.zip
        copy    C:\FORMS\ZSK\ARCHIVE\20231212\KYCCL_7831001422_3194_20231212_000001.zip
        sign    C:\FORMS\ZSK\OUT\KYCCL_7831001422_3194_20231212_000001.zip.sig
        enc     C:\FORMS\ZSK\OUT\KYCCL_7831001422_3194_20231212_000001.zip.enc
        upload  C:\FORMS\ZSK\OUT
            upload  KYCCL_7831001422_3194_20231212_000001.zip.enc
            upload  KYCCL_7831001422_3194_20231212_000001.zip.sig
        check registration

22:00 CryptoBot ЗСК. Получение реестра с портала5: /usr/local/zsk_bin/download.bat
        download C:\FORMS\ZSK\IN
            download KYC_20231212.xml.zip.enc
            download KYC_20231212.xml.zip.sig
        decr    C:\FORMS\ZSK\IN\KYC_20231212.xml.zip



22:15 AppServer3 ЗСК. Обработка полученного файла: \scripts\ZSK_BIN\XXI_570_load.bat
        source  \\192.165.72.12\ZSK_IN\KYC_20231212.xml.zip
        copy    G:\OD\FORMS\ZSK\IN\ARCHIVE\KYC_20231212.xml.zip
        copy    G:\OD\FORMS\ZSK\IN\KYC_20231212.xml.zip

22:30? AppServer3 ?
        G:\OD\FORMS\ZSK\BIN\XXI_570_load.py
            INSERT INTO XXI.ZSK_RISK_CIB (...) VALUES (...)
            DELETE FROM XXI.ZSK_RISK_CIB
            ...
            SELECT ...
        Функция вызвана 5005 раз
        Обновленны уровни риска у клиентов с номерами:
        Клиенту 11472 установлено значение параметра 0
        log     G:\OD\FORMS\ZSK\IN\ARCHIVE\KYC_20231212.xml_load_log.txt

23:30 AppServer3 ЗСК. Формирование отчета по контрагентам из списка Светофор: \scripts\ZSK_BIN\send_report.cmd odb1 XXISCR
 
*/

internal class Worker
{
    public RestClient RestClient = new();

    public bool Zadacha130 { get; set; }
    public bool Zadacha137 { get; set; }

    public string[]? Subscribers { get; set; }

    public Worker()
    {
        Program.Settings.Bind(nameof(Worker), this);

        Program.Smtp.Subscribers = Subscribers;

        Program.Settings.Bind(nameof(RestClient), RestClient);
        RestClient.Initialize();
    }

    public async Task Run()
    {
        if (Zadacha130)
        {
            var task130 = new Zadacha130(RestClient);
            await task130.Run();
        }

        if (Zadacha137)
        {
            var task137 = new Zadacha137(RestClient);
            await task137.Run();
        }

        //await Program.Smtp.SendMessageAsync("Task OK", "Job completed.");
    }
}
