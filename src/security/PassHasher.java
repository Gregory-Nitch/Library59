/**
 * Author: Gregory Nitch
 * GitHub repo: https://github.com/Gregory-Nitch/lib59
 */

package security;

import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.util.Base64;

/**
 * Used to hash passwords going into or compare hashes from a datastore.
 */
public class PassHasher {

    private MessageDigest msgDigest;
    private static final Base64.Encoder ENCODER = Base64.getEncoder();
    private static final SecureRandom SECURE_RANDOM = new SecureRandom();

    /**
     * Constructor for PassHasher object that takes in an algorithm to use,
     * also sets the MessageDigest object instance algorithm. A list of
     * available algorithms can be found here:
     * https://docs.oracle.com/en/java/javase/21/docs/specs/security/standard-names.html#messagedigest-algorithms
     * 
     * @param algorithm algorithm to use in hash production (such as SHA-256 ||
     *                  SHA-512)
     * @throws NoSuchAlgorithmException
     */
    public PassHasher(String algorithm)
            throws NoSuchAlgorithmException {
        this.msgDigest = MessageDigest.getInstance(algorithm);
    }

    /**
     * Gets the in use algorithm by the MessageDigest instance.
     * 
     * @return algorithm in use
     */
    public String getAlgorithm() {
        return msgDigest.getAlgorithm();
    }

    /**
     * Gets a new MessageDigest instance based on the new algorithm passed.
     * 
     * @param algorithm new algorithm to use
     * @throws NoSuchAlgorithmException
     */
    public void setAlgorithm(String algorithm)
            throws NoSuchAlgorithmException {
        this.msgDigest = MessageDigest.getInstance(algorithm);
    }

    /**
     * Generates byte arrays to be used as salt for hashing passwords.
     * 
     * @return a byte array of size 16 created via SecureRandom.
     */
    private byte[] genSalt() {
        byte[] salt = new byte[16];
        SECURE_RANDOM.nextBytes(salt);
        return salt;
    }

    /**
     * Generates hashes from passwords input by the user to be stored in the
     * database. Generates its own salt internally. (Initial entries only!).
     * (See hashPassToCompare())
     * 
     * @param rawPass unhashed password from user
     * @return hashed password with appended salt index 0 = password has &
     *         index 1 = saltString
     */
    public String[] hashPassToStore(String rawPass) {
        byte[] salt = genSalt();

        msgDigest.update(rawPass.getBytes(StandardCharsets.UTF_8));
        byte[] hashedBytes = msgDigest.digest(salt);

        String saltString = ENCODER.encodeToString(salt);
        String hashString = ENCODER.encodeToString(hashedBytes);

        return new String[] { hashString, saltString };
    }

    /**
     * Generates hashes from passwords input by the user to compare against
     * passwords in a data store. Passed stored salt is appended to the
     * raw password string prior to hashing.
     * 
     * @param rawPass    raw password input by the user
     * @param storedSalt salt retrieved from data store
     * @return hashed password with appended stored salt
     */
    public String hashPassToCompare(String rawPass, String storedSalt) {
        String stringToHash = rawPass + storedSalt;

        byte[] hashedBytes = msgDigest.digest(
                stringToHash.getBytes(StandardCharsets.UTF_8));

        return ENCODER.encodeToString(hashedBytes);
    }
}
