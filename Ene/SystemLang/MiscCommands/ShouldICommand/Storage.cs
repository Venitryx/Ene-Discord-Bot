using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

using Newtonsoft.Json;

namespace Ene.SystemLang.MiscCommands.ShouldICommand
{
    public class Storage
    {
        public static void SaveCommandInfo(IEnumerable<ShouldI> commands, string filePath)
        {
            string json = JsonConvert.SerializeObject(commands, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        public static IEnumerable<ShouldI> LoadCommandInfo(string filePath)
        {
            string folderPath = Global.GetFolderPath(filePath);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            if (!File.Exists(filePath)) Commands.Initialize();
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<ShouldI>>(json);
        }
    }
}
