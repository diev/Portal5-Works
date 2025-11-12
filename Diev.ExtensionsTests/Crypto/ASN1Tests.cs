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

using Diev.Extensions.Crypto;

namespace Diev.ExtensionsTests.Crypto;

[TestClass()]
public class ASN1Tests
{
    [TestMethod()]
    public void OidToBytesTest()
    {
        string oid = "1.2.840.113549.1.7.2";
        var b = ASN1.Oid(oid);
        int i = 0;

        if (!(
                b[i++] == 0x2A &&
                b[i++] == 0x86 &&
                b[i++] == 0x48 &&
                b[i++] == 0x86 &&
                b[i++] == 0xF7 &&
                b[i++] == 0x0D &&
                b[i++] == 0x01 &&
                b[i++] == 0x07 &&
                b[i++] == 0x02))
        {
            Assert.Fail("Convert string -> bytes.");
        }
    }

    [TestMethod()]
    public void OidToStringTest()
    {
        string oid = "1.2.840.113549.1.7.2";
        byte[] bytes = [0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x07, 0x02];

        if (ASN1.Oid(bytes) != oid)
        {
            Assert.Fail("Convert bytes -> string.");
        }
    }
}
