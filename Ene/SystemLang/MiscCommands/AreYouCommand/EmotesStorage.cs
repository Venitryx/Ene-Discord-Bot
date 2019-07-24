using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Ene.SystemLang.MiscCommands.AreYouCommand
{
    class EmotesStorage
    {
        private const string emotesStorageFolderPath = "Resources/SystemLang/MiscCommands/AreYouCommand";
        private const string emotesStorageFilePath = "Emotes.json";

        private static Emotes.EmotesData emotesData;
        static EmotesStorage()
        {
            if (!Directory.Exists(emotesStorageFolderPath)) Directory.CreateDirectory(emotesStorageFolderPath);
            if (!File.Exists(emotesStorageFolderPath + "/" + emotesStorageFilePath))
            {
                emotesData = new Emotes.EmotesData();
                emotesData.angerLevel = 0;
                emotesData.happinessLevel = 7;
                emotesData.sadnessLevel = 4;
                emotesData.fearLevel = 3;
                emotesData.loveLevel = 10;
                string json = JsonConvert.SerializeObject(emotesData, Formatting.Indented);
                File.WriteAllText(emotesStorageFolderPath + "/" + emotesStorageFilePath, json);
            }
            else
            {
                string json = File.ReadAllText(emotesStorageFolderPath + "/" + emotesStorageFilePath);
                emotesData = JsonConvert.DeserializeObject<Emotes.EmotesData>(json);
            }
        }

        public static void SaveData()
        {
            string json = JsonConvert.SerializeObject(emotesData, Formatting.Indented);
            File.WriteAllText(emotesStorageFolderPath + "/" + emotesStorageFilePath, json);
        }
    }
}
