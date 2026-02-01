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

using Diev.Extensions.CredentialManager;

namespace Diev.ExtensionsTests.Credentials
{
    [TestClass()]
    public class CredentialManagerTests
    {
        [TestMethod()]
        public void ReadCredentialPortalTest()
        {
            string test = "http://localhost username password";
            var cred = CredentialService.StaticRead(test);

            if (!cred.TargetName.Equals("http://localhost"))
            {
                Assert.Fail("Read host");
            }

            if (!cred.UserName!.Equals("username"))
            {
                Assert.Fail("Read username");
            }

            if (!cred.Password!.Equals("password"))
            {
                Assert.Fail("Read password");
            }
        }

        [TestMethod()]
        public void ReadCredentialSmtpTlsTest()
        {
            string test = "gmail.com 589 tls sen@der password";
            var cred = CredentialService.StaticRead(test);

            if (!cred.TargetName.Equals("gmail.com 589 tls"))
            {
                Assert.Fail("Read SMTP settings");
            }

            if (!cred.UserName!.Equals("sen@der"))
            {
                Assert.Fail("Read sen@der");
            }

            if (!cred.Password!.Equals("password"))
            {
                Assert.Fail("Read password");
            }
        }

        [TestMethod()]
        public void ReadCredentialSmtpNoTlsTest()
        {
            string test = "gmail.com 25 sender password";
            var cred = CredentialService.StaticRead(test);

            if (!cred.TargetName.Equals("gmail.com 25"))
            {
                Assert.Fail("Read SMTP settings");
            }

            if (!cred.UserName!.Equals("sender"))
            {
                Assert.Fail("Read sender");
            }

            if (!cred.Password!.Equals("password"))
            {
                Assert.Fail("Read password");
            }
        }
    }
}
