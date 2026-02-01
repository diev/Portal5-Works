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

namespace CryptoBot.Tasks.Load;

public class LoaderSettings
{
    public string ZipPath { get; set; } = "Download/Portal5";
    public string DocPath { get; set; } = "Download";
    public int[] Exclude { get; set; } = []; //[ 97, 104, 107, 113, 114, 123, 133, 140, 155, 156, 159, 169 ];
    public bool Decrypt { get; set; }
    public bool Delete { get; set; }
    public bool Overwrite { get; set; }
}
