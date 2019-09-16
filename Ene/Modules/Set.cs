﻿using System;
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
using Ene.Core.Verification;
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
        [Alias("verification channel")]
        [Command("verification channel:")]
        public async Task SetVerificationChannel(ulong channelID, ulong roleID)
        {
            VerifiedChannels.GetChannelInfo(Context.Guild.Id, channelID, roleID);
            await Context.Channel.SendMessageAsync("Setting verification channel to " + channelID);
        }
    }
}