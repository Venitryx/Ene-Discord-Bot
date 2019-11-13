using Discord;
using Discord.WebSocket;
using Ene.Handlers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Victoria;
using Victoria.Entities;

namespace Ene.Services
{
    public class MusicService
    {
        
        private LavaRestClient _lavaRestClient;
        private LavaSocketClient _lavaSocketClient;
        private DiscordSocketClient _client;

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
        }

        public async Task<Embed> PlayAsync(string search, string query, ulong guildID)
        {
            var _player = _lavaSocketClient.GetPlayer(guildID);

            if (search.Equals("YouTube") || search.Equals("youtube") || search.Equals("yt"))
            {
                var results = await _lavaRestClient.SearchYouTubeAsync(query);
                if (results.LoadType == LoadType.NoMatches || results.LoadType == LoadType.LoadFailed)
                {
                    string description = String.Format("Sorry, that song doesn't exist or it can't be loaded: {0}.", query);
                    return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);
                }

                var track = results.Tracks.FirstOrDefault();

                if (_player.IsPlaying)
                {
                    _player.Queue.Enqueue(track);
                    string description = String.Format("{0} has been added to the queue!", track.Title);
                    string imageURL = await VictoriaExtensions.FetchThumbnailAsync(track);
                    return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor, imageURL);

                }
                else
                {
                    await _player.PlayAsync(track);
                    string description = String.Format("Now Playing: {0}", track.Title);
                    string imageURL = await VictoriaExtensions.FetchThumbnailAsync(track);
                    return await EmbedHandler.CreateBasicEmbedTitleOnly(description, Global.mainColor, imageURL);
                }
            }
            else if (search.Equals("SoundCloud") || search.Equals("soundcloud") || search.Equals("sc"))
            {
                var results = await _lavaRestClient.SearchSoundcloudAsync(query);
                if (results.LoadType == LoadType.NoMatches || results.LoadType == LoadType.LoadFailed)
                {
                    string description = String.Format("Sorry, that song doesn't exist or it can't be loaded: {0}.", query);
                    return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);
                }

                var track = results.Tracks.FirstOrDefault();

                if (_player.IsPlaying)
                {
                    _player.Queue.Enqueue(track);
                    string description = String.Format("{0} has been added to the queue!", track.Title);
                    string imageURL = await VictoriaExtensions.FetchThumbnailAsync(track);
                    return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor, imageURL);

                }
                else
                {
                    await _player.PlayAsync(track);
                    string description = String.Format("Now Playing: {0}", track.Title);
                    string imageURL = await VictoriaExtensions.FetchThumbnailAsync(track);
                    return await EmbedHandler.CreateBasicEmbedTitleOnly(description, Global.mainColor, imageURL);
                }
            }
            else
            {
                string description = String.Format("Must have a valid search method! Try using \"SoundCloud\" or \"YouTube\".");
                return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);

            }
        }

        public async Task<Embed> StopAsync(ulong guildID)
        {
            var _player = _lavaSocketClient.GetPlayer(guildID);

            if (_player is null)
            {
                string description = "Hey, my music function is already stopped!";
                return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);
            }
            else
            {
                await _player.StopAsync();
                string description = "Okay, fine. Stopping the music.";
                return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);
            }
        }

        public async Task<Embed> SkipAsync(ulong guildID)
        {
            var _player = _lavaSocketClient.GetPlayer(guildID);

            if (_player is null)
            {
                string nothingPlayingDescription = "Hey, nothing is playing.";
                return await EmbedHandler.CreateBasicEmbedWOTitle(nothingPlayingDescription, Global.mainColor);
            }
            if (_player.Queue.Items.Count() is 0)
            {
                string nothingLeftDescription = "Hey, I can't skip if nothing else is in the playlist.";
                return await EmbedHandler.CreateBasicEmbedWOTitle(nothingLeftDescription, Global.mainColor);
            }

            var oldTrack = _player.CurrentTrack;
            await _player.SkipAsync();

            string imageURL = await VictoriaExtensions.FetchThumbnailAsync(_player.CurrentTrack);
            string description = String.Format("Fine. Skipping: {0}\n \nNow Playing: {1}", oldTrack.Title, _player.CurrentTrack.Title);
            return await EmbedHandler.CreateBasicEmbedTitleOnly(description, Global.mainColor, imageURL);
        }

        public async Task<Embed> SetVolumeAsync(int vol, ulong guildID)
        {
            var _player = _lavaSocketClient.GetPlayer(guildID);

            if (_player is null)
            {
                string nothingPlayingDescription = "Can't set the volume if nothing is playing!";
                return await EmbedHandler.CreateBasicEmbedWOTitle(nothingPlayingDescription, Global.mainColor);
            }

            if (vol > 150 || vol <= 2)
            {
                string volumeDescription = "Please use a number between 2 - 150!";
                return await EmbedHandler.CreateBasicEmbedWOTitle(volumeDescription, Global.mainColor);
            }

            await _player.SetVolumeAsync(vol);
            string description = String.Format("The volume is set to {0}!", vol);
            return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);
        }

        public async Task<Embed> PauseOrResumeAsync(ulong guildID)
        {
            var _player = _lavaSocketClient.GetPlayer(guildID);

            if (_player is null)
            {
                string description = "Nothing is playing!";
                return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);
            }

            if (!_player.IsPaused)
            {
                await _player.PauseAsync();
                string description = "Music is paused!";
                return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);
            }
            else
            {
                await _player.ResumeAsync();
                string description = "Music resumed!";
                return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);
            }
        }

        public async Task<Embed> ResumeAsync(ulong guildID)
        {
            var _player = _lavaSocketClient.GetPlayer(guildID);

            if (_player is null)
            {
                string description = "Can't resume if nothing is playing!";
                return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);

            }              

            if (_player.IsPaused)
            {
                await _player.ResumeAsync();
                string description = "Music resumed!";
                return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);
            }

            string alreadyNotPausedDescription = "Music is already unpaused!";
            return await EmbedHandler.CreateBasicEmbedWOTitle(alreadyNotPausedDescription, Global.mainColor);
        }

        public async Task<Embed> RepeatAsync(ulong guildID)
        {
            var _player = _lavaSocketClient.GetPlayer(guildID);

            if (_player is null)
            {
                string description = "Can't repeat if nothing is playing!";
                return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);

            }

            if (_player.Queue.Items.Count() != 0)
            {
                string nothingLeftDescription = "Readding this song to the queue!";
                var track = _player.CurrentTrack;
                _player.Queue.Enqueue(track);
                return await EmbedHandler.CreateBasicEmbedWOTitle(nothingLeftDescription, Global.mainColor);
            }
            else
            {
                string alreadyNotPausedDescription = "Okay! I will repeat this song once it's done!";
                var track = _player.CurrentTrack;
                _player.Queue.Enqueue(track);
                return await EmbedHandler.CreateBasicEmbedWOTitle(alreadyNotPausedDescription, Global.mainColor);
            }
        }

        public async Task<Embed> GetLyrics(ulong guildID)
        {
            var _player = _lavaSocketClient.GetPlayer(guildID);

            if (_player is null)
            {
                string description = "Can't get lyrics if nothing is playing!";
                return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);

            }

            var track = _player.CurrentTrack;
            string lyrics = await VictoriaExtensions.FetchLyricsAsync(track);
            if (string.IsNullOrWhiteSpace(lyrics))
            {
                return await EmbedHandler.CreateBasicEmbedWOTitle("Can't find any lyrics!", Global.mainColor);
            }
            else return await EmbedHandler.CreateBasicEmbedWOTitle(lyrics, Global.mainColor);

        }

        /*
        public async Task<Embed> LoopAsync(string isLooping, ulong guildID)
        {
            var _player = _lavaSocketClient.GetPlayer(guildID);

            if (_player is null)
            {
                string description = "Can't loop if nothing is playing!";
                return await EmbedHandler.CreateBasicEmbedWOTitle(description, Global.mainColor);
            }

            if(isLooping.Equals("y") || isLooping.Equals("yes") || isLooping.Equals("true"))
            {
                
            }
            else if(isLooping.Equals("n") || isLooping.Equals("no") || isLooping.Equals("false"))
            {

            }
            
        }
        */

        private async Task ClientReadyAsync()
        {
            await _lavaSocketClient.StartAsync(_client);
        }

        private async Task TrackFinished(LavaPlayer player, LavaTrack track, TrackEndReason reason)
        {
            if (!reason.ShouldPlayNext())
                return;

            if (!player.Queue.TryDequeue(out var item) || !(item is LavaTrack nextTrack))
            {
                var embed = await EmbedHandler.CreateBasicEmbedWOTitle("There are no more songs.", Global.mainColor);
                await player.TextChannel.SendMessageAsync("", false, embed);
                return;
            }
            else
            {
                string imageURL = await VictoriaExtensions.FetchThumbnailAsync(nextTrack);
                var embed = await EmbedHandler.CreateBasicEmbedTitleOnly(String.Format("Now Playing: {0}", nextTrack.Title), Global.mainColor, imageURL);
                await player.PlayAsync(nextTrack);
                await player.TextChannel.SendMessageAsync("", false, embed);
            }
        }

        private Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }
        
    }
}
