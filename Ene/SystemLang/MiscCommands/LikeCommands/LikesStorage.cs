using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

using Newtonsoft.Json;
using static Ene.SystemLang.MiscCommands.LikeCommands.DefaultLikeMessages;

namespace Ene.SystemLang.MiscCommands.LikeCommands
{
    public static class LikesStorage
    {
        public static void SaveLikes(IEnumerable<Like> likes, string filePath)
        {
            string json = JsonConvert.SerializeObject(likes, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static void SaveDefaultLikeMessages(DefaultLikeMessagesData defaultLikeMessagesData, string filePath)
        {
            string json = JsonConvert.SerializeObject(defaultLikeMessagesData, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static string[] GetDirAndFilePath(string filePath)
        {
            string[] splitFilePath = filePath.Split(new char[] { '/'}, StringSplitOptions.RemoveEmptyEntries);
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
                    folderPath = folderPath.Substring(0, folderPath.Length-1);
                    wholeFilePath = folderPath + "/" + folder;
                    break;
                }
            }
            string[] trueDirAndFilePath = new string[] { folderPath, wholeFilePath};
            return trueDirAndFilePath;
        }

        public static IEnumerable<Like> LoadLikes(string filePath)
        {
            string[] dirAndFilePath = GetDirAndFilePath(filePath);
            if (!Directory.Exists(dirAndFilePath[0])) Directory.CreateDirectory(dirAndFilePath[0]);
            if (!File.Exists(dirAndFilePath[1])) Likes.InitializeLikes();
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Like>>(json);
        }

        public static DefaultLikeMessagesData LoadDefaultLikeMessages(string filePath)
        {
            string[] dirAndFilePath = GetDirAndFilePath(filePath);
            if (!Directory.Exists(dirAndFilePath[0])) Directory.CreateDirectory(dirAndFilePath[0]);
            if (!File.Exists(dirAndFilePath[1])) Likes.InitializeDefaultLikeMessages();
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<DefaultLikeMessagesData>(json);
        }
    }
}
