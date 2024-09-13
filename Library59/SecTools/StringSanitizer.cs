namespace Library59.SecTools;

public static class StringSanitizer
{
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

            for (int i = 0; i < input.Length; i++)
            {// Escape ' characters -> need only pairs -> ''
                if (i < input.Length - 1 && input[i] == '\'' && input[i + 1] == '\'')
                {
                    i++;
                }
                else if (i < input.Length - 1 && input[i] == '\'' && input[i + 1] != '\'')
                {
                    input = input.Insert(i, "'");
                    i++;
                }
                else if (input[i] == '\'')
                {
                    input = input.Insert(i, "'");
                    i++;
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
