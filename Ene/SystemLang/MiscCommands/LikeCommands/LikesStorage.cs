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

        public static IEnumerable<Like> LoadLikes(string filePath)
        {
            string folderPath = Global.GetFolderPath(filePath);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            if (!File.Exists(filePath)) Likes.InitializeLikes();
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Like>>(json);
        }

        public static DefaultLikeMessagesData LoadDefaultLikeMessages(string filePath)
        {
            string folderPath = Global.GetFolderPath(filePath);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            if (!File.Exists(filePath)) Likes.InitializeDefaultLikeMessages();
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<DefaultLikeMessagesData>(json);
        }
    }
}
