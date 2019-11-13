using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Ene.Core.Songs
{
    public static class SongDataStorage
    {

        public struct SongConfig
        {
            public bool UseRomanizedNames;
            public List<Song> Songs;
        }

        public static void SaveSongConfig(SongConfig songConfig, string filePath)
        {
            string json = JsonConvert.SerializeObject(songConfig, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static string GetFolderPath(string filePath)
        {
            string[] splitFilePath = filePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string folderPath = "";
            foreach (string folder in splitFilePath)
            {
                if (!folder.Contains('.'))
                {
                    folderPath += (folder + '/');
                }
                else
                {
                    folderPath = folderPath.Substring(0, folderPath.Length - 1);
                    break;
                }
            }
            return folderPath;
        }
        public static SongConfig LoadSongConfig(string filePath)
        {
            /*
            string folderPath = GetFolderPath(filePath);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            if (!File.Exists(filePath)) Songs.Initialize();
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<SongConfig>(json);
            */

            string folderPath = GetFolderPath(filePath);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            if (!File.Exists(filePath)) Songs.Initialize();
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<SongConfig>(json);
        }
    }
}
