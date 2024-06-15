using System.Collections;
using System.Text.RegularExpressions;
using Library59.SecTools;

namespace Library59Tests;

public class PassValidatorTests
{
    [Theory]
    [InlineData("GoodPass1", true)]
    [InlineData("2short", false)]
    [InlineData("noNumberTest", false)]
    [InlineData("un$af3found", false)]
    [InlineData("12345678", false)] // No letter test
    public void TestPassValidatorDefaultSettings(string samplePass, bool expected)
    {
        PassValidator validator = new();
        bool actual = validator.CheckIfSafePass(samplePass);

        Assert.Equal(actual, expected);
    }

    // Generates test cases for 'TestPassValidatorCustomSettings()' below
    public class TestValidatorGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _validators =
        [   // Validator state to test ........................., pass, expected
            [new PassValidator(3, null, null, true, true, false), "pa1", true],
            [new PassValidator(3, null, null, true, true, false), "p1", false],
            [new PassValidator(8, null, null, false, true, false), "noNumberPass", true],
            [new PassValidator(8, null, null, false, true, false), "noNumberPas1", false],
            [new PassValidator(3, null, null, true, false, false), "12345678", true],
            [new PassValidator(3, null, null, true, false, false), "1234567E", false],
            [new PassValidator(8,
                               new Regex(".*\\$.*"),
                               new Regex("[^a-zA-Z0-9$]"),
                               true,
                               true,
                               true), "pa$$wor8", true],
            [new PassValidator(8,
                               new Regex(".*\\$.*"),
                               new Regex("[^a-zA-Z0-9$]"),
                               true,
                               true,
                               true), "passwor8", false],

        ];

        public IEnumerator<object[]> GetEnumerator() => _validators.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Theory]
    [ClassData(typeof(TestValidatorGenerator))]
    public void TestPassValidatorCustomSettings(PassValidator validator,
                                                string samplePass,
                                                bool expected)
    {
        bool actual = validator.CheckIfSafePass(samplePass);

        Assert.Equal(expected, actual);
    }


    // TODO test throwing on Validator Construction
}