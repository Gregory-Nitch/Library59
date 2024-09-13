namespace Library59.SecTools;

public static class StringSanitizer
{
    /// <summary>
    /// Sanitizes a user variable input into SQL Server, ASSUMES ' character is already escaped.
    /// </summary>
    /// <param name="input">input from user</param>
    /// <param name="maxLength">max expected lenth of input</param>
    /// <returns>returns a string with all unsafe characters removed (' characters are untouched
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">thrown if input is longer and max</exception>
    /// <exception cref="ArgumentNullException">thrown if input is null or becomes empty during
    /// sanitization</exception>
    public static string CheckForSQLServer(string input, int maxLength)
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
                char c = input[i];
                if ((c < 'A' || c > 'Z') &&
                    (c < 'a' || c > 'z') &&
                    c != '\'' &&
                    (c < '0' || c > '9'))
                {
                    input = input.Remove(i, 1);
                    i--;
                }
            }
            if (string.IsNullOrWhiteSpace(input))
            {// String had only unsafe characters
                throw new ArgumentNullException(nameof(input));
            }
            return input;
        }
        throw new ArgumentNullException(nameof(input));
    }
}
