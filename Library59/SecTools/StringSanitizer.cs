namespace Library59.SecTools;

public static class StringSanitizer
{
    /// <summary>
    /// Sanitizes a user variable input into SQL Server, ASSUMES ' character is already escaped.
    /// </summary>
    /// <param name="input">input from user</param>
    /// <param name="maxLength">max expected lenth of input</param>
    /// <param name="whiteList">list of characters permited for the string</param>
    /// <returns>returns a string with all unsafe characters removed (' characters are untouched
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">thrown if input is longer and max</exception>
    /// <exception cref="ArgumentNullException">thrown if input is null or becomes empty during
    /// sanitization</exception>
    public static string CheckForSQLServer(string input, int maxLength, char[] whiteList)
    {
        if (input != null && input.Length > maxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(input),
                                                  $"ERR: Input exceded max length of {maxLength}...");
        }

        if (!string.IsNullOrWhiteSpace(input))
        {
            for (int i = 0; i < input.Length; i++)
            {// Remove unsafe characters
                if (!whiteList.Contains(input[i]))
                {
                    input = input.Remove(i, 1);
                    i--;
                }
            }
            if (string.IsNullOrWhiteSpace(input))
            {// String had only unsafe characters
                throw new ArgumentNullException(nameof(input), "ERR: Entry had only unsafe characters...");
            }
            return input;
        }
        throw new ArgumentNullException(nameof(input), "ERR: Cannot sanitize an empty string...");
    }

    public static readonly char[] SQLServerWhiteList =
    [
        '\'',
        ' ',
        'a',
        'b',
        'c',
        'd',
        'e',
        'f',
        'g',
        'h',
        'i',
        'j',
        'k',
        'l',
        'm',
        'n',
        'o',
        'p',
        'q',
        'r',
        's',
        't',
        'u',
        'v',
        'w',
        'x',
        'y',
        'z',
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
        'I',
        'J',
        'K',
        'L',
        'M',
        'N',
        'O',
        'P',
        'Q',
        'R',
        'S',
        'T',
        'U',
        'V',
        'W',
        'X',
        'Y',
        'Z',
        '0',
        '1',
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8',
        '9'
    ];
}
