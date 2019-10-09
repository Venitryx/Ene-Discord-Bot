using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;

using Discord.Audio;

using Ene.Core.UserAccounts;
using Ene.Core.Servers;
using Ene.Preconditions;
using Ene.SystemLang;
using Ene.SystemLang.MiscCommands.AreYouCommand;
using Ene.SystemLang.MiscCommands.LikeCommands;
using Ene.SystemLang.MiscCommands.ShouldICommand;

using RedditSharp;
using NReco.ImageGenerator;
using Newtonsoft.Json;
using CoreHtmlToImage;
using static RedditSharp.Things.VotableThing;
using JikanDotNet;

namespace Ene.Modules
{
    [Group("set")]
    public class Set : ModuleBase<SocketCommandContext>
    {
        [Alias("verification channel", "verification channel to:", "verification channel to")]
        [Command("verification channel:")]
        public async Task SetVerificationChannel(SocketGuildChannel guildChannel)
        {
            Servers.SetVerificationChannel(Context.Guild.Id, guildChannel.Id);
            await Context.Channel.SendMessageAsync("All new members will now need to verify in #" + guildChannel + " before they can access channels!");
        }

        [Alias("verification role", "verification role to:", "verification role to")]
        [Command("verification role:")]
        public async Task SetVerificationRole(SocketRole guildRole)
        {
            Servers.SetVerifiedRole(Context.Guild.Id, guildRole.Id);
            await Context.Channel.SendMessageAsync("All new members will now get the role " + guildRole + " when they become verified!");
        }

        [Alias("bot channel", "bot channel to:", "bot channel to")]
        [Command("bot channel:")]
        public async Task SetBotChannel(SocketGuildChannel guildChannel)
        {
            Servers.SetBotChannel(Context.Guild.Id, guildChannel.Id);
            await Context.Channel.SendMessageAsync("Main bot commands can now only be used in #" + guildChannel + "!");
        }

        [Alias("music text channel", "music text channel to:", "music text channel to")]
        [Command("music text channel:")]
        public async Task SetTextChannel(SocketGuildChannel guildChannel)
        {
            Servers.SetMusicTextChannel(Context.Guild.Id, guildChannel.Id);
            await Context.Channel.SendMessageAsync("Music commands can now only be used in #" + guildChannel + "!");
        }

        [Alias("music voice channel", "music voice channel to:", "music voice channel to")]
        [Command("music voice channel:")]
        public async Task SetMusicVoiceChannel(ulong voiceChannelID)
        {
            Servers.SetMusicVoiceChannel(Context.Guild.Id, voiceChannelID);
            await Context.Channel.SendMessageAsync("Music commands can now only be used in #" + voiceChannelID + "!");
        }
    }
}
