using System;
using System.Collections.Generic;
using System.Text;

namespace Ene.Core.Servers
{
    public class Server
    {
        public ulong VerificationChannelID { get; set; }
        public ulong MusicTextChannelID { get; set; }
        public ulong MusicVoiceChannelID { get; set; }
        public ulong BotChannelID { get; set; }
        public ulong GuildID { get; set; }
        public ulong VerifiedRoleID { get; set; }
        public List<VerifiedUser> VerifiedUsers { get; set; }
    }
}
