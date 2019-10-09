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
    [Group("what")]
    [RequireBotChannel()]
    public class What : ModuleBase<SocketCommandContext>
    {
        [Alias("can you do?")]
        [Command("can you do")]
        public async Task GetCommands()
        {
            var author = new EmbedAuthorBuilder()
            .WithName("Here's a list of things I can do.")
            .WithIconUrl(Context.Client.CurrentUser.GetAvatarUrl());
            var fieldMusic = new EmbedFieldBuilder()
                    .WithName("I can play music!")
                    .WithValue($"Commands:" +
                    $"\n{Config.bot.cmdPrefix}join the channel." +
                    $"\n{Config.bot.cmdPrefix}play <YouTube/SoundCloud> <link or search>" +
                    $"\n{Config.bot.cmdPrefix}stop playing." +
                    $"\n{Config.bot.cmdPrefix}skip the song." +
                    $"\n{Config.bot.cmdPrefix}repeat the song." +
                    $"\n{Config.bot.cmdPrefix}leave the channel.")
                    .WithIsInline(true);
            var fieldMisc = new EmbedFieldBuilder()
                    .WithName("I can perform other various fun tasks!")
                    .WithValue($"Commands:" +
                    $"\n{Config.bot.cmdPrefix}should I <insert_action(s)_here>?" +
                    $"\n{Config.bot.cmdPrefix}do you like <insert_something_here>?" +
                    $"\n{Config.bot.cmdPrefix}get anime: <name>" +
                    $"\n{Config.bot.cmdPrefix}get subreddit: <type> <subreddit_name>" +
                    $"\n{Config.bot.cmdPrefix}say: <message I should type to the channel>")
                    .WithIsInline(false);
            var embed = new EmbedBuilder()
                    .AddField(fieldMusic)
                    .AddField(fieldMisc)
                    .WithAuthor(author)
                    .WithDescription("If you need help, just call me by saying \"Ene, what can you do?\"")
                    .WithColor(Global.mainColor)
                    .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Alias("are my stats?")]
        [Command("are my stats")]
        public async Task MyStats()
        {
            var account = UserAccounts.GetAccount(Context.User);
            await Context.Channel.SendMessageAsync(StringManipulation.AddMasterSuffix($"You have {account.XP} XP and {account.Points} points."));
        }
    }
}
