using Library59.SecTools;

namespace Library59Tests.SecToolsTests;

public class StringSanitizerTests
{
    [Theory]
    [InlineData("asdfASDF1234'", "asdfASDF1234''")]
    [InlineData("asdf$ASDF%1234''", "asdfASDF1234''")]
    [InlineData("asdf$A'SDF%1234''", "asdfA''SDF1234''")]
    [InlineData("a'''sdf", "a''''sdf")]
    [InlineData("asdf", "asdf")]
    public void TestCheckForSQLServer(string input, string expected)
    {
        string actual = StringSanitizer.CheckForSQLServer(input, 100);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("!@#$")]
    [InlineData(null)]
    public void TestNullCheckForSQLServer(string input)
    {
        Assert.Throws<ArgumentNullException>(() => StringSanitizer.CheckForSQLServer(input, 100));
    }

    [Theory]
    [InlineData("asdf")]
    public void TestRangeCheckForSQLServer(string input)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => StringSanitizer.CheckForSQLServer(input, 1));
    }
}
