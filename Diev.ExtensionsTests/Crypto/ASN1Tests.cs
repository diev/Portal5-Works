namespace Diev.Extensions.Crypto.Tests;

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
