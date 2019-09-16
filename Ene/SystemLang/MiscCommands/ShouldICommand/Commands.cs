using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord.WebSocket;

using Ene.Core;

namespace Ene.SystemLang.MiscCommands.ShouldICommand
{
    public static class Commands
    {
        private static List<ShouldI> commands;
        private static string commandLogFilePath = "Resources/SystemLang/MiscCommands/ShouldICommand/CommandLog.json";

        static Commands()
        {
            LoadCommandInfo();
        }
        //saves file
        public static void SaveCommandInfo()
        {
            Storage.SaveCommandInfo(commands, commandLogFilePath);
        }

        //loads file
        public static void LoadCommandInfo()
        {
            commands = Storage.LoadCommandInfo(commandLogFilePath).ToList();
        }

        public static void DeleteCommandInfo(ShouldI command)
        {
            var now = DateTime.UtcNow;
            if(command.TimeOfCommand.AddMilliseconds(RepeatingTimer.loopingPurgeTimer.Interval) <= now)
            {
                commands.Remove(command);
                SaveCommandInfo();
            }
        }

        public static void DeleteAllCommandInfo()
        {
            var now = DateTime.UtcNow;
            commands.RemoveAll(command => command.TimeOfCommand.AddMilliseconds(RepeatingTimer.loopingPurgeTimer.Interval) <= now);
            SaveCommandInfo();
        }

        public static void Initialize()
        {
            commands = new List<ShouldI>() { };
        }

        public static ShouldI GetCommandInfo(ulong id, string command, string reply = null)
        {
            return GetOrCreateCommandInfo(id, command, reply);

        }

        private static ShouldI GetOrCreateCommandInfo(ulong id, string command, string reply = null)
        {
            var result = from c in commands
                         where
                         c.ID == id
                         && c.Command == command
                         select c;

            var commandInfo = result.FirstOrDefault();
            if (commandInfo == null) commandInfo = CreateCommandInfo(id, command, reply);
            return commandInfo;
        }
        private static ShouldI CreateCommandInfo(ulong id, string command, string reply = null)
        {
            var newCommandInfo = new ShouldI()
            {
                ID = id,
                Command = command,
                Reply = reply,
                TimeOfCommand = DateTime.UtcNow,
                TimesRun = 0
            };

            commands.Add(newCommandInfo);
            SaveCommandInfo();
            return newCommandInfo;
        }

    }
}
