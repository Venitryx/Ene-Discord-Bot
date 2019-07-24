using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace Ene
{
    class Utilities
    {
        private static Dictionary<string, string> messages;

        static Utilities()
        {
            string json = File.ReadAllText("Resources/SystemLang/messages.json");
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            messages = data.ToObject<Dictionary<string, string>>();
        }

        public static string GetMessage(string key)
        {
            if (messages.ContainsKey(key)) return messages[key];
            return "";
        }
        
        public static string GetMessage(string key, params object[] parameter)
        {
            if (messages.ContainsKey(key)) return string.Format(messages[key], parameter);
            return "";
        }
    }
}
