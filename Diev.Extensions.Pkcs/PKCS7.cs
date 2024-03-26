#region License
/*
Copyright 2024 Dmitrii Evdokimov
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

using System.Security.Cryptography.Pkcs;

namespace Diev.Extensions.Pkcs;

public class PKCS7
{
    /// <summary>
    /// Извлечь из PKCS#7 с ЭП чистый исходный текст.
    /// Криптопровайдер и проверка ЭП здесь не используются - только извлечение блока данных из формата ASN.1.
    /// Требуется дополнительный пакет System.Security.Cryptography.Pkcs
    /// </summary>
    /// <param name="data">Массив байтов с сообщением в формате PKCS#7.</param>
    /// <returns>Массив байтов с исходным сообщением без ЭП.</returns>
    public static byte[] CleanSign(byte[] data)
    {
        var signedCms = new SignedCms();
        signedCms.Decode(data);

        return signedCms.ContentInfo.Content;
    }

    /// <summary>
    /// Извлечь из файла PKCS#7 с ЭП чистый исходный файл.
    /// Криптопровайдер и проверка ЭП здесь не используются - только извлечение блока данных из формата ASN.1
    /// </summary>
    /// <param name="src">Исходный файл.</param>
    /// <param name="dst">Файл с результатом.</param>
    /// <returns></returns>
    public static async Task CleanSignAsync(string src, string dst)
    {
        byte[] data = await File.ReadAllBytesAsync(src);
        byte[] data2 = CleanSign(data);
        await File.WriteAllBytesAsync(dst, data2);
    }
}
