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

using Discord.Audio;

using Ene.Core.UserAccounts;
using NReco.ImageGenerator;
using Newtonsoft.Json;
using CoreHtmlToImage;
using AIMLbot;

namespace Ene.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        public Color mainColor = new Color(103, 163, 227);
        String punctuation = "?;";

        /*[Command("what are my stats?")]
        public async Task MyStats()
        {
            var account = UserAccounts.GetAccount(Context.User);
            await Context.Channel.SendMessageAsync($"You have { account.XP} XP and {account.Points} points.");
        }
        */

        [Command("join the channel.", RunMode = RunMode.Async)]
        public async Task JoinChannel(IVoiceChannel channel = null)
        {
            // Get the audio channel
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }
            var audioClient = await channel.ConnectAsync();
            await SendAsync(audioClient, "C:\\Users\\codev\\Documents\\GitHub\\Ene-Discord-Bot\\Ene\\bin\\Debug\\netcoreapp2.2");
            
        }

        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }

        private async Task SendAsync(IAudioClient client, string path)
        {
            // Create FFmpeg using the previous example
            using (var ffmpeg = CreateStream(path))
            using (var output = ffmpeg.StandardOutput.BaseStream)
            using (var discord = client.CreatePCMStream(AudioApplication.Mixed))
            {
                try { await output.CopyToAsync(discord); }
                finally { await discord.FlushAsync(); }
            }
        }

        [Command("get random person.")]
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
            embed.WithColor(mainColor);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
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

        /*
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
            await Context.Channel.SendMessageAsync($"You gained {xp} XP.");
        }
        */

        [Command("say")]
        public async Task Say([Remainder]string msg)
        {
            var embed = new EmbedBuilder();
            embed.WithDescription(msg);
            embed.WithColor(mainColor);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [RequireOwner]
        [Command("say and delete")]
        public async Task SecretSay([Remainder]string msg)
        {
            var embed = new EmbedBuilder();
            embed.WithDescription(msg);
            embed.WithColor(mainColor);
            await Context.Message.DeleteAsync();
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [RequireOwner]
        [Command("whisper")]
        public async Task whisper([Remainder]string msg)
        {
            var embed = new EmbedBuilder();
            msg = SubstitutePronouns(msg);
            msg = SubstituteVerbs(msg);
            embed.WithDescription(msg);
            embed.WithColor(mainColor);
            await Context.Client.GetUser(232345332162363394).SendMessageAsync("", false, embed.Build());
        }

        internal string SubstitutePronouns(string previousString)
        {
            string[] words = previousString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Equals("I"))
                    words[i] = words[i].Replace("I", "you");
                else if (words[i].Equals("me"))
                    words[i] = words[i].Replace("me", "you");
                else if (words[i].Equals("my"))
                    words[i] = words[i].Replace("my", "your");
                else if (words[i].Equals("our"))
                    words[i] = words[i].Replace("our", "your");
                else if (words[i].Equals("mine"))
                    words[i] = words[i].Replace("mine", "yours");
                else if (words[i].Equals("ours"))
                    words[i] = words[i].Replace("ours", "yours");
            }
            return string.Join(" ", words);
        }

        internal string SubstituteVerbs(string previousString)
        {
            string[] words = previousString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Equals("am"))
                    words[i] = words[i].Replace("am", "are");
                else if (words[i].Equals("are"))
                    words[i] = words[i].Replace("are", "am");
            }
            return string.Join(" ", words);
        }

        internal string RemoveFanboys(string previousString)
        {
            string[] words = previousString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if(words[0].Equals("for") || words[0].Equals("and") || words[0].Equals("nor") || words[0].Equals("but")
                || words[0].Equals("or") || words[0].Equals("yet") || words[0].Equals("so"))
            {
                words[0] = "";
            }
            return string.Join(" ", words);
        }

        [Command("should I")]
        public async Task Pick([Remainder]string msg)
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
                var answer = replies[r.Next(replies.Count - 1)];

                var embed = new EmbedBuilder();
                embed.WithDescription(answer);
                embed.WithColor(mainColor);
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            else
            {
                for (int i = 0; i < options.Length; i++)
                {
                    string newString;
                    options[i] = RemoveFanboys(options[i]);

                    string punctuationString = options[i].Substring(options[i].Length - 1);
                    if (punctuation.Contains(punctuationString))
                    {
                        newString = options[i].Substring(0, options[i].Length - 1);
                        newString = SubstitutePronouns(newString);
                        newString = SubstituteVerbs(newString);
                        punctuationString = punctuationString.Replace('?', '.');
                        punctuationString = punctuationString.Replace(';', '.');
                        newString += punctuationString;
                    }
                    else
                    {
                        newString = options[i];
                        newString = SubstitutePronouns(newString);
                        newString = SubstituteVerbs(newString);
                        newString += ".";
                    }
                    options[i] = newString;
                }

                Random r = new Random();
                string selection = options[r.Next(0, options.Length)];

                var embed = new EmbedBuilder();
                embed.WithDescription("You should " + selection);
                embed.WithColor(mainColor);
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }

        /*
        [Command("help please.")]
        public async Task ShowCommands([Remainder]string arg = "")
        {
            if (!UserIsDeveloper((SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync(":x: You need the Developer role to do that. " + Context.User.Mention);
                return;
            }
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetAlert("help"));
        }

        private bool UserIsDeveloper(SocketGuildUser user)
        {
            string targetRoleName = "Developer";
            var result = from r in user.Guild.Roles
                         where r.Name == targetRoleName
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
        */
    }
}
