/**
 * Author: Gregory Nitch
 * GitHub repo: https://github.com/Gregory-Nitch/lib59
 */

package config;

import java.io.FileInputStream;
import java.io.IOException;
import java.util.HashMap;
import java.util.Map;
import java.util.Properties;

/**
 * Used to load configurations from xml documents, see sampleSettings.xml for an
 * example.
 */
public class XMLConfigLoader {

    private XMLConfigLoader() {
        throw new IllegalStateException("XMLConfigLoader is a utility class!");
    }

    /**
     * Loads passed requested properties from passed xml file.
     * 
     * @param configPath   path to xml file
     * @param requestProps properties needed from config file
     * @return map of properties(key) and their values(value)
     * @throws IOException If the file can't be found throws an IOException
     */
    public static Map<String, String> getConfigProperties(
            final String configPath,
            final String[] requestProps)
            throws IOException {

        Properties props = new Properties();
        props.loadFromXML(new FileInputStream(configPath));
        Map<String, String> configMap = new HashMap<>();

        for (int i = 0; i < requestProps.length; i++) {
            String requestedKey = requestProps[i];
            String prop = props.getProperty(requestedKey);
            configMap.put(requestedKey, prop);
        }

        return configMap;
    }

}
