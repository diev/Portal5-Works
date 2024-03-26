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

using System.Text;

using Diev.Extensions.LogFile;

namespace Diev.Extensions.Crypto;

public static class ASN1
{
    public static int BufferSize = 4096;

    /// <summary>
    /// Извлечь из файла PKCS#7 с ЭП чистый исходный файл.
    /// Криптопровайдер и проверка ЭП здесь не используются - только извлечение блока данных из формата ASN.1
    /// </summary>
    /// <param name="sourcePath">Исходный файл.</param>
    /// <param name="destinationPath">Файл с результатом.</param>
    /// <returns></returns>
    public static async Task<bool> CleanSignAsync(string sourcePath, string destinationPath)
    {
        try
        {
            using var stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, true);
            using var reader = new BinaryReader(stream);

            // type 0x30, length 0x80 or 1..4 bytes additionally
            ReadTypeLen();

            // type 0x06 length 0x09 data... - ObjectIdentifier (signedData "1.2.840.113549.1.7.2")
            if (!ReadOid(0x02))
                return false;

            // 0xA0 0x80 0xA0 0x80
            ReadTypeLen(2);

            // 0x02 0x01 0x01 - Integer (version 1)
            if (!ReadVersion())
                return false;

            // 0x31 ... - list of used algoritms
            ReadTypeLenData();

            // 0x30 0x80
            ReadTypeLen();

            // type 0x06 length 0x09 data... - ObjectIdentifier (data "1.2.840.113549.1.7.1")
            if (!ReadOid(0x01))
                return false;

            // 0xA0 0x80 0x24 0x80
            ReadTypeLen(2);

            // type 0x04 - OctetString
            if (reader.ReadByte() != 0x04)
                return false;

            // length of enclosed data (long or undefined)
            var len = ReadLength();

            if (len is null) // undefined
            {
                var start = stream.Position;
                var end = Seek(stream, [0x00, 0x00]);

                len = end - start;
                stream.Position = start;
            }

            // start of enclosed data
            using (var output = new FileStream(destinationPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, BufferSize, true))
            {
                //await stream.CopyToAsync(output); //TODO copy len bytes only
                //output.SetLength((long)len); // truncate tail and write to disk
                //await output.FlushAsync();

                var buffer = new byte[BufferSize];
                int bytesRead;
                long bytesToRead = (long)len;

                while ((bytesRead = await stream.ReadAsync(buffer.AsMemory(0, (int)Math.Min(BufferSize, bytesToRead)))) > 0)
                {
                    await output.WriteAsync(buffer.AsMemory(0, bytesRead));
                    bytesToRead -= bytesRead;
                }
            }

            var fileInfo = new FileInfo(destinationPath);

            return fileInfo.Exists && fileInfo.Length == len;

            #region local functions
            // 1..4 bytes
            long? ReadLength()
            {
                byte b = reader.ReadByte();

                // undefined: end by 0x00 0x00 bytes
                if (b == 0x80)
                    return null;

                // 1 next byte: 128..255
                if (b == 0x81)
                {
                    var v = reader.ReadByte();
                    return v;
                }

                // 2 next bytes: 256..65535
                if (b == 0x82)
                {
                    var v = reader.ReadBytes(2);
                    return
                        v[0] * 0x100 +
                        v[1];
                }

                // 3 next bytes: 65536..16777215
                if (b == 0x83)
                {
                    var v = reader.ReadBytes(3);
                    return
                        v[0] * 0x10000 +
                        v[1] * 0x100 +
                        v[2];
                }

                // 4 next bytes, 2 standards:
                // 1 .. 4 294 967 295
                // 16 777 216 .. 4 294 967 295 (4 Gb)
                if (b == 0x84)
                {
                    var v = reader.ReadBytes(4);
                    return
                        v[0] * 0x1000000 +
                        v[1] * 0x10000 +
                        v[2] * 0x100 +
                        v[3];
                }

                // this byte: 0..127
                else
                    return b;
            }

            // 06 09 then
            // 2A 86 48 86 F7 0D 01 07 02 - oid 1.2.840.113549.1.7.2 "signedData"
            // 2A 86 48 86 F7 0D 01 07 01 - oid 1.2.840.113549.1.7.1 "data"
            bool ReadOid(byte n)
            {
                var b = reader.ReadBytes(11);
                int i = 0;
                return
                    b[i++] == 0x06 && // type 06 => ObjectIdentifier
                    b[i++] == 0x09 && // length => 9 bytes

                    b[i++] == 0x2A && // data ...
                    b[i++] == 0x86 &&
                    b[i++] == 0x48 &&
                    b[i++] == 0x86 &&
                    b[i++] == 0xF7 &&
                    b[i++] == 0x0D &&
                    b[i++] == 0x01 &&
                    b[i++] == 0x07 &&
                    b[i++] == n;
            }

            // 02 01 01
            bool ReadVersion()
            {
                var b = reader.ReadBytes(3);
                int i = 0;
                return
                    b[i++] == 0x02 && // type 02 => Integer
                    b[i++] == 0x01 && // length => 1 byte
                    b[i++] == 0x01;   // data (1 => version 1)
            }

            // skip type and length
            // 30 80
            // 02 01
            void ReadTypeLen(int n = 1)
            {
                for (int i = 0; i < n; i++)
                {
                    // type
                    reader.ReadByte();

                    // length
                    ReadLength();
                }
            }

            // skip type, length and data by this length
            // 02 01 01
            void ReadTypeLenData()
            {
                // type
                reader.ReadByte();

                // length
                var len = ReadLength();

                //data
                if (len is null) // undefined
                {
                    var end = Seek(stream, [0x00, 0x00]);
                    stream.Position = end + 2;
                }
                else
                {
                    //reader.ReadBytes((int)len);
                    stream.Position += (long)len;
                }
            }
            #endregion local functions
        }
        catch
        {
            Logger.TimeLine(@$"Ошибка снятия ЭП с файла ""{sourcePath}"".");
            return false;
        }
    }


    // https://keestalkstech.com/2010/11/seek-position-of-a-string-in-a-file-or-filestream/
    // Written by Kees C. Bakker, updated on 2022-09-18

    /* EXAMPLE:
        var url = "https://keestalkstech.com/wp-content/uploads/2020/06/photo-with-xmp.jpg?1";

        using var client = new HttpClient();
        using var downloadStream = await client.GetStreamAsync(url);

        using var stream = new MemoryStream();
        await downloadStream.CopyToAsync(stream);

        stream.Position = 0;
        var enc = Encoding.UTF8;
        var start = Seek(stream, "<x:xmpmeta", enc);
        var end = Seek(stream, "<?xpacket", enc);

        stream.Position = start;
        var buffer = new byte[end - start];
        stream.Read(buffer, 0, buffer.Length);
        var xmp = enc.GetString(buffer);
    */

    public static long Seek(Stream stream, string str, Encoding encoding)
    {
        var search = encoding.GetBytes(str);
        return Seek(stream, search);
    }

    public static long Seek(Stream stream, byte[] search)
    {
        int bufferSize = BufferSize;

        if (bufferSize < search.Length * 2)
            bufferSize = search.Length * 2;

        var buffer = new byte[bufferSize];
        var size = bufferSize;
        var offset = 0;
        var position = stream.Position;

        while (true)
        {
            var r = stream.Read(buffer, offset, size);

            // when no bytes are read -- the string could not be found
            if (r <= 0)
                return -1;

            // when less then size bytes are read, we need to slice
            // the buffer to prevent reading of "previous" bytes
            ReadOnlySpan<byte> ro = buffer;

            if (r < size)
                ro = ro[..(offset + size)];

            // check if we can find our search bytes in the buffer
            var i = ro.IndexOf(search);

            if (i > -1)
                return position + i;

            // when less then size was read, we are done and found nothing
            if (r < size)
                return -1;

            // we still have bytes to read, so copy the last search
            // length to the beginning of the buffer. It might contain
            // a part of the bytes we need to search for

            offset = search.Length;
            size = bufferSize - offset;
            Array.Copy(buffer, buffer.Length - offset, buffer, 0, offset);
            position += bufferSize - offset;
        }
    }
}

/*
using System.Security.Cryptography.Pkcs;

/// <summary>
/// Извлечь из PKCS#7 с ЭП чистый исходный текст.
/// Криптопровайдер и проверка ЭП здесь не используются - только извлечение блока данных из формата ASN.1
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
*/
