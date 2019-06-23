using System;
using System.Collections.Generic;
using System.Text;

using Discord;
using Discord.WebSocket;

namespace Ene
{
    internal static class Global
    {
        internal static DiscordSocketClient Client { get; set; }
        internal static Game Game { get; set; }
    }
}
