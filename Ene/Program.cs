using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Ene
{
    public class Program
    {
        static void Main(string[] args)
        => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {        
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, "NTU2NTU0NDc1MTQ2NDQ0ODc5.D276vQ.VWifhboErHt0JR-HBrphTkLAt4w");
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        public async Task RegisterCommandsAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null || msg.Author.IsBot) return;
            int argPos = 0;

            if (msg.HasStringPrefix("Ene, ", ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, msg);
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }
    }
}
