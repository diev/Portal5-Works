﻿
#region License
/*
Copyright 2022-2024 Dmitrii Evdokimov
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

public interface ICrypto
{
    string DecryptCommand { get; set; }
    string EncryptCommand { get; set; }
    string Exe { get; set; }
    string My { get; set; }
    string[]? MyOld { get; set; }
    string SignCommand { get; set; }
    string SignDetachedCommand { get; set; }
    string VerifyCommand { get; set; }
    string VerifyDetachedCommand { get; set; }
    bool Visible { get; set; }

    Task<bool> DecryptFileAsync(string file, string resultFile);
    Task<bool> EncryptFileAsync(string file, string resultFile, string? to = null);
    Task<bool> SignDetachedFileAsync(string file, string resultFile);
    Task<bool> SignFileAsync(string file, string resultFile);
    Task<bool> VerifyDetachedFileAsync(string file, string signFile);
    Task<bool> VerifyFileAsync(string file, string resultFile);
}
