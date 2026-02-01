#region License
/*
Copyright 2022-2026 Dmitrii Evdokimov
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

namespace Diev.Extensions.Crypto;

public class CryptoSettings
{
    /// <summary>
    /// Исполняемый файл командной строки.
    /// </summary>
    public string? Util { get; set; }

    /// <summary>
    /// Запускать ли программу видимой.
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Отпечаток своего сертификата.
    /// </summary>
    public string? My { get; set; }

    /// <summary>
    /// Архив отпечатков своего сертификата.
    /// </summary>
    public string[] MyOld { get; set; } = [];

    /// <summary>
    /// Пароль своего сертификата.
    /// </summary>
    public string? PIN { get; set; }

    /// <summary>
    /// Массив отпечатков сертификатов получателей, на которые надо шифровать.
    /// Свой программа добавит самостоятельно.
    /// </summary>
    public string[] EncryptTo { get; set; } = [];

    /// <summary>
    /// Команда подписи.
    /// {0} - исходный файл;
    /// {1} - подписанный файл;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    public string? Sign { get; set; }

    /// <summary>
    /// Команда отсоединенной подписи.
    /// {0} - исходный файл;
    /// {1} - файл отсоединенной подписи;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    public string? SignDetached { get; set; }

    /// <summary>
    /// Команда проверки и снятия подписи.
    /// {0} - исходный файл;
    /// {1} - чистый файл;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    public string? Verify { get; set; }

    /// <summary>
    /// Команда проверки отсоединенной подписи.
    /// {0} - исходный файл;
    /// {1} - файл отсоединенной подписи;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    public string? VerifyDetached { get; set; }

    /// <summary>
    /// Команда шифрования.
    /// {0} - исходный файл;
    /// {1} - зашифрованный файл;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    public string? Encrypt { get; set; }

    /// <summary>
    /// Команда расшифрования.
    /// {0} - исходный файл;
    /// {1} - расшифрованный файл;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    public string? Decrypt { get; set; }

    /// <summary>
    /// Команда подписи.
    /// {0} - исходный файл;
    /// Результат (хэш) будет на следующей строке.
    /// </summary>
    public string? CalcHash { get; set; }

    /// <summary>
    /// Команда проверки хэша.
    /// {0} - исходный файл;
    /// {1} - строка хэша.
    /// Если {1} не указан, то значение будет взято из файла {0}.hsh
    /// </summary>
    public string? VerifyHash { get; set; }
}
