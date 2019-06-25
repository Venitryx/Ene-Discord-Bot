using System;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using AIMLbot;

using Ene.Core;
using Ene.Services;

using Victoria;

namespace Ene
{
    public class Program
    {
        private DiscordSocketClient _client;
        private CommandService _cmdService;
        private IServiceProvider _services;

        static void Main(string[] args)
         => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            if (Config.bot.token == "" || Config.bot.token == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                AlwaysDownloadUsers = true,
                MessageCacheSize = 50
            });

            _cmdService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                CaseSensitiveCommands = false
            });

            _client.Log += Log;
            _client.Ready += RepeatingTimer.StartSongActivityTimer;
            _client.Ready += RepeatingTimer.StartAfkTimer;
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            _services = SetUpServices();
            Global.Client = _client;
            Game game = RepeatingTimer.pickRandomSongDisplay();
            await Global.Client.SetActivityAsync(game);
            var cmdHandler = new CommandHandler(_client, _cmdService, _services);
            await cmdHandler.InitializeAsync();

            await _services.GetRequiredService<MusicService>().InitializeAsync();
            await Task.Delay(-1); 


        }

        private IServiceProvider SetUpServices()
            => new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_cmdService)
            .AddSingleton<LavaRestClient>()
            .AddSingleton<LavaSocketClient>()
            .AddSingleton<MusicService>()
            .BuildServiceProvider();

        public async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}
