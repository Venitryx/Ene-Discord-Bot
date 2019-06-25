using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Ene.Core;
using Ene.Modules;
using AIMLbot;

namespace Ene
{
    class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _cmdService;
        private readonly IServiceProvider _services;

        Bot AI = new Bot();
        private User myUser;

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
            int argPos = 0;
            if(msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos) 
                || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _cmdService.ExecuteAsync(context, argPos, _services);
                if(!result.IsSuccess)
                {
                    switch (result.Error)
                    {
                        case CommandError.BadArgCount:
                            Console.WriteLine(context.Message.Content);
                            if (context.Message.Content.Equals("Ene, should I"))
                            {
                                embed.WithDescription("Should you what?");
                                embed.WithColor(103, 163, 227);
                                await context.Channel.SendMessageAsync("", false, embed.Build());
                            }
                            break;
                        case CommandError.UnknownCommand:
                            break;
                        case CommandError.Exception:
                            break;
                        default:
                            await context.Channel.SendMessageAsync($"Something went wrong: ({result.ErrorReason})");
                            break;

                    }
                }
                await _client.SetStatusAsync(UserStatus.Online);
                var afkTimer = RepeatingTimer.StartAfkTimer();
            }

        }

        private Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }
    }
}
