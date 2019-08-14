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

        public static string[] GetDirAndFilePath(string filePath)
        {
            string[] splitFilePath = filePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string folderPath = "";
            string wholeFilePath = "";
            foreach (string folder in splitFilePath)
            {
                if (!folder.Contains('.'))
                {
                    folderPath += (folder + '/');
                }
                else
                {
                    folderPath = folderPath.Substring(0, folderPath.Length - 1);
                    wholeFilePath = folderPath + "/" + folder;
                    break;
                }
            }
            string[] trueDirAndFilePath = new string[] { folderPath, wholeFilePath };
            return trueDirAndFilePath;
        }
        public static IEnumerable<ShouldI> LoadCommandInfo(string filePath)
        {
            string[] dirAndFilePath = GetDirAndFilePath(filePath);
            if (!Directory.Exists(dirAndFilePath[0])) Directory.CreateDirectory(dirAndFilePath[0]);
            if (!File.Exists(dirAndFilePath[1])) Commands.Initialize();
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<ShouldI>>(json);
        }
    }
}
