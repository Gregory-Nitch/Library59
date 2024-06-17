
using Library59.SecTools;

namespace Library59Tests.SecToolsTests;

public class PassHasherTests
{
    [Theory]
    [InlineData("Samplepass1", false, true)]
    [InlineData("Samplepass1", true, false)]
    public void TestPassHasher(string samplePass, bool testVerifyFail, bool expected)
    {
        string sampleVerifyPass;
        if (testVerifyFail)
        {// Need to submit invalid to Verify Method
            sampleVerifyPass = "invalid";
        }
        else
        {
            sampleVerifyPass = samplePass;
        }

        string hash = PassHasher.HashPass(samplePass);

        bool actual = PassHasher.VerifyPass(sampleVerifyPass, hash);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestPassHasherHashingThrowsNullsOrEmpties(string emptyOrNullPass)
    {
        Assert.Throws<ArgumentNullException>(() => PassHasher.HashPass(emptyOrNullPass));
    }

    [Theory]
    [InlineData("", "dummy")]
    [InlineData("dummy", "")]
    [InlineData(null, "dummy")]
    [InlineData("dummy", null)]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void TestPassHasherVerifyThrowsNullsOrEmpties(string samplePass, string sampleHash)
    {
        Assert.Throws<ArgumentNullException>(() => PassHasher.VerifyPass(samplePass, sampleHash));
    }
}
