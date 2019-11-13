using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Ene.Core;
using Ene.SystemLang;
using Ene.Modules;

namespace Ene
{
    class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _cmdService;
        private readonly IServiceProvider _services;

        public CommandHandler(DiscordSocketClient client, CommandService cmdService, IServiceProvider services)
        {
            _client = client;
            _cmdService = cmdService;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _cmdService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _cmdService.Log += LogAsync;
            _client.MessageReceived += HandleCommandAsync;
        } 
        

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var embed = new EmbedBuilder();
            var msg = s as SocketUserMessage;
            if (msg == null) return;

            var context = new SocketCommandContext(_client, msg);
            Global.context = context;
            int argPos = 0;
            if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos)
                || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _cmdService.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                {
                    switch (result.Error)
                    {
                        case CommandError.BadArgCount:
                            Console.WriteLine(context.Message.Content);
                            if (context.Message.Content.Equals("Ene, should I"))
                            {
                                embed.WithDescription("Should you what?");
                                embed.WithColor(Global.mainColor);
                                await context.Channel.SendMessageAsync("", false, embed.Build());
                            }
                            break;
                        case CommandError.UnknownCommand:
                            var replies = new List<string>();

                            replies.Add("Sorry, I don't know how to respond to that.");
                            replies.Add("I can't answer that quite yet.");
                            replies.Add("I haven't been programmed to respond to that just yet.");
                            replies.Add("Sorry, that's an invalid command. Maybe try something different for the time being.");
                            replies.Add("I can't understand. Maybe try something else?");

                            Random r = new Random();
                            var answer = replies[r.Next(replies.Count)];

                            embed.WithDescription(StringManipulation.AddMasterSuffix(answer));
                            embed.WithColor(Global.mainColor);
                            await context.Channel.TriggerTypingAsync();
                            await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(answer));
                            await context.Channel.SendMessageAsync("", false, embed.Build());
                            break;
                        case CommandError.Exception:
                            await context.Channel.SendMessageAsync(result.ErrorReason);
                            break;
                        default:
                            await context.Channel.SendMessageAsync(result.ErrorReason);
                            break;
                    }
                }
                await _client.SetStatusAsync(UserStatus.Online);
                await RepeatingTimer.StartAfkTimer();
            }
        }
        private Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }
    }
}
