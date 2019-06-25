using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Ene.Services;

namespace Ene.Modules
{
    public class Music : ModuleBase<SocketCommandContext>
    {
        private MusicService _musicService;
        public Color mainColor = new Color(103, 163, 227);
        String punctuation = "?;";

        public Music(MusicService musicService)
        {
            _musicService = musicService;
        }

        [Command("join the channel.")]
        public async Task Join()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(mainColor);

            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                embed.WithDescription("You need to connect to a voice channel first.");
                await ReplyAsync("", false, embed.Build());
                return;
            }
            else
            {
                await _musicService.ConnectAsync(user.VoiceChannel, Context.Channel as ITextChannel);
                embed.WithDescription(String.Format("Now playing in {0}.", user.VoiceChannel.Name));
                await ReplyAsync("", false, embed.Build());
            }
        }

        [Command("leave the channel.")]
        public async Task Leave()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(mainColor);
            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                embed.WithDescription("Please join the channel before commanding me.");
            }
            else
            {
                await _musicService.LeaveAsync(user.VoiceChannel);
                embed.WithDescription(String.Format("Goodbye, {0}", Context.User.Username));
            }
            await ReplyAsync("", false, embed.Build());
        }

        [Command("please play")]
        public async Task Play([Remainder]string query)
        {
            var result = await _musicService.PlayAsync(query, Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithDescription(result);
            embed.WithColor(mainColor);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("please stop playing.")]
        public async Task Stop()
        {
            await _musicService.StopAsync();
            var embed = new EmbedBuilder();
            embed.WithDescription("Okay, fine. Stopping the music.");
            embed.WithColor(mainColor);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("please skip the song.")]
        public async Task Skip()
        {
            var result = await _musicService.SkipAsync();
            var embed = new EmbedBuilder();
            embed.WithDescription(result);
            embed.WithColor(mainColor);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("set volume to")]
        public async Task Volume(int vol)
        {
            var result = await _musicService.SetVolumeAsync(vol);
            var embed = new EmbedBuilder();
            embed.WithDescription(result);
            embed.WithColor(mainColor);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("pause the song.")]
        public async Task Pause()
        {
            var result = await _musicService.PauseOrResumeAsync();
            var embed = new EmbedBuilder();
            embed.WithDescription(result);
            embed.WithColor(mainColor);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("resume playing.")]
        public async Task Resume()
        {
            var result = await _musicService.ResumeAsync();
            var embed = new EmbedBuilder();
            embed.WithDescription(result);
            embed.WithColor(mainColor);
            await ReplyAsync("", false, embed.Build());
        }
    }
}
