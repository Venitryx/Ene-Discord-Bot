using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

using Discord;
using Discord.WebSocket;

using Ene.SystemLang.MiscCommands.ShouldICommand;

namespace Ene.Core
{
    internal static class RepeatingTimer
    {
        internal static Timer loopingSongActivityTimer, loopingAfkTimer, loopingPurgeTimer;
        private static SocketTextChannel channel;
        private static Game game;
        private static bool isJapanese = true;

        internal static Task StartSongActivityTimer()
        {
            loopingSongActivityTimer = new Timer()
            {
                Interval = 3.5 * 60 * 1000,
                //Interval = 5 * 1000,
                AutoReset = true,
                Enabled = true
            };
            loopingSongActivityTimer.Elapsed += OnSongActivityTimerTicked;

            return Task.CompletedTask;
        }

        internal static Task StartAfkTimer()
        {
            loopingAfkTimer = new Timer()
            {
                Interval = 10 * 60 * 1000,
                //Interval = 5 * 1000,
                AutoReset = false,
                Enabled = true
            };
            loopingAfkTimer.Elapsed += OnAfkTimerTicked;

            return Task.CompletedTask;
        }

        internal static Task StartDeleteMessageTimer()
        {
            loopingPurgeTimer = new Timer()
            {
                Interval = 1 * 60 * 1000,
                AutoReset = true,
                Enabled = true
            };
            loopingPurgeTimer.Elapsed += OnPurgeTimerTicked;

            return Task.CompletedTask;
        }

        private static async void OnSongActivityTimerTicked(object sender, ElapsedEventArgs e)
        {
            string songName = SongDisplay.getNameOfSong(SongDisplay.songIndex, isJapanese);
            game = new Game(songName, ActivityType.Listening);
            SongDisplay.songIndex++;
            await Global.Client.SetActivityAsync(game);
        }

        private static async void OnAfkTimerTicked(object sender, ElapsedEventArgs e)
        {
            await Global.Client.SetStatusAsync(UserStatus.AFK);
        }

        private static async void OnPurgeTimerTicked(object sender, ElapsedEventArgs e)
        {
            Commands.DeleteAllCommandInfo();
            channel = Global.Client.GetGuild(446409245571678208).GetTextChannel(591059698128519168);
            await channel.SendMessageAsync("Purged!");
        }
    }
}
