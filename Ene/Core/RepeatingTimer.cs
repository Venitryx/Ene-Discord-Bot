using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

using Discord;
using Discord.WebSocket;

using Ene.SystemLang.MiscCommands.ShouldICommand;
using Ene.Core.Songs;

namespace Ene.Core
{
    internal static class RepeatingTimer
    {
        internal static Timer loopingSongActivityTimer, loopingAfkTimer, loopingPurgeTimer;
        private static SocketTextChannel channel;
        private static Game game;

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
                Interval = 2 * 60 * 1000,
                AutoReset = true,
                Enabled = true
            };
            loopingPurgeTimer.Elapsed += OnPurgeTimerTicked;

            return Task.CompletedTask;
        }


        private static async void OnSongActivityTimerTicked(object sender, ElapsedEventArgs e)
        {
            Game game = SongDisplay.pickRandomSongDisplay();
            await Global.Client.SetActivityAsync(game);
        }
        

        private static async void OnAfkTimerTicked(object sender, ElapsedEventArgs e)
        {
            await Global.Client.SetStatusAsync(UserStatus.AFK);
        }

        private static async void OnPurgeTimerTicked(object sender, ElapsedEventArgs e)
        {
            Commands.DeleteAllCommandInfo();
        }
    }
}
