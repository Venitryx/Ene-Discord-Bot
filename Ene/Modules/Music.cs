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

        public Music(MusicService musicService)
        {
            _musicService = musicService;
        }

        [Command("join the channel.")]
        public async Task Join()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Global.mainColor);

            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                embed.WithDescription("Please join the channel before commanding me.");
                await ReplyAsync("", false, embed.Build());
                return;
            }
            else
            {
                await _musicService.ConnectAsync(user.VoiceChannel, Context.Channel as ITextChannel);
                embed.WithDescription(String.Format("Now joining :loud_sound: {0}.", user.VoiceChannel.Name.ToLower()));
                await ReplyAsync("", false, embed.Build());
            }
        }

        [Command("leave the channel.")]
        public async Task Leave()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Global.mainColor);
            var user = Context.User as SocketGuildUser;

            if (user.VoiceChannel is null)
            {
                embed.WithDescription("Please join the channel before commanding me.");
            }
            else
            {
                await _musicService.LeaveAsync(user.VoiceChannel);
                embed.WithDescription(String.Format("Okay {0}, I will see you later.", Context.User.Username));
            }
            await ReplyAsync("", false, embed.Build());
        }

        [Command("play")]
        public async Task Play([Remainder]string query)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Global.mainColor);

            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                embed.WithDescription("Please join the channel before commanding me.");
                await ReplyAsync("", false, embed.Build());
                return;
            }
            else
            {
                await _musicService.ConnectAsync(user.VoiceChannel, Context.Channel as ITextChannel);
                var result = await _musicService.PlayAsync(query, Context.Guild.Id);
                embed.WithDescription(result);
                embed.WithColor(Global.mainColor);
                await ReplyAsync("", false, embed.Build());
            }
        }

        [Command("stop playing.")]
        public async Task Stop()
        {
            var result = await _musicService.StopAsync();
            var embed = new EmbedBuilder();
            embed.WithDescription(result);
            embed.WithColor(Global.mainColor);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("skip the song.")]
        public async Task Skip()
        {
            var result = await _musicService.SkipAsync();
            var embed = new EmbedBuilder();
            embed.WithDescription(result);
            embed.WithColor(Global.mainColor);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("set volume to")]
        public async Task Volume(int vol)
        {
            var result = await _musicService.SetVolumeAsync(vol);
            var embed = new EmbedBuilder();
            embed.WithDescription(result);
            embed.WithColor(Global.mainColor);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("pause the song.")]
        public async Task Pause()
        {
            var result = await _musicService.PauseOrResumeAsync();
            var embed = new EmbedBuilder();
            embed.WithDescription(result);
            embed.WithColor(Global.mainColor);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("resume playing.")]
        public async Task Resume()
        {
            var result = await _musicService.ResumeAsync();
            var embed = new EmbedBuilder();
            embed.WithDescription(result);
            embed.WithColor(Global.mainColor);
            await ReplyAsync("", false, embed.Build());
        }

        [Command("repeat the song")]
        public async Task Repeat()
        {
            var result = await _musicService.ResumeAsync();
            var embed = new EmbedBuilder();
            embed.WithDescription(result);
            embed.WithColor(Global.mainColor);
            await ReplyAsync("", false, embed.Build());
        }
    }
}
