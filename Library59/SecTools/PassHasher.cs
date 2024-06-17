
using System.Security.Cryptography;

namespace Library59.SecTools;

/// <summary>
/// Hashes passwords and verifies them based on the SHA512 algorithm and PBKDF2.
/// </summary>
public class PassHasher
{
    private const int _saltSize = 16;
    private const int _keySize = 64;
    private const int _iterations = 100_000;
    private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;
    private const char _delimiter = ':';

    /// <summary>
    /// Hashes input passwords and salts them into a set delimited by ':', sanatize input before 
    /// use.
    /// </summary>
    /// <param name="rawPass">Raw password input by user (sanatize user input before this)</param>
    /// <returns>A hash set of hash:salt:iters:algo as a string</returns>
    /// <exception cref="ArgumentNullException">Thrown if rawPass is null || empty</exception>
    public static string HashPass(string rawPass)
    {
        if (string.IsNullOrWhiteSpace(rawPass))
        {
            throw new ArgumentNullException(nameof(rawPass),
                "ERR: password to hash cannot be null || empty...");
        }

        byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(rawPass, salt, _iterations, _algorithm, _keySize);

        return string.Join(_delimiter,
                           Convert.ToHexString(hash),
                           Convert.ToHexString(salt),
                           _iterations,
                           _algorithm);
    }

    /// <summary>
    /// Verifies a passed password against a retrieved hash set.
    /// </summary>
    /// <param name="rawPass">Input from user (sanatize before this)</param>
    /// <param name="hashSet">Hash set from data store</param>
    /// <returns>True if rawPass matches the password in hashSet</returns>
    /// <exception cref="ArgumentNullException">Thrown if rawPass || hashSet are null || empty
    /// </exception>
    public static bool VerifyPass(string rawPass, string hashSet)
    {
        if (string.IsNullOrWhiteSpace(rawPass))
        {
            throw new ArgumentNullException(nameof(rawPass),
                "ERR: password to verify cannot be null || empty...");
        }
        else if (string.IsNullOrWhiteSpace(hashSet))
        {
            throw new ArgumentNullException(nameof(rawPass),
                "ERR: hashSet cannot be null || empty...");
        }

        string[] hashSegments = hashSet.Split(_delimiter);
        byte[] hash = Convert.FromHexString(hashSegments[0]);
        byte[] salt = Convert.FromHexString(hashSegments[1]);
        int iterations = int.Parse(hashSegments[2]);
        HashAlgorithmName algorithm = new(hashSegments[3]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(rawPass,
                                                     salt,
                                                     iterations,
                                                     algorithm,
                                                     hash.Length);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}
