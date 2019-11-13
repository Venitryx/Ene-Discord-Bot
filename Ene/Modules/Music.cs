using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Ene.Preconditions;
using Ene.Services;
using Ene.SystemLang;
using Ene.Core.Servers;
using System.Linq;

namespace Ene.Modules
{
    [RequireMusicChannel()]
    public class Music : ModuleBase<SocketCommandContext>
    {
        
        private MusicService _musicService;

        public Music(MusicService musicService)
        {
            _musicService = musicService;
        }
        

        //work on order so that it joins the default voice channel if it is set and the user is not in a channel, and if the voice channel is not set, it joins the channel the user is in.
        [Alias("join the channel", "join.", "join")]
        [Command("join the channel.")]
        public async Task Join()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Global.mainColor);

            var serverResult = from s in Servers.servers
                               where
                               s.GuildID == Context.Guild.Id
                               select s;
            var serverInfo = serverResult.FirstOrDefault();
            if (serverInfo is null) serverInfo = Servers.GetServerInfo(Context.Guild.Id);

            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                embed.WithDescription("Please join the channel before asking me!");
                await ReplyAsync("", false, embed.Build());
                return;
            }
            else if (user.VoiceChannel.Id != serverInfo.MusicVoiceChannelID && serverInfo.MusicVoiceChannelID != 0)
            {
                embed.WithDescription(String.Format("Sorry, but I can not play music in that voice channel."));
                await ReplyAsync("", false, embed.Build());
            }
            else
            {
                await _musicService.ConnectAsync(user.VoiceChannel, Context.Channel as ITextChannel);
                embed.WithDescription(String.Format("Now joining :loud_sound: {0}.", user.VoiceChannel.Name.ToLower()));
                await ReplyAsync("", false, embed.Build());
            }
        }

        [Alias("leave the channel", "leave.", "leave")]
        [Command("leave the channel.")]
        public async Task Leave()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Global.mainColor);

            var serverResult = from s in Servers.servers
                               where
                               s.GuildID == Context.Guild.Id
                               select s;
            var serverInfo = serverResult.FirstOrDefault();
            if (serverInfo is null) serverInfo = Servers.GetServerInfo(Context.Guild.Id);

            var user = Context.User as SocketGuildUser;

            if (user.VoiceChannel is null)
            {
                embed.WithDescription("Please join the channel before asking me!");
            }
            else if (user.VoiceChannel.Id != serverInfo.MusicVoiceChannelID && serverInfo.MusicVoiceChannelID != 0)
            {
                embed.WithDescription(String.Format("Sorry, but I can not leave since you're not in the voice channel!"));
                await ReplyAsync("", false, embed.Build());
            }
            else
            {
                await _musicService.LeaveAsync(user.VoiceChannel);
                embed.WithDescription(String.Format("Okay {0}, see ya later!", StringManipulation.GetMasterPlaceholder(Context.User)));
            }
            await ReplyAsync("", false, embed.Build());
        }

        [Command("play")]
        public async Task Play(string searchMethod, [Remainder]string query)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Global.mainColor);

            var serverResult = from s in Servers.servers
                               where
                               s.GuildID == Context.Guild.Id
                               select s;
            var serverInfo = serverResult.FirstOrDefault();
            if (serverInfo is null) serverInfo = Servers.GetServerInfo(Context.Guild.Id);

            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                embed.WithDescription("Please join the channel before asking me!");
                await ReplyAsync("", false, embed.Build());
                return;
            }
            else if (user.VoiceChannel.Id != serverInfo.MusicVoiceChannelID && serverInfo.MusicVoiceChannelID != 0)
            {
                embed.WithDescription(String.Format("Sorry, but I can not play since you're not in the right voice channel!"));
                await ReplyAsync("", false, embed.Build());
            }
            else
            {
                await _musicService.ConnectAsync(user.VoiceChannel, Context.Channel as ITextChannel);
                var result = await _musicService.PlayAsync(searchMethod, query, Context.Guild.Id);
                await ReplyAsync("", false, result);
            }
        }

        [Alias("stop playing", "stop.", "stop")]
        [Command("stop playing.")]
        public async Task Stop()
        {
            var result = await _musicService.StopAsync(Context.Guild.Id);
            await ReplyAsync("", false, result);
        }

        [Alias("get lyrics")]
        [Command("get lyrics.")]
        public async Task Lyrics()
        {
            var result = await _musicService.GetLyrics(Context.Guild.Id);
            await ReplyAsync("", false, result);
        }

        [Alias("skip the song", "skip.", "skip")]
        [Command("skip the song.")]
        public async Task Skip()
        {
            var result = await _musicService.SkipAsync(Context.Guild.Id);
            await ReplyAsync("", false, result);
        }

        [Command("set volume to")]
        public async Task Volume(int vol)
        {
            var result = await _musicService.SetVolumeAsync(vol, Context.Guild.Id);
            await ReplyAsync("", false, result);
        }

        [Alias("pause the song", "pause.", "pause")]
        [Command("pause the song.")]
        public async Task Pause()
        {
            var result = await _musicService.PauseOrResumeAsync(Context.Guild.Id);
            await ReplyAsync("", false, result);
        }

        [Alias("resume playing", "resume.", "resume")]
        [Command("resume playing.")]
        public async Task Resume()
        {
            var result = await _musicService.ResumeAsync(Context.Guild.Id);
            await ReplyAsync("", false, result);
        }

        [Alias("repeat the song", "repeat this song.", "repeat this song", "repeat.", "repeat")]
        [Command("repeat the song.")]
        public async Task Repeat()
        {
            var result = await _musicService.RepeatAsync(Context.Guild.Id);
            await ReplyAsync("", false, result);
        }

        internal async Task SendIndividualMessages(string[] messages, bool useEmbed = false)
        {
            if (useEmbed)
            {
                var embed = new EmbedBuilder();
                embed.WithDescription("");
                embed.WithColor(Global.mainColor);
                await Context.Channel.TriggerTypingAsync();
                await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(""));
                var message = await ReplyAsync("", false, embed.Build());

                for (int i = 0; i < messages.Length; i++)
                {
                    await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(messages[i]));
                    await message.ModifyAsync(x => x.Embed = embed.WithDescription(messages[i]).Build());
                }
            }
            else
            {
                await Context.Channel.TriggerTypingAsync();
                await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(""));
                var message = await ReplyAsync("");

                for (int i = 0; i < messages.Length; i++)
                {
                    await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(messages[i]));
                    await message.ModifyAsync(x => x.Content = messages[i]);
                }
            }
        }
    }
}
