/**
 * Author: Gregory Nitch
 * GitHub repo: https://github.com/Gregory-Nitch/lib59
 */

package security;

/**
 * Used to enforce password standards for applications. Default configureation
 * calls for atleast 8 characters, one number, one letter and rejects any
 * special character with a black list ([^a-zA-z0-9]). Password requirements
 * are only set through the constructor.
 */
public class PasswordSanitizer {

    private int requiredLength = 8;
    private String blackList = "[^a-zA-z0-9]";
    private String whiteList = null;
    private boolean requiresNumber = true;
    private boolean requiresLetter = true;
    private boolean requiresSpecial = false;
    private static final String LETTER_REGEX = ".*[A-Za-z].*";
    private static final String NUMBER_REGEX = ".*\\d.*";

    /**
     * Default constructor all settings set to default.
     */
    public PasswordSanitizer() {
        // Default settings above
    }

    /**
     * Constructor for custom settings.
     * 
     * @param requiredLength  required length for validating passwords
     * @param blackList       characters not allowed in passwords
     * @param whiteList       characters allowed in passwords (used for special
     *                        characters)
     * @param requiresNumber  indicates numbers must be in password
     * @param requiresLetter  indicates letters must be in password
     * @param requiresSpecial indicates special characters must be in password
     * @throws IllegalArgumentException if requires special is true and the
     *                                  whitelist parameter is null or empty then an
     *                                  IllegalArgumentExecption
     *                                  will be thrown
     */
    public PasswordSanitizer(int requiredLength,
            String blackList,
            String whiteList,
            boolean requiresNumber,
            boolean requiresLetter,
            boolean requiresSpecial)
            throws IllegalArgumentException {

        if (requiresSpecial && (whiteList == null || whiteList.isEmpty())) {
            throw new IllegalArgumentException(
                    "If special characters are required the whitelist " +
                            "parameter cannot be empty or null!");
        }

        this.blackList = blackList;
        this.whiteList = whiteList;
        this.requiresNumber = requiresNumber;
        this.requiresLetter = requiresLetter;
        this.requiresSpecial = requiresSpecial;
        this.requiredLength = requiredLength;
    }

    /**
     * Returns a boolean for if a given password meets the set security
     * requirements.
     * 
     * @param pass password to validate
     * @return weather or not the password meets requirements
     * @throws IllegalStateException will throw an IllegalStateException if
     *                               the whitelist is null and special characters
     *                               are required
     */
    public boolean isValidPassword(String pass)
            throws IllegalStateException {
        if (pass.length() < requiredLength) {
            return false;
        }
        boolean letterMet = true;
        boolean numberMet = true;
        boolean specialMet = true;
        boolean isUnsafe = pass.matches(blackList);

        if (requiresLetter && !pass.matches(LETTER_REGEX)) {
            letterMet = false;
        }

        if (requiresNumber && !pass.matches(NUMBER_REGEX)) {
            numberMet = false;
        }

        if (requiresSpecial && whiteList == null) {
            throw new IllegalStateException();
        } else if (requiresSpecial && !pass.matches(whiteList)) {
            specialMet = false;
        }

        return letterMet && numberMet && specialMet && !isUnsafe;
    }
}
