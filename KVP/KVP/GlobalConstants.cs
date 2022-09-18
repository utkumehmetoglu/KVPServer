using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace KVP
{
    public static class GlobalConstants
    {
        public static string ListenIp = "0.0.0.0";
        public static int KvpPort = 1789;



        /*public static void DoConfigure()
        {
            if (!File.Exists("KvpConfig.json"))
                File.WriteAllBytes("Config.json", null);
            String Config = File.ReadAllText("Config.json");
            Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(Config);
            foreach (var field in typeof(GlobalConstants).GetFields())
            {
                if (values.ContainsKey(field.Name))
                {
                    try
                    {


                        field.SetValue(null, values[field.Name]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cannot set global constant value " + field.Name + " Caused by:\n\n\n" + ex.ToString());
                    }
                }
            }
        }*/



    }
}
