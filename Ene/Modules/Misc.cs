using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Ene.Core.UserAccounts;
using NReco.ImageGenerator;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Ene.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        Color mainColor = new Color(103, 163, 227);
        String punctuation = "?,;";

        /*[Command("what are my stats?")]
        public async Task MyStats()
        {
            var account = UserAccounts.GetAccount(Context.User);
            await Context.Channel.SendMessageAsync($"You have { account.XP} XP and {account.Points} points.");
        }
        */
        [Command("who am I?")]
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
            embed.WithTitle("You are");
            embed.WithDescription(firstName + " " + lastName);
            embed.WithColor(mainColor);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        [Command("hello")]
        public async Task Hello()
        {
            string html = String.Format("<html>\n < div class=\"container\">\n<img src=\"https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/7c6ac08c-8216-4359-99c9-5f4908ab3d37/d7ivgaj-d4fdc395-68e1-4f54-b9c3-c3e6bad06f7b.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcLzdjNmFjMDhjLTgyMTYtNDM1OS05OWM5LTVmNDkwOGFiM2QzN1wvZDdpdmdhai1kNGZkYzM5NS02OGUxLTRmNTQtYjljMy1jM2U2YmFkMDZmN2IucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.9aqFbPX_v_vot7iHKDjM019Qx9atdg8GvWcwEXAb2Hs\">\n<div class=\"centered\">Hello {0}</div>\n</div>\n</html>", Context.User.Username);
            string css = "<style>\n @font-face { font - family: 'cargo'; src: url('C:/Users/codev/Downloads/Website/8thCargo.ttf') format('truetype'); }\n\n.container {\n position: relative;\n text-align: center;\n color: white;\n    }\n.centered {\n position: absolute;\n top: 50 %;\n left: 50 %;\n font-family: 'cargo', 'Comic Sans MS';\n color: black;\n font-size: 50px;\n transform: translate(-50 %, -40 %);\n    }\n\n body {\n background: rgb(44, 110, 159);\n background-blend - mode: darken;\n    }\n </ style > ";
            var converter = new HtmlToImageConverter
            {
                Width = 500,
                Height = 150
            };
            var pngBytes = converter.GenerateImage(css + html, NReco.ImageGenerator.ImageFormat.Png);
            await Context.Channel.SendFileAsync(new MemoryStream(pngBytes), "Ene.png");
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
            await Context.Channel.SendMessageAsync($"You gained {xp} XP.");
        }

        [Command("say")]
        public async Task Say([Remainder]string msg)
        {
            var embed = new EmbedBuilder();
            embed.WithDescription(msg);
            embed.WithColor(mainColor);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("should I")]
        public async Task Pick([Remainder]string msg)
        {
            string[] options = msg.Split(new String[] { "or", ", " }, StringSplitOptions.RemoveEmptyEntries);
            if (options.Length is 0)
            {
                var embed = new EmbedBuilder();

                embed.WithDescription("Should you what?");
                embed.WithColor(mainColor);
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }

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
                    if (options[i].StartsWith("or"))
                    {
                        options[i] = options[i].Substring(2, options[i].Length);
                    }
                    string s = options[i].Substring(options[i].Length - 1);
                    Console.WriteLine(s);
                    if (punctuation.Contains(s))
                    {
                        options[i] = options[i].Replace('?', '.');
                        options[i] = options[i].Replace(';', '.');

                    }
                    else options[i] = options[i] + ".";
                }

                Random r = new Random();
                string selection = options[r.Next(0, options.Length)];

                var embed = new EmbedBuilder();
                embed.WithDescription("You should " + selection);
                embed.WithColor(mainColor);
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }

        [Command("get data count.")]
        public async Task GetData()
        {
            await Context.Channel.SendMessageAsync("Data has " + DataStorage.GetPairsCount() + " pairs.");
            DataStorage.AddPairToStorage("Count" + DataStorage.GetPairsCount() + " " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(), "TheCount" + DataStorage.GetPairsCount());
        }
    }
}
