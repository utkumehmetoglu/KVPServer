using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Kvp
{
    internal class KvpUtils
    {
        public static KvpMessage encode(Object messageObject) 
        {
        if (messageObject == null) {
            throw new KvpException("Invalid KVP object, given object cannot be null!");
             }

        return encode(messageObject, false);
         }


 
      public static KvpMessage encode(Object messageObject, bool ignoreNull)
      {
        if (messageObject == null) {
        throw new KvpException("Invalid KVP object, given object cannot be null!");
       }

        return generateKvpMessageFromObject(messageObject, ignoreNull);
       }
       
       private static KvpMessage generateKvpMessageFromObject(Object messageObject, bool ignoreNull)
       {
            KvpMessage kvpMessage = new KvpMessage();
            return kvpMessage;
            
       }

    }
}

