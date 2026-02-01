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

#if NET10_0_OR_GREATER
    // Native support of Async ZipFile
#else
using System.IO.Compression;

namespace Diev.Extensions.Tools;

/// <summary>
/// Before NET 10
/// </summary>
public static class AsyncZipFile
{
    /// <summary>
    /// Async ZipFile.ExtractToDirectory()
    /// </summary>
    /// <param name="zipPath"></param>
    /// <param name="extractPath"></param>
    /// <returns></returns>
    public static async Task ExtractToDirectoryAsync(string zipPath, string extractPath)
    {
        using var archive = ZipFile.OpenRead(zipPath);
        await archive.ExtractEntriesAsync(extractPath).ConfigureAwait(false);
    }

    public static async Task ExtractEntriesAsync(this ZipArchive archive, string extractPath)
    {
        foreach (var entry in archive.Entries)
        {
            await ExtractEntryAsync(entry, extractPath).ConfigureAwait(false);
        }
    }

    private static async Task ExtractEntryAsync(ZipArchiveEntry entry, string extractPath)
    {
        string filePath = Path.Combine(extractPath, entry.FullName.Replace('/', '\\'));
        var directory = Path.GetDirectoryName(filePath);

        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        using var stream = await OpenEntryForWriteAsync(filePath).ConfigureAwait(false);
        using var compressedStream = entry.Open();
        await compressedStream.CopyToAsync(stream).ConfigureAwait(false);
    }

    private static async Task<FileStream> OpenEntryForWriteAsync(string filePath)
    {
        return await Task.Run(() =>
            new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true));
    }
}
#endif
