using System;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;

using Ene.Core;
using Ene.Services;
using Ene.SystemLang.MiscCommands.AreYouCommand;
using Ene.SystemLang.MiscCommands.LikeCommands;

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
            EmotesStorage.SaveData();
            Likes.Initialize();

            if (Config.bot.token == "" || Config.bot.token == null)
            {
                Console.WriteLine("Invalid bot token!");
                return;
            }

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
            _client.ReactionAdded += OnReactionAdded;
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);

            await _client.StartAsync();
            _services = SetUpServices();
            Global.Client = _client;
            Game game = SongDisplay.pickRandomSongDisplay();
            await Global.Client.SetActivityAsync(game);
            await Global.Client.SetStatusAsync(UserStatus.Online);
            var cmdHandler = new CommandHandler(_client, _cmdService, _services);         
            await cmdHandler.InitializeAsync();

            await _services.GetRequiredService<MusicService>().InitializeAsync();
            await Task.Delay(-1); 
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if(reaction.MessageId == Global.MessageIdToTrack)
            {
                if(reaction.Emote.Name == "✅")
                {
                    await channel.SendMessageAsync(reaction.User.Value.Username + " reacted with :white_check_mark:.");
                }
                if (reaction.Emote.Name == "❎")
                {
                    await channel.SendMessageAsync(reaction.User.Value.Username + " reacted with :negative_squared_cross_mark:");
                }
            }
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
