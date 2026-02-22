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

using Diev.Extensions.Tools;

namespace CryptoBot.Helpers;

internal static class Paths
{
    public static string GetTempPath(string path)
    {
        //TODO Directory.CreateTempSubdirectory(prefix);

        string temp = Path.Combine(path, "TEMP");

        if (Directory.Exists(temp))
            Directory.Delete(temp, true);

        if (Directory.Exists(temp))
            throw new Exception($"Не удалось удалить старую директорию {temp.PathQuoted()}.");

        if (!Directory.CreateDirectory(temp).Exists)
            throw new DirectoryNotFoundException($"Не удалось создать новую директорию {temp.PathQuoted()}.");

        return temp;
    }
}
