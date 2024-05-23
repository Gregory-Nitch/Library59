/**
 * Author: Gregory Nitch
 * GitHub repo: https://github.com/Gregory-Nitch/lib59
 */

package security;

import java.security.SecureRandom;

/**
 * Used to generate two factor authorization codes for users.
 */
public class TFAGenerator {

    private int requiredLength = 6;
    private static final String DIGITS = "1234567890";
    private static final SecureRandom SECURE_RANDOM = new SecureRandom();

    /**
     * Default constructor sets TFA code length to 6.
     */
    public TFAGenerator() {
        // Uses default settings
    }

    /**
     * Constructor to set required length of the generator.
     * 
     * @param requiredLength required length of generated TFA codes
     */
    public TFAGenerator(int requiredLength) {
        if (requiredLength < 0) {
            throw new IllegalArgumentException("TFA code required length cannot" +
                    " be less than 0!");
        }
        this.requiredLength = requiredLength;
    }

    /**
     * Generates two factor authentication codes with length determined by
     * the class instance. Codes are created with SecureRandom.
     * 
     * @return TFA code for user
     */
    public String genTFACode() {
        StringBuilder builder = new StringBuilder();

        for (int i = 0; i < requiredLength; i++) {
            int randomIdx = SECURE_RANDOM.nextInt(DIGITS.length());
            builder.append(DIGITS.charAt(randomIdx));
        }

        return builder.toString();
    }
}
