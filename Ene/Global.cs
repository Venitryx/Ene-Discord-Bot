using System;
using System.Collections.Generic;
using System.Text;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace Ene
{
    internal static class Global
    {
        internal static DiscordSocketClient Client { get; set; }
        internal static SocketUser User { get; set; }
        internal static SocketCommandContext context { get; set; }
        internal static ulong MessageIdToTrack { get; set; }
        internal static Color mainColor = new Color(103, 163, 227);

        internal static string GetFolderPath(string filePath)
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
    }
}
