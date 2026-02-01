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

namespace CryptoBot.Tasks.Z137;

public class Zadacha137Settings
{
    public string UploadPath { get; set; } = "."; // "FORMS/ZSK/OUT";
    public string Xsd { get; set; } = "ClientFileXML.xsd";
    public string Zip { get; set; } = "KYCCL_7831001422_3194_{0:yyyyMMdd}_000001.zip";
    public string[] Subscribers { get; set; } = [];
}
