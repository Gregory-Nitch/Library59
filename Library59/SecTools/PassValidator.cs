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
    private Regex? Whitelist = null;

    private Regex Blacklist = BlackListRegex();
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
    /// <param name="whitelist">Regex for whitelisting characters must be supplied for 
    /// settings that call for special characters</param>
    /// <param name="blacklist">Regex for blacklisting, default = [^a-zA-Z0-9] if not
    /// supplied</param>
    /// <param name="reqsNum">Forces numbers in passwords</param>
    /// <param name="reqsLet">Forces letters in passwords</param>
    /// <param name="reqsSpec">Forces special characters in passwords (see whitelist)</param>
    /// <exception cref="ArgumentNullException">Thrown if specials are required but not whitelist
    /// Regex is passed</exception>
    /// <exception cref="ArgumentException">Thrown if specials are not required but a whitelist
    /// has been supplied</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if required length <= 0</exception>
    public PassValidator(int reqLen,
                         Regex? whitelist,
                         Regex? blacklist,
                         bool reqsNum,
                         bool reqsLet,
                         bool reqsSpec)
    {

        if (reqLen < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(reqLen), "ERR: Required password " +
            $"length cannot be <= 0 (and ideally should not be less than 8) actual = {reqLen}...");
        }
        else if (!reqsNum && !reqsLet && !reqsSpec)
        {
            throw new ArgumentException("ERR: All password requirements are false, atleast one " +
            "must be true...");
        }

        RequiredLength = reqLen;
        RequiresNumber = reqsNum;
        RequiresLetter = reqsLet;
        RequiresSpecial = reqsSpec;

        if (RequiresSpecial && whitelist == null)
        {
            throw new ArgumentNullException(nameof(whitelist), "ERR: PassValidator is set to " +
             "test for special characters but the whiteList Regex is null...");
        }
        else if (!RequiresSpecial && whitelist != null)
        {
            throw new ArgumentException("ERR: PassValidator is not set to test for special " +
                "characters but the whiteList Regex is not null, whiteList would NOT be used" +
                "...");
        }
        else
        {
            Whitelist = whitelist;
        }

        if (Whitelist != null && blacklist == null)
        {
            throw new ArgumentNullException(nameof(blacklist), "ERR: Cannot use a whitelist " +
            "with the default blacklist...");
        }
        else if (blacklist != null)
        {
            Blacklist = blacklist;
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
        else if (Blacklist.IsMatch(rawPass))
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

        if (RequiresSpecial && Whitelist != null && !Whitelist.IsMatch(rawPass))
        {
            specialFlag = false;
        }

        return letterFlag && numberFlag && specialFlag;
    }
}
