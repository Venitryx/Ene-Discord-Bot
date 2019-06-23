using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Ene.Modules;
using AIMLbot;

namespace Ene
{
    class CommandHandler
    {
        DiscordSocketClient _client;
        CommandService _service;

        Bot AI = new Bot();
        private User myUser;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
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
                var result = await _service.ExecuteAsync(context, argPos, null);
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
            }

        }
    }
}
