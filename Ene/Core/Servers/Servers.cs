using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord.WebSocket;
using Discord;

namespace Ene.Core.Servers
{
    public static class Servers
    {
        internal static List<Server> servers;
        private static string commandLogFilePath = "Resources/servers.json";

        static Servers()
        {
            LoadVerificationInfo();
        }
        //saves file
        public static void SaveVerificationInfo()
        {
            ServerDataStorage.SaveVerificationInfo(servers, commandLogFilePath);
        }

        //loads file
        public static void LoadVerificationInfo()
        {
            servers = ServerDataStorage.LoadVerificationInfo(commandLogFilePath).ToList();
        }

        public static void Initialize()
        {
            servers = new List<Server>() { };
            SaveVerificationInfo();
        }

        public static Server GetServerInfo(ulong guildID)
        {
            return GetOrCreateServerInfo(guildID);

        }

        //needs verification channel before verification command
        //method for adding users to verified group

        private static Server GetOrCreateServerInfo(ulong guildID)
        {
            var result = from s in servers
                         where
                         s.GuildID == guildID 
                         select s;
            var serverInfo = result.FirstOrDefault();
            if (serverInfo == null) serverInfo = CreateServerInfo(guildID);
            return serverInfo;
        }
        private static Server CreateServerInfo(ulong guildID)
        {
            var newServerInfo = new Server()
            {
                GuildID = guildID,
                VerifiedUsers = new List<VerifiedUser>() { }
            };

            servers.Add(newServerInfo);
            SaveVerificationInfo();
            return newServerInfo;
        }

        public static void SetVerificationChannel(ulong guildID, ulong channelID)
        {
            var result = from s in servers
                         where
                         s.GuildID == guildID
                         select s;
            var serverInfo = result.FirstOrDefault();
            if (serverInfo == null) serverInfo = CreateServerInfo(guildID);

            serverInfo.VerificationChannelID = channelID;
            SaveVerificationInfo();
        }

        public static void SetVerifiedRole(ulong guildID, ulong roleID)
        {
            var result = from s in servers
                         where
                         s.GuildID == guildID
                         select s;
            var serverInfo = result.FirstOrDefault();
            if (serverInfo == null) serverInfo = CreateServerInfo(guildID);

            serverInfo.VerifiedRoleID = roleID;
            SaveVerificationInfo();
        }

        public static void SetBotChannel(ulong guildID, ulong channelID)
        {
            var result = from s in servers
                         where
                         s.GuildID == guildID
                         select s;
            var serverInfo = result.FirstOrDefault();
            if (serverInfo == null) serverInfo = CreateServerInfo(guildID);

            serverInfo.BotChannelID = channelID;
            SaveVerificationInfo();
        }

        public static void SetMusicTextChannel(ulong guildID, ulong channelID)
        {
            var result = from s in servers
                         where
                         s.GuildID == guildID
                         select s;
            var serverInfo = result.FirstOrDefault();
            if (serverInfo == null) serverInfo = CreateServerInfo(guildID);

            serverInfo.MusicTextChannelID = channelID;
            SaveVerificationInfo();
        }

        public static void SetMusicVoiceChannel(ulong guildID, ulong channelID)
        {
            var result = from s in servers
                         where
                         s.GuildID == guildID
                         select s;
            var serverInfo = result.FirstOrDefault();
            if (serverInfo == null) serverInfo = CreateServerInfo(guildID);

            serverInfo.MusicVoiceChannelID = channelID;
            SaveVerificationInfo();
        }

        public static void AddUserVerified(SocketGuildUser guildUser, string firstName, string lastName, int advNumber, int studentID, ulong guildID)
        {
            var serverResult = from s in servers
                         where
                         s.GuildID == guildID
                         select s;
            var serverInfo = serverResult.FirstOrDefault();
            if (serverInfo == null) serverInfo = CreateServerInfo(guildID);

            ulong targetRoleID = serverInfo.VerifiedRoleID;
            var roleResult = from r in guildUser.Guild.Roles
                         where r.Id == targetRoleID
                         select r;
            IRole role = roleResult.FirstOrDefault();

            var verifiedUser = new VerifiedUser()
            {
                ID = guildUser.Id,
                StudentID = studentID,
                FirstName = firstName,
                LastName = lastName,
                AdvisoryNumber = advNumber               
            };
            var verifiedUsers = serverInfo.VerifiedUsers;
            var userResult = from u in verifiedUsers
                             where u.ID == verifiedUser.ID
                             select u;
            var user = userResult.FirstOrDefault();

            if(!verifiedUsers.Contains(user))
                verifiedUsers.Add(verifiedUser);
            guildUser.AddRoleAsync(role);
            SaveVerificationInfo();
        }
    }
}
