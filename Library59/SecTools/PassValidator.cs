using System.Text.RegularExpressions;

namespace Library59.SecTools;

/// <summary>
/// Used to enforce password standards can be constructed to taliored requirements or can be used
/// with default values.
/// </summary>
public partial class PassValidator
{
    // Default settings
    private int RequiredLength = 8;
    private bool RequiresNumber = true;
    private bool RequiresLetter = true;
    private bool RequiresSpecial = false;
    private Regex? WhiteList = null;

    private Regex BlackList = BlackListRegex();
    [GeneratedRegex("[^a-zA-Z0-9]")]
    private static partial Regex BlackListRegex();

    private Regex letterRegex = LetterRegex();
    [GeneratedRegex(".*[a-zA-Z].*")]
    private static partial Regex LetterRegex();

    private Regex numberRegex = NumberRegex();
    [GeneratedRegex(".*\\d.*")]
    private static partial Regex NumberRegex();

    /// <summary>
    /// Constructs a with default settings required length = 8, number required, letter required,
    /// no special required, null whitelist, blacklist = [^a-zA-Z0-9].
    /// </summary>
    public PassValidator() { }

    /// <summary>
    /// Constructs a validator with custom requirements.
    /// </summary>
    /// <param name="reqLen">Required length of passwords</param>
    /// <param name="whiteList">Regex for whitelisting characters must be supplied for 
    /// settings that call for special characters</param>
    /// <param name="blackList">Regex for blacklisting, default = [^a-zA-Z0-9] if not
    /// supplied</param>
    /// <param name="reqsNum">Forces numbers in passwords</param>
    /// <param name="reqsLet">Forces letters in passwords</param>
    /// <param name="reqsSpec">Forces special characters in passwords (see whitelist)</param>
    /// <exception cref="ArgumentNullException">Thrown if specials are required but not whitelist
    /// Regex is passed</exception>
    /// <exception cref="ArgumentException">Thrown if specials are not required but a whitelist
    /// has been supplied</exception>
    public PassValidator(int reqLen,
                         Regex? whiteList,
                         Regex? blackList,
                         bool reqsNum,
                         bool reqsLet,
                         bool reqsSpec)
    {
        RequiredLength = reqLen;
        RequiresNumber = reqsNum;
        RequiresLetter = reqsLet;
        RequiresSpecial = reqsSpec;

        if (RequiresSpecial && whiteList == null)
        {
            throw new ArgumentNullException(nameof(whiteList), "Error: PassValidator is set to " +
             "test for special characters but the whiteList Regex is null...");
        }
        else if (!RequiresSpecial && whiteList != null)
        {
            throw new ArgumentException("Error: PassValidator is not set to test for special " +
                "characters but the whiteList Regex is not null, whiteList would NOT be used" +
                "...");
        }
        else
        {
            WhiteList = whiteList;
        }

        if (WhiteList != null && blackList == null)
        {
            throw new ArgumentNullException(nameof(blackList), "Error: Cannot use a whitelist " +
            "with the default blacklist...");
        }
        else if (blackList != null)
        {
            BlackList = blackList;
        }
    }

    /// <summary>
    /// Checks if given raw password meets application requriements.
    /// </summary>
    /// <param name="rawPass">password to check</param>
    /// <returns>true if password is safe -> (OK), false if unsafe -> (REJECT INPUT)</returns>
    public bool CheckIfSafePass(string rawPass)
    {
        if (string.IsNullOrWhiteSpace(rawPass) || rawPass.Length < RequiredLength)
        {
            return false;
        }
        else if (BlackList.IsMatch(rawPass))
        {
            return false;
        }

        bool letterFlag = true;
        bool numberFlag = true;
        bool specialFlag = true;

        if (RequiresLetter && !letterRegex.IsMatch(rawPass))
        {
            letterFlag = false;
        }
        else if (!RequiresLetter && letterRegex.IsMatch(rawPass))
        {
            letterFlag = false;
        }

        if (RequiresNumber && !numberRegex.IsMatch(rawPass))
        {
            numberFlag = false;
        }
        else if (!RequiresNumber && numberRegex.IsMatch(rawPass))
        {
            numberFlag = false;
        }

        if (RequiresSpecial && WhiteList != null && !WhiteList.IsMatch(rawPass))
        {
            specialFlag = false;
        }

        return letterFlag && numberFlag && specialFlag;
    }
}
