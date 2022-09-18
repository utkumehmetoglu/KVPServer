using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Kvp
{
    internal class KvpExtractor
    {

        string Buffer;

        public KvpExtractor(string Buffer)
        {
            this.Buffer = Buffer;
        }

        public KvpExtractor()
        {
            
        }

        public int GetStxCount()
        {
            int retval = 0;
            for(int i = 0; i < Buffer.Length; i++)
            {
                if (Buffer[i].Equals(KvpMessage.STX))
                    retval++;
            }
            return retval;
        }
        public int GetEtxCount()
        {
            int retval = 0;
            for (int i = 0; i < Buffer.Length; i++)
            {
                if (Buffer[i].Equals(KvpMessage.ETX))
                    retval++;
            }
            return retval;
        }

        /*public bool HasHead()
        {
            if (GetEtxCount() + 1 == GetStxCount())
                return true;
            return false;
        }
        public bool HasTail()
        {
            if (GetStxCount()+1 == GetEtxCount())
                return true;
            return false;
        }*/


        public bool HasHead()
        {
            if (Buffer.Length < 2)
                return false;
            int i = Buffer.Length-1;
            while (i >= 0)
            {
                if (Buffer[i].Equals(KvpMessage.STX))
                    return true;
                else if (Buffer[i].Equals(KvpMessage.ETX))
                    return false;
                i--;
            }
            return false;
        }

        public bool HasTail()
        {
            if (Buffer.Length < 2)
                return false;
            int i = 0;
            while (i < Buffer.Length)
            {
                if (Buffer[i].Equals(KvpMessage.ETX))
                    return true;
                else if (Buffer[i].Equals(KvpMessage.STX))
                    return false;
                i++;
            }
            return false;
        }


        public String getTail()
        {
            //if (HasTail() == false)
             //   throw new KvpException("Corrupt Message");
            int i = 0;
            while (Buffer[i] != KvpMessage.ETX)
            {
                i++;
            }
            return Buffer.Substring(0, i+1);
            
        }
        public String getHead()
        {
            if (HasHead() == false)
                throw new KvpException("Corrupt Message");
            int i = Buffer.Length - 1;
            int k = 0;
            while (Buffer[i] != KvpMessage.ETX)
            {
                k++;
                i--;
            }
            return Buffer.Substring(i+1, k);

        }


/*     public List<string> ExtractMessages(string Buffer)
        {

            List<string> messages = new List<string>();
            StringBuilder tempmessage = new StringBuilder();
            bool readmode = false;
            for (int i = 0; i < Buffer.Length; i++)
            {
                if (Buffer[i] == KvpMessage.STX)
                    readmode = true;

                if (readmode == true)
                {
                    tempmessage.Append(Buffer[i]);

                }
                if (Buffer[i] == KvpMessage.ETX)
                {
                    readmode = false;
                    messages.Add(tempmessage.ToString());
                    tempmessage.Clear();
                }


            }

            return messages;


        }
*/
        public List<KvpMessage> ExtractMessages(string Buffer)
        {

            List<KvpMessage> messages = new List<KvpMessage>();
            StringBuilder tempmessage = new StringBuilder();
            bool readmode = false;
            for (int i = 0; i < Buffer.Length; i++)
            {
                if (Buffer[i] == KvpMessage.STX)
                    readmode = true;

                if (readmode == true)
                {
                    tempmessage.Append(Buffer[i]);

                }
                if (Buffer[i] == KvpMessage.ETX)
                {
                    readmode = false;
                    messages.Add(new KvpMessage(tempmessage.ToString()));
                    tempmessage.Clear();
                }


            }

            return messages;


        }

        public List<string> ExtractMessages()
        {

            List<string> messages = new List<string>();
            StringBuilder tempmessage = new StringBuilder();
            bool readmode = false;
            for (int i = 0; i < Buffer.Length; i++)
            {
                if (Buffer[i] == KvpMessage.STX)
                    readmode = true;

                if (readmode == true)
                {
                    tempmessage.Append(Buffer[i]);

                }
                if (Buffer[i] == KvpMessage.ETX)
                {
                    readmode = false;
                    messages.Add(tempmessage.ToString());
                    tempmessage.Clear();
                }


            }

            return messages;


        }


    }
}
