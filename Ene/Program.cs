using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Discord;


namespace Ene
{
    public class Program
    {
        DiscordSocketClient _client;
        CommandHandler _handler;

        static void Main(string[] args)
         => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            if (Config.bot.token == "" || Config.bot.token == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });
            _client.Log += Log;
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();

            Game game = new Game("Kagerou Project", ActivityType.Listening);
            await _client.SetActivityAsync(game);
            await _client.SetStatusAsync(UserStatus.AFK);
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1); 
        }

        public async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            
        }
    }
}
