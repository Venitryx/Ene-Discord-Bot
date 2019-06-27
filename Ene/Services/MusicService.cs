﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using Ene.Core;

using Victoria;
using Victoria.Entities;

namespace Ene.Services
{
    public class MusicService
    {
        private LavaRestClient _lavaRestClient;
        private LavaSocketClient _lavaSocketClient;
        private DiscordSocketClient _client;
        private LavaPlayer _player;

        public Color mainColor = new Color(103, 163, 227);

        public MusicService(LavaRestClient lavaRestClient, DiscordSocketClient client, LavaSocketClient lavaSocketClient)
        {
            _client = client;
            _lavaRestClient = lavaRestClient;
            _lavaSocketClient = lavaSocketClient;
        }

        public Task InitializeAsync()
        {
            _client.Ready += ClientReadyAsync;
            _lavaSocketClient.Log += LogAsync;
            _lavaSocketClient.OnTrackFinished += TrackFinished;
            return Task.CompletedTask;
        }

        public async Task ConnectAsync(SocketVoiceChannel voiceChannel, ITextChannel textChannel)
            => await _lavaSocketClient.ConnectAsync(voiceChannel, textChannel);

        public async Task LeaveAsync(SocketVoiceChannel voiceChannel)
        {
            await _lavaSocketClient.DisconnectAsync(voiceChannel);
            _player = null;
        }

        public async Task<string> PlayAsync(string query, ulong guildId)
        {
            _player = _lavaSocketClient.GetPlayer(guildId);
            var results = await _lavaRestClient.SearchYouTubeAsync(query);
            if (results.LoadType == LoadType.NoMatches || results.LoadType == LoadType.LoadFailed)
            {
                return String.Format("Sorry, can't find any songs by {0}.", query);
            }

            var track = results.Tracks.FirstOrDefault();

            if (_player.IsPlaying)
            { 
                _player.Queue.Enqueue(track);
                RepeatingTimer.loopingSongActivityTimer.Stop();
                return String.Format("{0} has been added to the queue!", track.Title);

            }
            else
            {
                Game game = new Game(track.Title, ActivityType.Listening);
                await Global.Client.SetActivityAsync(game);
                RepeatingTimer.loopingSongActivityTimer.Stop();

                await _player.PlayAsync(track);
                return String.Format("Now Playing: {0}", track.Title);
            }
        }

        public async Task<string> StopAsync()
        {
            if (_player is null)
            {
                return "Hey, my music function is already stopped!";
            }
            else
            {
                await _player.StopAsync();
                _player = null;
                Game game = RepeatingTimer.pickRandomSongDisplay();
                await Global.Client.SetActivityAsync(game);
                RepeatingTimer.loopingSongActivityTimer.Start();
                return "Okay, fine. Stopping the music.";
            }
        }

        public async Task<string> SkipAsync()
        {
            if (_player is null)
            {
                return "Hey, nothing is playing.";
            }
            if (_player.Queue.Items.Count() is 0)
            {
                return "Hey, I can't skip if nothing else is in the playlist.";
            }

            var oldTrack = _player.CurrentTrack;
            await _player.SkipAsync();

            Game game = new Game(_player.CurrentTrack.Title, ActivityType.Listening);
            await Global.Client.SetActivityAsync(game);
            RepeatingTimer.loopingSongActivityTimer.Stop();

            return String.Format("Fine. Skipping: {0}\n \nNow Playing: {1}", oldTrack.Title, _player.CurrentTrack.Title);
        }

        public async Task<string> SetVolumeAsync(int vol)
        {
            if (_player is null)
                return "Nothing is playing.";

            if (vol > 150 || vol <= 2)
            {
                return "Please use a number between 2 - 150.";
            }

            await _player.SetVolumeAsync(vol);
            return String.Format("The volume is set to: {0}.", vol);
        }

        public async Task<string> PauseOrResumeAsync()
        {
            if (_player is null)
                return "Nothing is playing.";

            if (!_player.IsPaused)
            {
                await _player.PauseAsync();
                return "Music is paused.";
            }
            else
            {
                await _player.ResumeAsync();
                return "Music resumed.";
            }
        }

        public async Task<string> ResumeAsync()
        {
            if (_player is null)
                return "Nothing is playing.";

            if (_player.IsPaused)
            {
                await _player.ResumeAsync();
                return "Music resumed.";
            }

            return "Music is already not paused.";
        }


        private async Task ClientReadyAsync()
        {
            await _lavaSocketClient.StartAsync(_client);
        }

        private async Task TrackFinished(LavaPlayer player, LavaTrack track, TrackEndReason reason)
        {
            if (!reason.ShouldPlayNext())
                return;

            var embed = new EmbedBuilder();
            embed.WithColor(mainColor);

            if (!player.Queue.TryDequeue(out var item) || !(item is LavaTrack nextTrack))
            {
                embed.WithDescription("There are no more songs.");

                Game game = RepeatingTimer.pickRandomSongDisplay();
                await Global.Client.SetActivityAsync(game);
                RepeatingTimer.loopingSongActivityTimer.Start();
                await player.TextChannel.SendMessageAsync("", false, embed.Build());
                return;
            }
            else
            {
                embed.WithDescription(String.Format("Now Playing: {0}", nextTrack.Title));
                await player.PlayAsync(nextTrack);
                Game game = new Game(nextTrack.Title, ActivityType.Listening);
                await Global.Client.SetActivityAsync(game);
                await player.TextChannel.SendMessageAsync("", false, embed.Build());
            }
        }

        private Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }
    }
}