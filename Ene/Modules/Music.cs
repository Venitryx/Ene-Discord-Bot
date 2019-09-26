using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Ene.Services;
using Ene.SystemLang;

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
                embed.WithDescription("Please join the channel before asking me!");
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
                embed.WithDescription("Please join the channel before asking me!");
            }
            else
            {
                await _musicService.LeaveAsync(user.VoiceChannel);
                embed.WithDescription(String.Format("Okay {0}, see ya later!", GetMasterPlaceholder(Context.User)));
            }
            await ReplyAsync("", false, embed.Build());
        }

        private string GetMasterPlaceholder(SocketUser user)
        {
            if (StringManipulation.isBotOwner(user))
            {
                return "Master";
            }
            else return user.Username;
        }

        [Command("play")]
        public async Task Play(string searchMethod, [Remainder]string query)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Global.mainColor);

            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                embed.WithDescription("Please join the channel before asking me!");
                await ReplyAsync("", false, embed.Build());
                return;
            }
            else
            {
                await _musicService.ConnectAsync(user.VoiceChannel, Context.Channel as ITextChannel);
                var result = await _musicService.PlayAsync(searchMethod, query, Context.Guild.Id);
                await ReplyAsync("", false, result);
            }
        }

        [Command("stop playing.")]
        public async Task Stop()
        {
            var result = await _musicService.StopAsync(Context.Guild.Id);
            await ReplyAsync("", false, result);
        }

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

        [Command("pause the song.")]
        public async Task Pause()
        {
            var result = await _musicService.PauseOrResumeAsync(Context.Guild.Id);
            await ReplyAsync("", false, result);
        }

        [Command("resume playing.")]
        public async Task Resume()
        {
            var result = await _musicService.ResumeAsync(Context.Guild.Id);
            await ReplyAsync("", false, result);
        }

        [Command("repeat the song.")]
        public async Task Repeat()
        {
            var result = await _musicService.ResumeAsync(Context.Guild.Id);
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
