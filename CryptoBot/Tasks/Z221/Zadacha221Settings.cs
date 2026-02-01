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

namespace CryptoBot.Tasks.Z221;

public class Zadacha221Settings
{
    public string ArchivePath { get; set; } = "Archive"; // "FORMS/ZBR/OUT/Archive";
    public string UploadPath { get; set; } = "."; // "FORMS/ZBR/OUT";
    public string Zip { get; set; } = "AFN_4030702_0000000_{0:yyyyMMdd}_{1:D5}.zip";
    public string[] Subscribers { get; set; } = [];
}
