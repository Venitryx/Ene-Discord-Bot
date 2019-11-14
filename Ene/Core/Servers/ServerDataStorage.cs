using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Ene.Core.Servers
{
    public static class ServerDataStorage
    {
        public static void SaveVerificationInfo(IEnumerable<Server> verifiedChannel, string filePath)
        {
            string json = JsonConvert.SerializeObject(verifiedChannel, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        public static IEnumerable<Server> LoadVerificationInfo(string filePath)
        {
            string folderPath = Global.GetFolderPath(filePath);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            if (!File.Exists(filePath)) Servers.Initialize();
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Server>>(json);
        }
    }
}
