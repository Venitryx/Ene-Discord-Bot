using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord.WebSocket;
using Discord;

namespace Ene.Core.Verification
{
    public static class VerifiedChannels
    {
        private static List<VerifiedChannel> verifiedChannels;
        private static string commandLogFilePath = "Resources/verification_channels.json";

        static VerifiedChannels()
        {
            LoadVerificationInfo();
        }
        //saves file
        public static void SaveVerificationInfo()
        {
            ChannelDataStorage.SaveVerificationInfo(verifiedChannels, commandLogFilePath);
        }

        //loads file
        public static void LoadVerificationInfo()
        {
            verifiedChannels = ChannelDataStorage.LoadVerificationInfo(commandLogFilePath).ToList();
        }

        public static void Initialize()
        {
            verifiedChannels = new List<VerifiedChannel>() { };
            SaveVerificationInfo();
        }

        public static VerifiedChannel GetChannelInfo(ulong guildID, ulong channelID, ulong roleID)
        {
            return GetOrCreateChannelInfo(guildID, channelID, roleID);

        }

        //needs verification channel before verification command
        //method for adding users to verified group

        private static VerifiedChannel GetOrCreateChannelInfo(ulong guildID, ulong channelID, ulong roleID)
        {
            var result = from v in verifiedChannels
                         where
                         v.GuildID == guildID 
                         && v.ChannelID == channelID
                         && v.RoleID == roleID
                         select v;

            var channelInfo = result.FirstOrDefault();
            if (channelInfo == null) channelInfo = CreateChannelInfo(guildID, channelID, roleID);
            return channelInfo;
        }
        private static VerifiedChannel CreateChannelInfo(ulong guildID, ulong channelID, ulong roleID)
        {
            var newChannelInfo = new VerifiedChannel()
            {
                ChannelID = channelID,
                RoleID = roleID,
                GuildID = guildID,
                VerifiedUserIDs = new List<ulong>() { }
            };

            verifiedChannels.Add(newChannelInfo);
            SaveVerificationInfo();
            return newChannelInfo;
        }

        public static void AddUserVerified(SocketGuildUser guildUser, ulong guildID)
        {
            var result = from v in verifiedChannels
                         where
                         v.GuildID == guildID
                         select v;
            var channelInfo = result.FirstOrDefault();

            ulong targetRoleID = channelInfo.RoleID;
            var roleResult = from r in guildUser.Guild.Roles
                         where r.Id == targetRoleID
                         select r;
            IRole role = roleResult.FirstOrDefault();

            var verifiedUserIDs = channelInfo.VerifiedUserIDs;
            if(!verifiedUserIDs.Contains(guildUser.Id))
                verifiedUserIDs.Add(guildUser.Id);
            guildUser.AddRoleAsync(role);
            SaveVerificationInfo();
        }
    }
}
