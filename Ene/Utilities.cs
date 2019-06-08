using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace Ene
{
    class Utilities
    {
        private static Dictionary<string, string> alerts;

        static Utilities()
        {
            string json = File.ReadAllText("SystemLang/alerts.json");
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            alerts = data.ToObject<Dictionary<string, string>>();
        }

        public static string GetAlert(string key)
        {
            if (alerts.ContainsKey(key)) return alerts[key];
            return "";
        }

        
        public static string GetAlert(string key, params object[] parameter)
        {
            if (alerts.ContainsKey(key)) return string.Format(alerts[key], parameter);
            return "";
        }
        
       
        //will add this back if code doesn't work
        /*
        public static string GetFormattedAlert(string key, params object[] parameter)
        {
            if (alerts.ContainsKey(key)) return String.Format(alerts[key], parameter);
            return "";
        }

        public static string GetFormattedAlert(string key, object parameter)
        {
            return GetFormattedAlert(key, new object[] {parameter});
        }
        */
    }
}
