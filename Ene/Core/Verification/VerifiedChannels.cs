using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord.WebSocket;

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

        private static VerifiedChannel GetOrCreateChannelInfo(ulong guildID, ulong channelID, ulong roleID)
        {
            var result = from v in verifiedChannels
                         where
                         v.ChannelID == channelID
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
                GuildID = guildID
            };

            verifiedChannels.Add(newChannelInfo);
            SaveVerificationInfo();
            return newChannelInfo;
        }

    }
}
