/**
 * Author: Gregory Nitch
 * GitHub repo: https://github.com/Gregory-Nitch/lib59
 */

package security;

import java.security.SecureRandom;
import java.util.Base64;

/**
 * Used to generate anti cross site request forgery tokens.
 */
public class CSRFGenerator {

    private static final SecureRandom SECURE_RANDOM = new SecureRandom();
    private static final Base64.Encoder ENCODER = Base64.getEncoder()
            .withoutPadding();

    private CSRFGenerator() {
        throw new IllegalStateException("CSRFGenerator is a utility class!");
    }

    /**
     * Generates an array of 32 bytes via SecureRandom then encodes it to a
     * string.
     * 
     * @return CSRF token to be used in web form
     */
    public String genCSRF() {
        byte[] bytes = new byte[32];
        SECURE_RANDOM.nextBytes(bytes);
        return ENCODER.encodeToString(bytes);
    }
}
