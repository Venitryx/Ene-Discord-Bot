using System;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using AIMLbot;

using Ene.Core;


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
            _client.Ready += RepeatingTimer.StartSongActivityTimer;
            _client.Ready += RepeatingTimer.StartAfkTimer;
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            Global.Client = _client;

            if (RepeatingTimer.isSongCountEqual())
            {
                Random r = new Random();
                RepeatingTimer.songIndex = r.Next(0, RepeatingTimer.songCount);
            }

            Game game = new Game(RepeatingTimer.getNameOfSong(RepeatingTimer.songIndex, false), ActivityType.Listening);
            await Global.Client.SetActivityAsync(game);
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);;
            await Task.Delay(-1); 


        }

        public async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            
        }
    }
}
