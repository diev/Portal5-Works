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

namespace Diev.Portal5.API;

public class Files
{
    /// <summary>
    /// Файлы включенные в сообщение.
    /// Устанавливается в ответном сообщении.
    /// Example: "d55cdbbb-e41f-4a2a-8967-78e2a6e15701"
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Имя файла.
    /// Example (E): "KYC_20230925.xml.zip.enc"
    /// Example (S): "KYC_20230925.xml.zip.sig"
    /// 
    /// Example (E): "KYCCL_7831001422_3194_20231102_000001.zip.enc"
    /// Example (S): "KYCCL_7831001422_3194_20231102_000001.zip.sig"
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Указывается тип файла (только в версии v2).
    /// Допустимы следующие типы файлов:
    /// - Document – любые данные, которые не проходят логический контроль непосредственно на ВП ЕПВВ(файлы документов, любые архивы, в т.ч.зашифрованные, неструктурированные данные и другие файлы);
    /// - SerializedWebForm – xml-файл определенной структуры, который может быть проверен ВП ЕПВВ на соответствие его схеме;
    /// - Sign – файл УКЭП, проверка которой влияет на прием/отбраковку сообщения, применяется для основной подписи сообщения и подписи машиночитаемой доверенности;
    /// - PowerOfAttorney – файл машиночитаемой доверенности.
    /// </summary>
    public string? FileType { get; set; }

    /// <summary>
    /// Описание файла (необязательное поле, для запросов и предписаний из Банка России содержит имя файла с расширением, однако может содержать запрещённые символы Windows).
    /// Example: null
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Признак зашифрованности файла (ДСП).
    /// Example (E): true
    /// Example (S): false
    /// </summary>
    public bool Encrypted { get; set; } = false;

    /// <summary>
    /// Имя и расширение файла с данными, подписью для которого является данный файл (заполняется только для файлов подписи *.sig).
    /// Example (E): null
    /// Example (S): "d55cdbbb-e41f-4a2a-8967-78e2a6e15701"
    /// </summary>
    public string? SignedFile { get; set; }

    /// <summary>
    /// Указывается “http” или “aspera”. Необязательный параметр, указывающий тип репозитория, в который пользователь будет загружать файл.
    /// Example: "http"
    /// </summary>
    public string? RepositoryType { get; set; }

    /// <summary>
    /// Размер отправляемого файла в байтах. Имеет формат int64 (т.е. signed 64 bits).
    /// Example (E): 3238155
    /// Example (S): 3399
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// Информация о характеристиках репозитория, в который будет загружен файл.
    /// Example: [{
    /// "RepositoryType": "http",
    /// "Host": "https://portal5.cbr.ru",
    /// "Port": 81,
    /// "Path": "back/rapi2/messages/1d018a30-de5d-4f20-9eb9-b0890102f4be/files/14c80cb0-135d-42e2-b5a8-f1b04108d4ba/download"
    /// }]
    /// </summary>
    public List<RepositoryInfo>? RepositoryInfo { get; set; }
}

/*
[
   {
       "Id": "3d9f1174-ad1d-485e-8149-109ae7353688",
       "Name": "KYC_20231031.xml.zip.enc",
       "Description": null,
       "Encrypted": true,
       "SignedFile": null,
       "Size": 3324863,
       "RepositoryInfo": [
           {
               "RepositoryType": "http",
               "Host": "https://portal5.cbr.ru",
               "Port": 81,
               "Path": "back/rapi2/messages/6fbc3cf9-b48c-4a15-ba8c-b0ad002c489c/files/3d9f1174-ad1d-485e-8149-109ae7353688/download"
           }
       ]
   },
   {
       "Id": "d2a087db-55b8-4348-9ec7-5313c935ec41",
       "Name": "KYC_20231031.xml.zip.sig",
       "Description": null,
       "Encrypted": false,
       "SignedFile": "3d9f1174-ad1d-485e-8149-109ae7353688",
       "Size": 3399,
       "RepositoryInfo": [
           {
               "RepositoryType": "http",
               "Host": "https://portal5.cbr.ru",
               "Port": 81,
               "Path": "back/rapi2/messages/6fbc3cf9-b48c-4a15-ba8c-b0ad002c489c/files/d2a087db-55b8-4348-9ec7-5313c935ec41/download"
           }
       ]
   }
]
*/
