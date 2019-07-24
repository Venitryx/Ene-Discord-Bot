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
    }
}
