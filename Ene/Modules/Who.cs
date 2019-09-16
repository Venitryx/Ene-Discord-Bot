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
    [Group("who")]
    public class Who : ModuleBase<SocketCommandContext>
    {
        [Alias("are you?")]
        [Command("are you")]
        public async Task WhoAreYou()
        {
            var author = new EmbedAuthorBuilder()
            .WithName("Hi, my name's Ene! I'm a super pretty cyber girl!")
            .WithIconUrl(Context.Client.CurrentUser.GetAvatarUrl());
            var fieldAge = new EmbedFieldBuilder()
                    .WithName("Age:")
                    .WithValue("19")
                    .WithIsInline(true);
            var fieldBirthday = new EmbedFieldBuilder()
                    .WithName("Birthday:")
                    .WithValue("Unknown")
                    .WithIsInline(true);
            var fieldHeight = new EmbedFieldBuilder()
                    .WithName("Height:")
                    .WithValue("157cm")
                    .WithIsInline(true);
            var fieldGender = new EmbedFieldBuilder()
                    .WithName("Gender:")
                    .WithValue("F")
                    .WithIsInline(true);
            var fieldBloodtype = new EmbedFieldBuilder()
                    .WithName("Bloodtype:")
                    .WithValue("AB")
                    .WithIsInline(true);
            var fieldFavoriteColor = new EmbedFieldBuilder()
                    .WithName("Favorite Color:")
                    .WithValue("Blue")
                    .WithIsInline(true);
            var fieldSpecialPowers = new EmbedFieldBuilder()
                    .WithName("Abilities:")
                    .WithValue("Opening Eyes")
                    .WithIsInline(true);
            var fieldOccupations = new EmbedFieldBuilder()
                    .WithName("Occupations:")
                    .WithValue("Cybergirl\n6th member of the Mekakushi-dan")
                    .WithIsInline(true);
            var fieldOrigin = new EmbedFieldBuilder()
                    .WithName("Origin:")
                    .WithValue("[Kagerou Project](https://kagerouproject.fandom.com/wiki/Kagerou_Project)")
                    .WithIsInline(true);
            var embed = new EmbedBuilder()
                    .AddField(fieldAge)
                    .AddField(fieldBirthday)
                    .AddField(fieldHeight)
                    .AddField(fieldGender)
                    .AddField(fieldBloodtype)
                    .AddField(fieldFavoriteColor)
                    .AddField(fieldSpecialPowers)
                    .AddField(fieldOccupations)
                    .AddField(fieldOrigin)
                    .WithAuthor(author)
                    .WithColor(Global.mainColor)
                    .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
