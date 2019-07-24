using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

using Newtonsoft.Json;

namespace Ene.SystemLang.MiscCommands.DoYouLikeCommand
{
    public static class LikesStorage
    {
        
        //Save all userAccounts
        public static void SaveLikes(IEnumerable<Like> likes, string filePath)
        {
            string json = JsonConvert.SerializeObject(likes, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        //Get all userAccounts
        public static IEnumerable<Like> LoadLikes(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Like>>(json);
        }

        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }
        
        
            /*
        private const string likesStorageFolderPath = "Resources/SystemLang/MiscCommands/DoYouLikeCommand";
        private const string likesStorageFilePath = "Likes.json";

        private static Dictionary<string, int> pairs = new Dictionary<string, int>();

        public static void AddLikesToStorage(string key, int value)
        {
            pairs.Add(key, value);
            SaveData();
        }

        internal static int GetLikes(string key)
        {
            if (pairs.TryGetValue(key, out int value))
            {
                return value;
            }
            else
            {
                return 0;
            }
        }

        static LikesStorage()
        {
            if (!Directory.Exists(likesStorageFolderPath)) Directory.CreateDirectory(likesStorageFolderPath);
            if (!File.Exists(likesStorageFolderPath + "/" + likesStorageFilePath))
            {
                AddLikesToStorage("Kagerou Project", 10);
            }
            else
            {
                string json = File.ReadAllText(likesStorageFolderPath + "/" + likesStorageFilePath);
                pairs = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
            }
        }

        public static void SaveData()
        {
            string json = JsonConvert.SerializeObject(pairs, Formatting.Indented);
            File.WriteAllText(likesStorageFolderPath + "/" + likesStorageFilePath, json);
        }
        */
    }
}
