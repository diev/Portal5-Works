#region License
/*
Copyright 2022-2025 Dmitrii Evdokimov
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

namespace Diev.Portal5.API.Messages.Create;

/// <summary>
/// Файл включенный в сообщение.
/// </summary>
public class DraftMessageFile
{
    /// <summary>
    /// Имя файла.
    /// Example (E): "KYC_20230925.xml.zip.enc"
    /// Example (S): "KYC_20230925.xml.zip.sig"
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Тип файла (только в версии v2).
    /// Допустимы следующие типы файлов:
    /// - Document – любые данные, которые не проходят логический контроль
    /// непосредственно на ВП ЕПВВ (файлы документов, любые архивы,
    /// в т.ч.зашифрованные, неструктурированные данные и другие файлы);
    /// - SerializedWebForm – xml-файл определенной структуры,
    /// который может быть проверен ВП ЕПВВ на соответствие его схеме;
    /// - Sign – файл УКЭП, проверка которой влияет на прием/отбраковку
    /// сообщения, применяется для основной подписи сообщения и подписи
    /// машиночитаемой доверенности;
    /// - PowerOfAttorney – файл машиночитаемой доверенности;
    /// </summary>
    public string? FileType { get; set; }

    /// <summary>
    /// Признак зашифрованности файла (ДСП).
    /// Example (E): true
    /// Example (S): false
    /// </summary>
    public bool Encrypted { get; set; } = false;

    /// <summary>
    /// Идентификатор файла, подписью для которого является данный файл
    /// (заполняется только для файлов подписи *.sig).
    /// Example (E): null
    /// Example (S): "KYC_20230925.xml.zip.enc"
    /// </summary>
    public string? SignedFile { get; set; }

    /// <summary>
    /// Общий размер файла в байтах (uint64).
    /// Example (E): 3238155
    /// Example (S): 3399
    /// </summary>
    public long Size { get; set; }
}

public static class MockDraftMessageFile
{
    public static string Text(bool encrypted = false) => encrypted ?
        """
        {
            "Name": "KYC_20231031.xml.zip.enc",
            "Encrypted": true,
            "Size": 3324863,
        }
        """
        :
        """
        {
            "Name": "KYC_20231031.xml.zip.sig",
            "SignedFile": "3d9f1174-ad1d-485e-8149-109ae7353688",
            "Size": 3399,
        }
        """;
}
