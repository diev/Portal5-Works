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

namespace Diev.Extensions.Tools;

public static class AsyncFile
{
    public static async Task CopyAsync(string sourceFileName, string destFileName, bool overwrite)
    {
        if (overwrite)
            await DeleteAsync(destFileName).ConfigureAwait(false);

        using var stream = File.Open(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var copyStream = File.Create(destFileName);
        await stream.CopyToAsync(copyStream).ConfigureAwait(false);
    }

    public static async Task MoveAsync(string sourceFileName, string destFileName, bool overwrite)
    {
        var s = Path.GetPathRoot(sourceFileName);
        var d = Path.GetPathRoot(destFileName);

        if (s is not null && s.Equals(d, StringComparison.OrdinalIgnoreCase)) //TODO Linux
        {
            await Task.Run(() => File.Move(sourceFileName, destFileName, overwrite));
        }
        else
        {
            await CopyAsync(sourceFileName, destFileName, overwrite).ConfigureAwait(false);
            await DeleteAsync(sourceFileName).ConfigureAwait(false);
        }
    }

    public static async Task CopyToDirectoryAsync(string sourceFile, string destDirName, bool overwrite)
    {
        Directory.CreateDirectory(destDirName);
        string dst = Path.Combine(destDirName, Path.GetFileName(sourceFile));
        await CopyAsync(sourceFile, dst, overwrite).ConfigureAwait(false);
    }

    public static async Task MoveToDirectoryAsync(string sourceFile, string destDirName, bool overwrite)
    {
        Directory.CreateDirectory(destDirName);
        string dst = Path.Combine(destDirName, Path.GetFileName(sourceFile));
        await MoveAsync(sourceFile, dst, overwrite).ConfigureAwait(false);
    }

    public static async Task DeleteAsync(string path)
    {
        if (File.Exists(path))
        {
            await Task.Run(() =>
            {
                File.Delete(path);
            });
        }
    }
}
