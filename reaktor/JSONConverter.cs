﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace reaktor
{
    public class JSONConverter
    {
        public String toJSON(Dictionary<String, String> dict)
        {
            String json = "{";

            // iterate keys and get their values
            foreach (String key in dict.Keys)
            {
                // append to json
                json += "\"" + key + "\":\"" + dict[key].Trim() + "\",";
            }

            json += "}";

            return json;
        }

        public Dictionary<String, String> fromJSON(String json)
        {
            Dictionary<String, String> dict = new Dictionary<String, String>();

            if (!String.IsNullOrEmpty(json)) 
            {
                json = json.Trim();

                // Remove leading and trailing breakets
                if (json.StartsWith("{"))
                {
                    json = json.Remove(0, 1);
                    json = json.TrimStart();
                }
                if (json.EndsWith("}"))
                {
                    json = json.Remove(json.Length - 1);
                    json = json.TrimEnd();
                }

                String[] jsonItems = json.Split(',');

                // iterate all comma-separeted key-value-pairs
                for (int i = 0; i < jsonItems.Length; i++)
                {
                    String currentPart = jsonItems[i];
                    // check if it's a 'real' item-separator or maybe a comma inside a 'marked' string
                    if (checkForConsistentQuotes(currentPart) && currentPart.Contains(":"))
                    {
                        processKeyValuePair(dict, currentPart);
                    }
                    // we splitted inside a string, so do some error-handling
                    else
                    {
                        // add upcoming parts as long as we do not have a 'working' part
                        do
                        {
                            i++;
                            currentPart += "," + jsonItems[i];
                        }
                        while (!(checkForConsistentQuotes(currentPart) && currentPart.Contains(":")));

                        processKeyValuePair(dict, currentPart);
                    }
                }
            }

            return dict;
        }

        protected void processKeyValuePair(Dictionary<String, String> dict, String keyValuePair)
        {
            String[] keyValueArray;
            String key = String.Empty;
            String value = String.Empty;

            // split by colon
            keyValueArray = keyValuePair.Split(':');
            key = keyValueArray[0].Trim();
            value = String.Empty;

            // every not-first colon needs to be inside a string by definition
            for (int j = 1; j < keyValueArray.Length; j++)
            {
                value += keyValueArray[j].Trim();
            }

            // add the key-value-pair
            dict.Add(key, value);
        }

        protected Boolean checkForConsistentQuotes(String str)
        {
            int singleQuotes = 0;
            int doubleQuotes = 0;

            if (String.IsNullOrEmpty(str))
                return true;

            foreach (Char c in str)
            {
                if (c == '"')
                    doubleQuotes++;
                if (c == '\'')
                    singleQuotes++;
            }

            if (singleQuotes % 2 == 0 && doubleQuotes % 2 == 0)
                return true;
            else
                return false;
        }
    }
}