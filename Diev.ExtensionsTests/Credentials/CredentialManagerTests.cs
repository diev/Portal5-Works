namespace Diev.Extensions.Credentials.Tests
{
    [TestClass()]
    public class CredentialManagerTests
    {
        [TestMethod()]
        public void ReadCredentialPortalTest()
        {
            string test = "http://localhost username password";
            var cred = CredentialManager.ReadCredential(test);

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
            var cred = CredentialManager.ReadCredential(test);

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
            var cred = CredentialManager.ReadCredential(test);

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