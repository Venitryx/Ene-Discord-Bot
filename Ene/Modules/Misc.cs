using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Diagnostics;

using IronOcr;

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
using Ene.Handlers;
using System.Net.Http;

namespace Ene.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [RequireOwner]
        [RequireBotChannel()]
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

        [RequireOwner]
        [RequireBotChannel()]
        [Alias("go to sleep")]
        [Command("go to sleep.")]
        public async Task Sleep()
        {
            var embed = await EmbedHandler.CreateBasicEmbedTitleOnly("Okay, oyasumi!", Global.mainColor, "https://i.pinimg.com/564x/f2/57/e8/f257e84288cead745bc3cc5b22e858d0.jpg");
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(embed.Title));
            await Context.Channel.SendMessageAsync("", false, embed);
            await Context.Client.SetStatusAsync(UserStatus.Offline);
        }

        [RequireVerificationChannel()]
        [Command("verify:")]
        public async Task Verify(string firstName, string lastName, int advNumber, int studentID, [Remainder]string nickname = null)
        {
            var embed = new EmbedBuilder();
            string message = "Yokoso, {0}.";
            embed.WithDescription(String.Format(message, firstName));
            embed.WithColor(Global.mainColor);
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(message));
            await Context.Channel.SendMessageAsync("", false, embed.Build());

            Servers.AddUserVerified((SocketGuildUser)Context.User, firstName, lastName, advNumber, studentID, Context.Guild.Id);
            if(nickname != null)
                await Context.Guild.GetUser(Context.User.Id).ModifyAsync(x => x.Nickname = nickname);
        }

        [Command("scan")]
        public async Task ScanFile()
        {
            AutoOcr OCR = new AutoOcr() { ReadBarCodes = false };

            var attachments = Context.Message.Attachments;

            WebClient myWebClient = new WebClient();

            string file = attachments.ElementAt(0).Filename;
            string url = attachments.ElementAt(0).Url;

            byte[] buffer = myWebClient.DownloadData(url);

            await Context.Channel.SendFileAsync(new MemoryStream(buffer), "Unknown.png");
            if (!File.Exists(String.Format("Pictures/Test-{0}.png", Context.User.Username)))
                File.WriteAllBytes(String.Format("Pictures/Test-{0}.png", Context.User.Username), buffer);

            await Task.Delay(5000);
            var results = OCR.Read(String.Format("Pictures/Test-{0}.png", Context.User.Username));
            await ReplyAsync(results.Text);
        }

        [Command("change nick")]
        public async Task Nick([Remainder]string nickname)
        {
            await Context.Guild.GetUser(Context.User.Id).ModifyAsync(x => x.Nickname = nickname);
        }


        [RequireUserPermission(GuildPermission.SendMessages)]
        [Alias("say")]
        [Command("say:")]
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

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("secretly say:")]
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
        [RequireBotChannel()]
        public async Task AskAreYou(string emote)
        {
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(5000);
            await Context.Channel.SendMessageAsync("OwO.");
        }

        [RequireOwner]
        [Command("add like")]
        public async Task AddLike(string name, double likability = 5.0, bool useSpecialMessage = false, [Remainder]string specialMessage = null)
        {
            var likedObject = Likes.GetLikedObject(objectName: name, likability, useSpecialMessage, specialMessage);
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(5000);
            await Context.Channel.SendMessageAsync(string.Format("Added {0} with value of {1}", name, likability));
        }

        [RequireOwner]
        [Command("reload likes")]
        public async Task LoadLikes()
        {
            Likes.LoadObjects();
            await ReplyAsync("Reloaded!");
        }

        [RequireOwner]
        [Command("reset likes")]
        public async Task ResetLikes()
        {
            Likes.InitializeLikes();
            await ReplyAsync("Resetted!");
        }

        [RequireOwner]
        [Command("reload like messages")]
        public async Task LoadLikeMessages()
        {
            Likes.LoadDefaultLikeMessages();
            await ReplyAsync("Reloaded!");
        }

        [RequireOwner]
        [Command("reset like messages")]
        public async Task ResetLikeMessages()
        {
            Likes.InitializeDefaultLikeMessages();
            await ReplyAsync("Resetted!");
        }

        [RequireOwner]
        [Command("reload all")]
        public async Task ReloadAll()
        {
            Likes.Reload();
            await ReplyAsync("Reloaded!");
        }

        [RequireOwner]
        [Command("reset all")]
        public async Task ResetAll()
        {
            Likes.ResetOrInitialize();
            await ReplyAsync("Resetted!");
        }

        [Command("do you like")]
        public async Task AskDoYouLike([Remainder]string objectName)
        {
            await SendIndividualMessages(StringManipulation.SplitIntoIndividualMessages(StringManipulation.AddMasterSuffix(Likes.GetMessage(objectName))), true);
        }

        [Command("how much do you like")]
        public async Task AskHowMuchDoYouLike(string objectName)
        {
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(5000);
            await Context.Channel.SendMessageAsync(String.Format("On a scale of 1 to 10: I give it a {0}.", Likes.GetLikability(objectName)));
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

        [Command("delete messages:")]
        public async Task DeleteMessages(string ammount)
        {
            if (!int.TryParse(ammount, out var num))
            {
                var embed = new EmbedBuilder();
                embed.WithDescription("You have to enter a number.");
                await ReplyAsync("", false, embed.Build());
            }
            else
            {
                var pastMessages = Context.Channel.GetMessagesAsync(num).FlattenAsync().Result;
                foreach (var msg in pastMessages)
                {
                    await msg.DeleteAsync();
                }
                var embed = new EmbedBuilder();
                embed.WithDescription(StringManipulation.AddMasterSuffix(String.Format("I've deleted {0} messages!", num)));
                embed.WithColor(Global.mainColor);
                var reply = await ReplyAsync("", false, embed.Build());
                await Task.Delay(4000);
                await reply.DeleteAsync();
            }
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

        internal async Task SendIndividualMessages(string[] messages, bool useEmbed = false)
        {
            if (useEmbed)
            {
                var embed = new EmbedBuilder();
                embed.WithDescription("");
                embed.WithColor(Global.mainColor);
                await Context.Channel.TriggerTypingAsync();
                await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(""));
                var message = await ReplyAsync("", false, embed.Build());

                for (int i = 0; i < messages.Length; i++)
                {
                    await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(messages[i]));
                    await message.ModifyAsync(x => x.Embed = embed.WithDescription(messages[i]).Build());
                }
            }
            else
            {
                await Context.Channel.TriggerTypingAsync();
                await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(""));
                var message = await ReplyAsync("");

                for (int i = 0; i < messages.Length; i++)
                {
                    await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(messages[i]));
                    await message.ModifyAsync(x => x.Content = messages[i]);
                }
            }
        }





        
        [Command("hello")]
        public async Task Hello()
        {
            
            /*
            var converter = new HtmlConverter();
            string html = String.Format($"<html>\n < h1 > Welcome {Context.User.Username}!</ h1 >\n </ html > ");
            string css = "<style>\n @font-face { font - family: 'cargo'; src: url('C:/Users/codev/Downloads/Website/8thCargo.ttf') format('truetype'); }\n\n body {\n background: rgba(0, 0, 0, .5)\n url(\"https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/7c6ac08c-8216-4359-99c9-5f4908ab3d37/d7ivgaj-d4fdc395-68e1-4f54-b9c3-c3e6bad06f7b.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcLzdjNmFjMDhjLTgyMTYtNDM1OS05OWM5LTVmNDkwOGFiM2QzN1wvZDdpdmdhai1kNGZkYzM5NS02OGUxLTRmNTQtYjljMy1jM2U2YmFkMDZmN2IucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.9aqFbPX_v_vot7iHKDjM019Qx9atdg8GvWcwEXAb2Hs\") no-repeat scroll 0% 0%;\n background-blend - mode: darken;\n    }\n\n h1 {\n position: absolute;\n top: 5 %;\n left: 12.5 %;\n font-family: 'cargo', 'Comic Sans MS';\n color: rgb(103, 163, 227);\n font-size: 50px;\n transform: translate(-50 %, -50 %);\n    }\n\n </ style > ";
            var bytes = converter.FromHtmlString(css + html, format: CoreHtmlToImage.ImageFormat.Png, quality: 100);

            var html1 = $@"<div><strong>Hello {Context.User.Username}</strong> World!</div>";
            var htmlBytes = converter.FromHtmlString(html1);
            File.WriteAllBytes(String.Format($"Pictures/EneWelcome-{Context.User.Username}.png"), bytes);
            await Context.Channel.SendFileAsync(new MemoryStream(htmlBytes), "Ene.png");
            */
            
            //NRecoLT needs license
            var converter = new HtmlToImageConverter
            {
                Width = 500,
                Height = 150
            };
            string html = String.Format($"<html>\n < h1 > Welcome {Context.User.Username}!</ h1 >\n </ html > ");
            string css = "<style>\n @font-face { font - family: 'cargo'; src: url('C:/Users/codev/Downloads/Website/8thCargo.ttf') format('truetype'); }\n\n body {\n background: rgba(0, 0, 0, .5)\n url(\"https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/7c6ac08c-8216-4359-99c9-5f4908ab3d37/d7ivgaj-d4fdc395-68e1-4f54-b9c3-c3e6bad06f7b.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcLzdjNmFjMDhjLTgyMTYtNDM1OS05OWM5LTVmNDkwOGFiM2QzN1wvZDdpdmdhai1kNGZkYzM5NS02OGUxLTRmNTQtYjljMy1jM2U2YmFkMDZmN2IucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.9aqFbPX_v_vot7iHKDjM019Qx9atdg8GvWcwEXAb2Hs\") no-repeat scroll 0% 0%;\n background-blend - mode: darken;\n    }\n\n h1 {\n position: absolute;\n top: 5 %;\n left: 12.5 %;\n font-family: 'cargo', 'Comic Sans MS';\n color: rgb(103, 163, 227);\n font-size: 50px;\n transform: translate(-50 %, -50 %);\n    }\n\n </ style > ";
            var pngBytes = converter.GenerateImage(css + html, NReco.ImageGenerator.ImageFormat.Png);
            await Context.Channel.SendFileAsync(new MemoryStream(pngBytes), "Ene.png");
            
        }
        
    }
}
