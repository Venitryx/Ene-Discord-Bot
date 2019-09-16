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
    [Group("should")]
    public class Should : ModuleBase<SocketCommandContext>
    {
        private string specialMessage = "end";
        private int b = 0;

        [Command("I")]
        public async Task AskShouldUser([Remainder]string msg)
        {
            string[] options = msg.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var command = Commands.GetCommandInfo(Context.User.Id, Context.Message.Content);

            if (!msg.Contains(specialMessage) || !StringManipulation.isBotOwner(Context.User))
            {
                var embed = new EmbedBuilder();
                switch (command.TimesRun)
                {
                    case 0:
                        embed.WithDescription(GetMessage(msg, options, command));
                        command.TimesRun++;
                        Commands.SaveCommandInfo();
                        await Context.Channel.TriggerTypingAsync();
                        await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(command.Reply));
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                        break;
                    case 1:
                        embed.WithDescription(String.Format("As I said before: \"{0}\"", command.Reply));
                        command.TimesRun++;
                        Commands.SaveCommandInfo();
                        await Context.Channel.TriggerTypingAsync();
                        await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(command.Reply));
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                        break;
                    case 2:
                        embed.WithDescription(String.Format("I said: \"{0}\"", command.Reply));
                        command.TimesRun++;
                        Commands.SaveCommandInfo();
                        await Context.Channel.TriggerTypingAsync();
                        await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(command.Reply));
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                        break;
                    case 3:
                        embed.WithDescription(String.Format("For the fourth time! I said: \"{0}\"", command.Reply));
                        command.TimesRun++;
                        Commands.SaveCommandInfo();
                        await Context.Channel.TriggerTypingAsync();
                        await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(command.Reply));
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                        break;
                    case 4:
                        embed.WithDescription("Alright, stop bothering me already!");
                        command.TimesRun++;
                        Commands.SaveCommandInfo();
                        await Context.Channel.TriggerTypingAsync();
                        await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(command.Reply));
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                        break;
                    default:
                        await SpamShouldICommand(command);
                        break;
                }
            }
            else if (StringManipulation.isBotOwner(Context.User))
            {
                var embed = new EmbedBuilder();
                switch (command.TimesRun)
                {
                    case 0:
                        command.Reply = "You should end the video here.";
                        embed.WithDescription(command.Reply);
                        command.TimesRun++;
                        Commands.SaveCommandInfo();
                        await Context.Channel.TriggerTypingAsync();
                        await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(command.Reply));
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                        break;
                    case 1:
                        embed.WithDescription(String.Format("As I said before: \"{0}\"", command.Reply));
                        command.TimesRun++;
                        Commands.SaveCommandInfo();
                        await Context.Channel.TriggerTypingAsync();
                        await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(command.Reply));
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                        break;
                    case 2:
                        embed.WithDescription(String.Format("You heard me: \"{0}\"", command.Reply));
                        command.TimesRun++;
                        Commands.SaveCommandInfo();
                        await Context.Channel.TriggerTypingAsync();
                        await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(command.Reply));
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                        break;
                    case 3:
                        embed.WithDescription(String.Format("Are you messing around or actually an idiot? I said: \"{0}\"", command.Reply));
                        command.TimesRun++;
                        Commands.SaveCommandInfo();
                        await Context.Channel.TriggerTypingAsync();
                        await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(command.Reply));
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                        break;
                    case 4:
                        embed.WithDescription("Urgh, go die already!");
                        command.TimesRun++;
                        Commands.SaveCommandInfo();
                        await Context.Channel.TriggerTypingAsync();
                        await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(command.Reply));
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                        break;
                    default:
                        await SpamShouldICommand(command);
                        break;
                }
            }
        }

        public string GetMessage(string msg, string[] options, ShouldI command)
        {
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

                string reply = StringManipulation.AddMasterSuffix(answer);
                command.Reply = reply;

                Commands.SaveCommandInfo();

                return reply;
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

                string reply = StringManipulation.AddMasterSuffix("You should " + selection);

                command.Reply = reply;
                Commands.SaveCommandInfo();

                return reply;
            }
        }

        public async Task SpamShouldICommand(ShouldI command)
        {
            var embed = new EmbedBuilder();
            embed.WithDescription("...");
            command.TimesRun++;
            Commands.SaveCommandInfo();
            //Commands.DeleteCommandInfo(command);
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(command.Reply));
            embed.WithColor(Global.mainColor);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
