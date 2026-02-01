#region License
/*
Copyright 2024-2025 Dmitrii Evdokimov
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

// The most up to date original version by Gérald Barré is available 
// on GitHub: https://github.com/meziantou/Meziantou.Framework/tree/master/src/Meziantou.Framework.Win32.CredentialManager
// NuGet package: https://www.nuget.org/packages/Meziantou.Framework.Win32.CredentialManager/
#endregion

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Win32.SafeHandles;

namespace Diev.Extensions.CredentialManager;

using static NativeMethods;

public class CredentialService : ICredentialService
{
    // Использует публичный конструктор
    public static Credential StaticRead(string targetName) =>
        new CredentialService().Read(targetName);

    public static void StaticWrite(string targetName, string userName, string? secret) =>
        new CredentialService().Write(targetName, userName, secret);

    public static Credential[] StaticEnumerate(string? filter = null) =>
        new CredentialService().Enumerate(filter);

    /// <summary>
    /// Get Credential by a mask string.
    /// </summary>
    /// <param name="targetName">String with 'Start *' or 'Name' (Windows secret only)
    /// or 'Host UserName Password' (Linux/Windows plain).</param>
    /// <returns>Credential record (type, host, username?, secret?).</returns>
    /// <exception cref="Exception"></exception>
    public Credential Read(string targetName)
    {
        var words = targetName.Split();
        int num = words.Length;

        // 'Name*' or 'Name *' or 'Name'

        if (num < 3)
        {
            // Windows secret only

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (targetName.Contains('*'))
                {
                    if (CredEnumerate(targetName, 0, out int count, out nint pCredentials))
                    {
                        if (count > 1)
                            throw new InvalidOperationException(
                                $"Windows Credential Manager has more '{targetName}' entries ({count})");

                        nint credential = Marshal.ReadIntPtr(pCredentials, 0);
                        var cred = Marshal.PtrToStructure<NativeCredential>(credential);

                        // 'Name *' => 'Name text', 'Username', 'Password'
                        return ReadFromNativeCredential(cred);
                    }

                    throw new InvalidOperationException(
                        $"Windows Credential Manager has no '{targetName}' entries");
                }

                if (CredRead(targetName, CredentialType.Generic, 0, out nint nCredPtr))
                {
                    using CriticalCredentialHandle critCred = new(nCredPtr);
                    var cred = critCred.GetNativeCredential();

                    // 'Name' => 'Name', 'Username', 'Password'
                    return ReadFromNativeCredential(cred);
                }

                throw new InvalidOperationException(
                    $"Windows Credential Manager has no '{targetName}' entries");
            }

            // Linux plain: 'Name Username' or 'Name'

            return num switch
            {
                // 'Name Username' => 'Name', 'Username', ''
                2 => new(CredentialType.Generic,
                    TargetName: words[0],
                    UserName: words[1],
                    Password: null),

                // 'Name' => 'Name', '', ''
                _ => new(CredentialType.Generic,
                    TargetName: targetName,
                    UserName: null,
                    Password: null)
            };
        }

        // Windows or Linux plain: 'Name Username Password'

        if (num == 3)
        {
            // 'Name Username Password' => 'Name', 'Username', 'Password'
            return new(CredentialType.Generic,
                TargetName: words[0],
                UserName: words[1],
                Password: words[2]);
        }

        // Windows or Linux plain: 'Name ... Username Password'

        if (num > 3)
        {
            // 'Name ... Username Password'
            string pattern = @"(.*)\s(\S*)\s(\S*)";
            var v = Regex.Matches(targetName, pattern)[0].Groups;

            if (v.Count == 4)
            {
                // 'Name ... Username Password' => 'Name ...', 'Username', 'Password'
                return new(CredentialType.Generic,
                    TargetName: v[1].Value,
                    UserName: v[2].Value,
                    Password: v[3].Value);
            }
        }

        throw new InvalidOperationException(
            $"Invalid Credential in '{targetName}' entry");
    }

    public void Write(string targetName, string userName, string? secret)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new InvalidOperationException(
                "Windows Credential Manager exists in Windows only");

        NativeCredential credential = new()
        {
            AttributeCount = 0,
            Attributes = nint.Zero,
            Comment = nint.Zero,
            TargetAlias = nint.Zero,
            Type = CredentialType.Generic,
            Persist = (uint)CredentialPersistence.LocalMachine,
            CredentialBlobSize = 0,
            TargetName = Marshal.StringToCoTaskMemUni(targetName),
            CredentialBlob = Marshal.StringToCoTaskMemUni(secret),
            UserName = Marshal.StringToCoTaskMemUni(userName ?? Environment.UserName)
        };

        if (secret is not null)
        {
            byte[] byteArray = Encoding.Unicode.GetBytes(secret);

            if (byteArray.Length > 512 * 5)
                throw new ArgumentOutOfRangeException(nameof(secret), "The secret message has exceeded 2560 bytes");

            credential.CredentialBlobSize = (uint)byteArray.Length;
        }

        bool written = CredWrite(ref credential, 0);

        Marshal.FreeCoTaskMem(credential.TargetName);
        Marshal.FreeCoTaskMem(credential.CredentialBlob);
        Marshal.FreeCoTaskMem(credential.UserName);

        if (!written)
        {
            int lastError = Marshal.GetLastWin32Error();
            throw new InvalidOperationException($"CredWrite failed with the error code {lastError}");
        }
    }

    public Credential[] Enumerate(string? filter = null)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new InvalidOperationException("Windows Credential Manager exists in Windows only");

        if (CredEnumerate(filter, 0, out int count, out nint pCredentials))
        {
            Credential[] result = new Credential[count];

            for (int n = 0; n < count; n++)
            {
                nint credential = Marshal.ReadIntPtr(pCredentials, n * Marshal.SizeOf<nint>());
                var cred = Marshal.PtrToStructure<NativeCredential>(credential);
                result[n] = ReadFromNativeCredential(cred);
            }

            return result;
        }

        int lastError = Marshal.GetLastWin32Error();
        throw new Win32Exception(lastError);
    }
    
    #region private
    private enum CredentialPersistence : uint
    {
        Session = 1,
        LocalMachine,
        Enterprise
    }

    private static Credential ReadFromNativeCredential(NativeCredential credential)
    {
        string targetName = Marshal.PtrToStringUni(credential.TargetName)!;
        string? userName = Marshal.PtrToStringUni(credential.UserName);

        if (credential.CredentialBlob == nint.Zero)
        {
            return new Credential(credential.Type, targetName, userName, null);
        }

        string secret = Marshal.PtrToStringUni(credential.CredentialBlob, (int)credential.CredentialBlobSize / 2);
        return new Credential(credential.Type, targetName, userName, secret);
    }

    sealed class CriticalCredentialHandle : CriticalHandleZeroOrMinusOneIsInvalid
    {
        public CriticalCredentialHandle(nint preexistingHandle)
        {
            SetHandle(preexistingHandle);
        }

        public NativeCredential GetNativeCredential()
        {
            return IsInvalid
                ? throw new InvalidOperationException("Invalid CriticalHandle!")
                : Marshal.PtrToStructure<NativeCredential>(handle);
        }

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
            {
                return false;
            }

            CredFree(handle);
            SetHandleAsInvalid();

            return true;
        }
    }
    #endregion private
}
