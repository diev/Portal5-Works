#region License
/*
Copyright 2023 Dmitrii Evdokimov
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

using Diev.Portal5;

namespace Zadacha_137;

/*
 
21:00 CryptoBot ЗСК. Отправка перечня клиентов в портал5: /usr/local/zsk_bin/upload.bat
        source  C:\FORMS\ZSK\OUT\20231212\KYCCL_7831001422_3194_20231212_000001.zip
        copy    C:\FORMS\ZSK\ARCHIVE\20231212\KYCCL_7831001422_3194_20231212_000001.zip
        sign    C:\FORMS\ZSK\OUT\KYCCL_7831001422_3194_20231212_000001.zip.sig
        enc     C:\FORMS\ZSK\OUT\KYCCL_7831001422_3194_20231212_000001.zip.enc
        upload  C:\FORMS\ZSK\OUT
            upload  KYCCL_7831001422_3194_20231212_000001.zip.enc
            upload  KYCCL_7831001422_3194_20231212_000001.zip.sig
        check registration

var json = await restClient.UploadDirectoryAsync("Zadacha_137", "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)","Upload");

*/

internal class Program
{
    private static readonly RestAPI _restAPI = new();

    static int Main(string[] args)
    {
        try
        {
            if (args.Length != 3)
            {
                throw new Exception("Укажите директорию для отправки, логин и пароль.");
            }

            string path = args[0];

            if (!Directory.Exists(path))
            {
                throw new Exception("Указанная директория не существует.");
            }

            _restAPI.Login(args[1], args[2]);

            for (int retries = 1; retries <= 10; retries++)
            {
                if (Upload(path))
                {
                    foreach (var file in Directory.GetFiles(path))
                    {
                        File.Delete(file);
                    }

                    Console.WriteLine("Файлы отправлены.");
                    return 0;
                }

                Thread.Sleep(retries * 2000);
            }

            throw new Exception("Ничего не отправлено.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 1;
        }
    }

    private static bool Upload(string path)
    {
        return _restAPI.UploadDirectoryAsync("Zadacha_137",
            "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)", path).Result;
    }
}
