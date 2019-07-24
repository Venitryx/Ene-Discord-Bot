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
using Ene.SystemLang;
using Ene.SystemLang.MiscCommands.AreYouCommand;
using Ene.SystemLang.MiscCommands.DoYouLikeCommand;

using NReco.ImageGenerator;
using Newtonsoft.Json;
using CoreHtmlToImage;
using AIMLbot;

namespace Ene.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Alias("who are you?", "what are you", "what are you?")]
        [Command("who are you")]
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

        [Alias("what can you do?")]
        [Command("what can you do")]
        public async Task GetCommands()
        {
            var author = new EmbedAuthorBuilder()
            .WithName("Here's a list of things I can do.")
            .WithIconUrl(Context.Client.CurrentUser.GetAvatarUrl());
            var fieldMusic = new EmbedFieldBuilder()
                    .WithName("I can play music!")
                    .WithValue($"Commands:" +
                    $"\n{Config.bot.cmdPrefix}join the channel." +
                    $"\n{Config.bot.cmdPrefix}play <youtube link or search>" +
                    $"\n{Config.bot.cmdPrefix}stop playing." +
                    $"\n{Config.bot.cmdPrefix}skip the song." +
                    $"\n{Config.bot.cmdPrefix}stop playing." +
                    $"\n{Config.bot.cmdPrefix}leave the channel.")
                    .WithIsInline(true);
            var fieldMisc = new EmbedFieldBuilder()
                    .WithName("I can perform other various fun tasks!")
                    .WithValue($"Commands:" +
                    $"\n{Config.bot.cmdPrefix}should I <insert action(s) here>?" +
                    $"\n{Config.bot.cmdPrefix}say <message I should type to the channel>")
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

        [RequireOwner]
        [Command("react")]
        public async Task HandleReaction()
        {
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(5000);
            RestUserMessage msg = await Context.Channel.SendMessageAsync("React with ✅ or ❎!");
            Global.MessageIdToTrack = msg.Id;
        }

        [RequireOwner]
        [Alias("leave all servers.")]
        [Command("leave all servers")]
        public async Task LeaveAllServers()
        {
            foreach (var guild in Context.Client.Guilds)
            {
                if(guild.Id != 446409245571678208 || guild.Id != 555496686601109534)
                {
                    await Context.Channel.SendMessageAsync("Leaving: " + guild.Name);
                    await guild.LeaveAsync();

                }
            }
            await Context.Channel.SendMessageAsync(StringManipulation.AddMasterSuffix("All done!"));
        }

        [Alias("get random person.", "get a random person", "get a random person.")]
        [Command("get random person")]
        public async Task GetRandomPerson()
        {
            string json;
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://randomuser.me/api/?gender=male&nat=JP");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string firstName = dataObject.results[0].name.first.ToString();
            firstName = firstName.First().ToString().ToUpper() + firstName.Substring(1);
            string lastName = dataObject.results[0].name.last.ToString();
            lastName = lastName.First().ToString().ToUpper() + lastName.Substring(1);
            string picture = dataObject.results[0].picture.large.ToString();

            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl(picture);
            embed.WithTitle("Found a random person:");
            embed.WithDescription(firstName + " " + lastName);
            embed.WithColor(Global.mainColor);
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(embed.Description));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("say")]
        public async Task Say([Remainder]string msg)
        {
            var embed = new EmbedBuilder();
            var authorEmbed = new EmbedAuthorBuilder()
                .WithName(String.Format("{0}#{1} said:", Context.User.Username, Context.User.Discriminator))
                .WithIconUrl(Context.User.GetAvatarUrl());

            embed.WithDescription(msg);
            embed.WithAuthor(authorEmbed);
            embed.WithColor(Global.mainColor);
            await Context.Message.DeleteAsync();
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(msg));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [RequireOwner]
        [Command("secretly say")]
        public async Task SecretSay([Remainder]string msg)
        {
            var embed = new EmbedBuilder();
          
            embed.WithDescription(msg);
            embed.WithColor(Global.mainColor);
            await Context.Message.DeleteAsync();
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(msg));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [RequireOwner]
        [Command("whisper")]
        public async Task Whisper(ulong id, [Remainder]string msg)
        {
            var embed = new EmbedBuilder();
            embed.WithDescription(msg);
            embed.WithColor(Global.mainColor);
            await Context.Client.GetUser(id).SendMessageAsync("", false, embed.Build());
        }

        [RequireOwner]
        [Command("broadcast")]
        public async Task Broadcast(ulong guildID, ulong channelID, [Remainder]string msg)
        {
            var embed = new EmbedBuilder();
            embed.WithDescription(msg);
            embed.WithColor(Global.mainColor);
            await Context.Client.GetGuild(guildID).GetTextChannel(channelID).TriggerTypingAsync();
            await Task.Delay(5000);
            await Context.Client.GetGuild(guildID).GetTextChannel(channelID).SendMessageAsync("", false, embed.Build());
        }

        [Command("are you")]
        public async Task AskAreYou(string emote)
        {
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(5000);
            await Context.Channel.SendMessageAsync("OwO.");
        }

        [Command("add like")]
        public async Task AddLike(string name, double value, bool useSpecialMessage = false, [Remainder]string specialMessage = null)
        {
            var likedObject = Likes.GetLikedObject(objectName: name, value, useSpecialMessage, specialMessage);
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(5000);
            await Context.Channel.SendMessageAsync(string.Format("Added {0} with value of {1}", name, value));
        }
        
        [Alias("you like")]
        [Command("do you like")]
        public async Task AskDoYouLike(string objectName)
        {
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(Likes.GetMessage(objectName)));
            await Context.Channel.SendMessageAsync(StringManipulation.AddMasterSuffix(Likes.GetMessage(objectName)));
        }

        [Command("how much do you like")]
        public async Task AskHowMuchDoYouLike(string objectName)
        {
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(5000);
            await Context.Channel.SendMessageAsync(Likes.GetLikability(objectName));
        }

        [Command("get stats")]
        public async Task GetStats([Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            var account = UserAccounts.GetAccount(target);
            await Context.Channel.SendMessageAsync($"{target.Username} has { account.XP} XP and {account.Points} points.");
        }

        [Command("addXP")]
        public async Task AddXP(uint xp, [Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            var account = UserAccounts.GetAccount(target);
            account.XP += xp;
            UserAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync($"{target.Username} gained {xp} XP.");
        }

        [Command("should I")]
        public async Task AskShouldUser([Remainder]string msg)
        {
            string[] options = msg.Split(new String[] {","}, StringSplitOptions.RemoveEmptyEntries);

            if (options.Length is 1)
            {
                var replies = new List<string>();

                replies.Add("Yes, you should.");
                replies.Add("No, you shouldn't.");
                replies.Add("You totally should.");
                replies.Add("You definitely shouldn't.");
                replies.Add("Maybe.");
                replies.Add("I'm not sure.");
                replies.Add("I don't know.");
                replies.Add("How should I know?");

                Random r = new Random();
                var answer = replies[r.Next(replies.Count)];

                var embed = new EmbedBuilder();
                embed.WithDescription(StringManipulation.AddMasterSuffix(answer));
                embed.WithColor(Global.mainColor);
                await Context.Channel.TriggerTypingAsync();
                await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(answer));
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            else
            {
                for (int i = 0; i < options.Length; i++)
                {
                    options[i] = StringManipulation.RemoveFanboys(options[i]);
                    options[i] = StringManipulation.StripPunctuation(options[i]);
                    options[i] = StringManipulation.StripSymbols(options[i]);
                    options[i] += '.';
                }

                Random r = new Random();
                string selection = options[r.Next(options.Length)];

                var embed = new EmbedBuilder();
                embed.WithDescription(StringManipulation.AddMasterSuffix("You should " + selection));
                embed.WithColor(Global.mainColor);
                await Context.Channel.TriggerTypingAsync();
                await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(selection));
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }
        [Alias("what are my stats?")]
        [Command("what are my stats")]
        public async Task MyStats()
        {
            var account = UserAccounts.GetAccount(Context.User);
            await Context.Channel.SendMessageAsync(StringManipulation.AddMasterSuffix($"You have {account.XP} XP and {account.Points} points."));
        }
      
        [Command("help please.")]
        public async Task ShowCommands([Remainder]string arg = "")
        {
            if (!UserIsDeveloper((SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync(":x: You need the Developer role to do that. " + Context.User.Mention);
                return;
            }
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetMessage("help"));
        }

        private bool UserIsDeveloper(SocketGuildUser user)
        {
            ulong targetRoleID = 588857834155016193;
            var result = from r in user.Guild.Roles
                         where r.Id == targetRoleID
                         select r.Id;
            ulong roleID = result.FirstOrDefault();
            if (roleID == 0) return false;
            var targetRole = user.Guild.GetRole(roleID);
            return user.Roles.Contains(targetRole);
        }

        [Command("get data count.")]
        public async Task GetData()
        {
            await Context.Channel.SendMessageAsync("Data has " + DataStorage.GetPairsCount() + " pairs.");
            DataStorage.AddPairToStorage("Count" + DataStorage.GetPairsCount() + " " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(), "TheCount" + DataStorage.GetPairsCount());
        }

        /*
        [Command("hello")]
        public async Task Hello()
        {
            
            var converter = new HtmlConverter();
            string html = String.Format($"<html>\n < h1 > Welcome {Context.User.Username}!</ h1 >\n </ html > ");
            string css = "<style>\n @font-face { font - family: 'cargo'; src: url('C:/Users/codev/Downloads/Website/8thCargo.ttf') format('truetype'); }\n\n body {\n background: rgba(0, 0, 0, .5)\n url(\"https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/7c6ac08c-8216-4359-99c9-5f4908ab3d37/d7ivgaj-d4fdc395-68e1-4f54-b9c3-c3e6bad06f7b.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcLzdjNmFjMDhjLTgyMTYtNDM1OS05OWM5LTVmNDkwOGFiM2QzN1wvZDdpdmdhai1kNGZkYzM5NS02OGUxLTRmNTQtYjljMy1jM2U2YmFkMDZmN2IucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.9aqFbPX_v_vot7iHKDjM019Qx9atdg8GvWcwEXAb2Hs\") no-repeat scroll 0% 0%;\n background-blend - mode: darken;\n    }\n\n h1 {\n position: absolute;\n top: 5 %;\n left: 12.5 %;\n font-family: 'cargo', 'Comic Sans MS';\n color: rgb(103, 163, 227);\n font-size: 50px;\n transform: translate(-50 %, -50 %);\n    }\n\n </ style > ";
            var bytes = converter.FromHtmlString(css + html, format: CoreHtmlToImage.ImageFormat.Png, quality: 100);

            var html1 = $@"<div><strong>Hello {Context.User.Username}</strong> World!</div>";
            var htmlBytes = converter.FromHtmlString(html1);
            //File.WriteAllBytes(String.Format($"Pictures/EneWelcome-{Context.User.Username}.png"), bytes);
            await Context.Channel.SendFileAsync(new MemoryStream(htmlBytes), "Ene.png");
            
            
            //NRecoLT needs license
            //var converter = new HtmlToImageConverter
            //{
            //    Width = 500,
            //    Height = 150
            //};
            //string html = String.Format($"<html>\n < h1 > Welcome {Context.User.Username}!</ h1 >\n </ html > ");
            //string css = "<style>\n @font-face { font - family: 'cargo'; src: url('C:/Users/codev/Downloads/Website/8thCargo.ttf') format('truetype'); }\n\n body {\n background: rgba(0, 0, 0, .5)\n url(\"https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/7c6ac08c-8216-4359-99c9-5f4908ab3d37/d7ivgaj-d4fdc395-68e1-4f54-b9c3-c3e6bad06f7b.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcLzdjNmFjMDhjLTgyMTYtNDM1OS05OWM5LTVmNDkwOGFiM2QzN1wvZDdpdmdhai1kNGZkYzM5NS02OGUxLTRmNTQtYjljMy1jM2U2YmFkMDZmN2IucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.9aqFbPX_v_vot7iHKDjM019Qx9atdg8GvWcwEXAb2Hs\") no-repeat scroll 0% 0%;\n background-blend - mode: darken;\n    }\n\n h1 {\n position: absolute;\n top: 5 %;\n left: 12.5 %;\n font-family: 'cargo', 'Comic Sans MS';\n color: rgb(103, 163, 227);\n font-size: 50px;\n transform: translate(-50 %, -50 %);\n    }\n\n </ style > ";
            //var pngBytes = converter.GenerateImage(css + html, NReco.ImageGenerator.ImageFormat.Png);
            //await Context.Channel.SendFileAsync(new MemoryStream(pngBytes), "Ene.png");
            
        }
        */
    }
}
