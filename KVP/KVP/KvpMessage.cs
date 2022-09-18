using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1.Kvp
{
    public class KvpMessage
    {
        public static char STX = '\x02';
        public static char ETX = '\x03';

        Dictionary<string, KvpEntry> kvpMap = new Dictionary<string, KvpEntry>();

        private KvpMessage requestMessage;

        private class DKvpParser
        {
            String kvpStr;
            String nextKey;
            bool finished = false;

            public DKvpParser(String aKvpStr)
            {
                kvpStr = aKvpStr;
                extractFirstKey();
            }

            private bool IsKeyValid(String aKey) {
                /* Rules for keys:
                 * 1) A key name must start with a letter or underscore (a-zA-Z_)
                 * 2) A key name must consist of letters, underscore or digits (a-zA-Z_0-9)
                 * 3) A key name must have at least one character */
                
                //return Regex.IsMatch("[a-zA-Z_]*", aKey);
                return Regex.IsMatch(aKey, "[a-zA-Z_]\\w*");
                //return true;
                
            }
            private void extractFirstKey()
            {
            int index;
            index = kvpStr.IndexOf(KvpEntry.KVP_SEPARATOR_CHAR);
            if (index< 1)
                throw new KvpException("Invalid KVP string! '" + kvpStr + "'");
            nextKey = kvpStr.Substring(0, index).Trim();
            if (!IsKeyValid(nextKey))
                throw new KvpException("Invalid KVP string! '" + kvpStr + "'");

            // delete beginning part up to (and including) separator char.
            if (index + 1 < kvpStr.Length)
                kvpStr = kvpStr.Substring(index + 1);
            else
                kvpStr = "";
        }

        public Boolean isFinished()
        {
            return finished;
        }
        public KvpEntry getNextKeyValue() {
            
            String key = nextKey, value;
            int index = 0;

            /* Get the substring between the last and the next separator.
             * The last word in this substring is the next key (the part after
             * the last space). The part up to the last space is the value of
             * the current key. */
            StringBuilder buf = new StringBuilder();
            int nextSep = kvpStr.IndexOf(KvpEntry.KVP_SEPARATOR_CHAR);
            while ((nextSep > 1) && (kvpStr.ElementAt(nextSep - 1) == KvpEntry.BACKSPACE_CHAR)) {
                // A separator character preceded by backslash is a literal char (i.e. "\=" corresponds to "=")
                buf.Append(kvpStr.Substring(index, nextSep - 1))
                                .Append(KvpEntry.KVP_SEPARATOR_CHAR);
                index = nextSep + 1;
                nextSep = kvpStr.IndexOf(KvpEntry.KVP_SEPARATOR_CHAR, index);
            }

            if (nextSep < 0) {
                buf.Append(kvpStr.Substring(index));
                value = buf.ToString().Trim();
                finished = true; // all of the input KVP string is processed
            } else {
                buf.Append(kvpStr.Substring(index, nextSep));

                String str = buf.ToString();
                int spaceIndex = str.LastIndexOf(KvpEntry.SPACE_CHAR);
                if (spaceIndex < 0)
                    throw new KvpException("Invalid KVP string! '" + kvpStr + "'");
                value = str.Substring(0, spaceIndex).Replace(" +$", "");

                nextKey = str.Substring(spaceIndex + 1);
                if (!IsKeyValid(nextKey))
                    throw new KvpException("Invalid KVP string! '" + kvpStr + "'");

                // delete beginning part up to (and including) separator char.
                if (nextSep + 1 < kvpStr.Length)
                    kvpStr = kvpStr.Substring(nextSep + 1);
                else
                    kvpStr = "";
            }

            return new KvpEntry(key, value);
        }

    }

    public KvpMessage() { }

    public Dictionary<string,KvpEntry> getKvpMap()
        {
            return kvpMap;
        }

    public KvpMessage(Object messageObject) 
    {
        KvpMessage generated = KvpUtils.encode(messageObject);
        this.kvpMap = generated.kvpMap;
    }
        public KvpMessage(String kvpString)
        {
            kvpString = kvpString.Trim();
            // Append STX & ETX if necessary
            /* if (kvpString.ElementAt(0) != STX && kvpString.ElementAt(kvpString.Length - 1) != ETX) {
                 kvpString = STX + kvpString + ETX;
             }
               
             }*/
            if (kvpString.Equals(String.Empty))
                return;

            if (kvpString.ElementAt(0) == STX && kvpString.ElementAt(kvpString.Length - 1) == ETX)
            {
                kvpString = kvpString.Remove(0, 1);
                kvpString = kvpString.Remove(kvpString.Length - 1, 1);
            }
            parseKvpString(kvpString);
        }
        private void parseKvpString(String kvpStr) 
        {
            DKvpParser parser = new DKvpParser(kvpStr);
        while (!parser.isFinished()) {
            KvpEntry kvp = parser.getNextKeyValue();
        kvpMap.Add(kvp.getKey(), kvp);
        }
        }
        public override String ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append(STX);
            for (int i = 0; i < kvpMap.Count; i++)
                s.Append(kvpMap.ElementAt(i).Key.ToString() + "=" + kvpMap.ElementAt(i).Value.getValue().ToString()+ " ");
                //s.Append("[" + kvpMap.ElementAt(i).Key.ToString() + "," + kvpMap.ElementAt(i).Value.getValue().ToString() + "]");
            //s.Append("[").Append(kvpMap.ElementAt(i).Value.getKey()).Append(kvpMap.ElementAt(i).Value.getValue()).Append("]");
            //s.Append(kvpMap.ElementAt(i).ToString());
            s.Append(ETX);
            return s.ToString();
        }



    }
}
