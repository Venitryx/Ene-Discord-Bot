﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Ene.Core.Verification
{
    public static class ChannelDataStorage
    {
        public static void SaveVerificationInfo(IEnumerable<VerifiedChannel> verifiedChannel, string filePath)
        {
            string json = JsonConvert.SerializeObject(verifiedChannel, Formatting.Indented);
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
        public static IEnumerable<VerifiedChannel> LoadVerificationInfo(string filePath)
        {
            string[] dirAndFilePath = GetDirAndFilePath(filePath);
            if (!Directory.Exists(dirAndFilePath[0])) Directory.CreateDirectory(dirAndFilePath[0]);
            if (!File.Exists(dirAndFilePath[1])) VerifiedChannels.Initialize();
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<VerifiedChannel>>(json);
        }
    }
}