using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Kvp
{
    public class KvpEntry 
    {
        public static char KVP_SEPARATOR_CHAR = '=';
        public static char SPACE_CHAR = ' ';
        public static char BACKSPACE_CHAR = '\\';
        public static String KVP_SEPARATOR = "=";
        public static String KVP_SEPARATOR_ESCAPED = "\\=";

        String key { get; set; }
        
        String value { get; set; }
        public KvpEntry(String aKey, String aValue)
        {
            this.key = aKey;
            this.value = aValue;
        }

        public String getKey()
        {
            return key;
        }

        public String getValue()
        {
            return value;
        }

        public static String makeKvpString(String aKey, String aValue)
        {
            try
            {
                return new KvpEntry(aKey, aValue).ToString();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void setKey(String aKey) 
        {
            String message = "Invalid KVP key!";
        if ((aKey == null) || (aKey.Length == 0))
            throw new KvpException(message);
        if (aKey.IndexOf(KVP_SEPARATOR_CHAR) >= 0)
            throw new KvpException(message);
        if (aKey.IndexOf(SPACE_CHAR) >= 0)
            throw new KvpException(message);
        key = aKey;
        }

          public String setValue(String aValue)
           {
             String tmpValue = value;
              value = aValue;
             return tmpValue;
          }

    public override String ToString()
    {
            return key + KVP_SEPARATOR + value.Replace(KVP_SEPARATOR, KVP_SEPARATOR_ESCAPED) + value;
    }

    }
}
