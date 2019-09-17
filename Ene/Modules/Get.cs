using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;

using Discord.Audio;

using Ene.Core.UserAccounts;
using Ene.SystemLang;
using Ene.SystemLang.MiscCommands.AreYouCommand;
using Ene.SystemLang.MiscCommands.LikeCommands;
using Ene.SystemLang.MiscCommands.ShouldICommand;

using RedditSharp;
using NReco.ImageGenerator;
using Newtonsoft.Json;
using CoreHtmlToImage;
using static RedditSharp.Things.VotableThing;
using JikanDotNet;

namespace Ene.Modules
{
    [Group("get")]
    public class Get : ModuleBase<SocketCommandContext>
    {
        [Alias("random person.", "a random person", "a random person.")]
        [Command("random person")]
        public async Task GetRandomPerson()
        {
            string json;
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://randomuser.me/api/?gender=male&nat=JP");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string firstName = dataObject.results[0].name.first.ToString();
            firstName = firstName.First().ToString().ToUpper() + firstName.Substring(1);
            string lastName = dataObject.results[0].name.last.ToString();
            lastName = lastName.First().ToString().ToUpper() + lastName.Substring(1);
            string picture = dataObject.results[0].picture.large.ToString();

            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl(picture);
            embed.WithTitle("Found a random person:");
            embed.WithDescription(firstName + " " + lastName);
            embed.WithColor(Global.mainColor);
            await Context.Channel.TriggerTypingAsync();
            await Task.Delay(StringManipulation.milisecondsToDelayPerCharacter(embed.Description));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Alias("server info.", "information about this server.")]
        [Command("server info")]
        public async Task GetServerInfo()
        {
            var embed = new EmbedBuilder();
            var fieldCreatedBy = new EmbedFieldBuilder()
                    .WithName("Created By")
                    .WithValue(Context.Guild.Owner.Nickname + "#" + Context.Guild.Owner.Discriminator)
                    .WithIsInline(true);
            var fieldServerCreated = new EmbedFieldBuilder()
                    .WithName("Date Created")
                    .WithValue(Context.Guild.CreatedAt.UtcDateTime)
                    .WithIsInline(true);
            var fieldMemberCount = new EmbedFieldBuilder()
                    .WithName("Member Count")
                    .WithValue(Context.Guild.GetRole(621081818153746433).Members.Count() + " Unique Members")
                    .WithIsInline(true);

            embed.WithTitle("Server Statistics");
            embed.AddField(fieldCreatedBy).AddField(fieldServerCreated).AddField(fieldMemberCount);
            embed.WithColor(Global.mainColor);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Alias("anime")]
        [Command("anime:")]
        public async Task GetAnime([Remainder]string anime)
        {
            IJikan jikan = new Jikan(true);
            AnimeSearchResult animeSearchResult = await jikan.SearchAnime(anime);

            var embed = new EmbedBuilder();
            var fieldAiring = new EmbedFieldBuilder()
                    .WithName("Status")
                    .WithValue(GetIsAnimeAiring(animeSearchResult))
                    .WithIsInline(true);
            var fieldStartDate = new EmbedFieldBuilder()
                    .WithName("Aired")
                    .WithValue(GetAnimeAired(animeSearchResult))
                    .WithIsInline(true);
            var fieldRating = new EmbedFieldBuilder()
                    .WithName("Rating")
                    .WithValue(animeSearchResult.Results.First().Rated)
                    .WithIsInline(true);
            var fieldEpisodes = new EmbedFieldBuilder()
                    .WithName("Episodes")
                    .WithValue(animeSearchResult.Results.First().Episodes)
                    .WithIsInline(true);


            embed.WithTitle(animeSearchResult.Results.First().Title + "\n" + animeSearchResult.Results.First().URL);
            embed.AddField(fieldAiring).AddField(fieldStartDate).AddField(fieldRating).AddField(fieldEpisodes);
            embed.WithDescription(animeSearchResult.Results.First().Description);
            embed.WithThumbnailUrl(animeSearchResult.Results.First().ImageURL);
            embed.WithColor(Global.mainColor);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Alias("this season's anime")]
        [Command("this season's anime.")]
        public async Task GetSeason()
        {
            IJikan jikan = new Jikan(true);
            Season season = jikan.GetSeason().Result;
            string list = "";
            int i = 1, limit = 20;
            var embed = new EmbedBuilder();
            foreach (var seasonEntry in season.SeasonEntries)
            {
                if (i < limit + 1)
                {
                    list += (i + ". " + seasonEntry.Title + "\n");
                    i++;
                }
                else break;
            }

            embed.WithTitle(season.SeasonName + " " + season.SeasonYear + " Anime");
            embed.WithDescription(list);
            embed.WithColor(Global.mainColor);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        private static string GetIsAnimeAiring(AnimeSearchResult animeSearchResult)
        {
            if (animeSearchResult.Results.First().Airing) return "Currently Airing";
            else return "Finished Airing";
        }

        private static string GetAnimeAired(AnimeSearchResult animeSearchResult)
        {
            return animeSearchResult.Results.First().StartDate.ToString() + " to " + GetAnimeEndDate(animeSearchResult);
        }

        private static string GetAnimeEndDate(AnimeSearchResult animeSearchResult)
        {
            if (animeSearchResult.Results.First().EndDate.HasValue == false) return "?";
            else return animeSearchResult.Results.First().EndDate.ToString();
        }

        [Command("subreddit:")]
        public async Task GetSubreddit(string type, string sub = "/r/animemes")
        {
            var reddit = new Reddit();
            var user = reddit.LogIn(Config.bot.redditUsername, Config.bot.redditPassword);
            var subreddit = reddit.GetSubreddit(sub);
            subreddit.Subscribe();

            type = StringManipulation.StripPunctuation(type);
            type = StringManipulation.StripSymbols(type);

            switch (type)
            {
                case "hot":
                    foreach (var post in subreddit.Hot.Take(5))
                    {
                        var embed = new EmbedBuilder();
                        try
                        {
                            embed.WithThumbnailUrl(post.Thumbnail.AbsoluteUri);
                        }
                        catch (InvalidOperationException)
                        {

                        }
                        embed.WithAuthor("u/" + post.AuthorName);
                        embed.WithTitle(post.Title);
                        embed.WithDescription(post.Url.AbsoluteUri);
                        embed.WithTimestamp(post.CreatedUTC);
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                    }
                    break;
                case "new":
                    foreach (var post in subreddit.New.Take(5))
                    {
                        var embed = new EmbedBuilder();
                        try
                        {
                            embed.WithThumbnailUrl(post.Thumbnail.AbsoluteUri);
                        }
                        catch (InvalidOperationException)
                        {

                        }
                        embed.WithAuthor("u/" + post.AuthorName);
                        embed.WithTitle(post.Title);
                        embed.WithDescription(post.Url.AbsoluteUri);
                        embed.WithTimestamp(post.CreatedUTC);
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                    }
                    break;
                case "controversial":
                    foreach (var post in subreddit.Controversial.Take(5))
                    {
                        var embed = new EmbedBuilder();
                        try
                        {
                            embed.WithThumbnailUrl(post.Thumbnail.AbsoluteUri);
                        }
                        catch (InvalidOperationException)
                        {

                        }
                        embed.WithAuthor("u/" + post.AuthorName);
                        embed.WithTitle(post.Title);
                        embed.WithDescription(post.Url.AbsoluteUri);
                        embed.WithTimestamp(post.CreatedUTC);
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                    }
                    break;
                case "rising":
                    foreach (var post in subreddit.Rising.Take(5))
                    {
                        var embed = new EmbedBuilder();
                        try
                        {
                            embed.WithThumbnailUrl(post.Thumbnail.AbsoluteUri);
                        }
                        catch (InvalidOperationException)
                        {

                        }
                        embed.WithAuthor("u/" + post.AuthorName);
                        embed.WithTitle(post.Title);
                        embed.WithDescription(post.Url.AbsoluteUri);
                        embed.WithTimestamp(post.CreatedUTC);
                        embed.WithColor(Global.mainColor);
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                    }
                    break;
                default:
                    var defaultEmbed = new EmbedBuilder();
                    defaultEmbed.WithDescription("Invalid type! I can do types that are \"hot\", \"new\", \"controversial\", or \"rising.\"");
                    defaultEmbed.WithColor(Global.mainColor);
                    await Context.Channel.SendMessageAsync("", false, defaultEmbed.Build());
                    break;
            }
        }

        [Command("stats")]
        public async Task GetStats([Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            var account = UserAccounts.GetAccount(target);
            await Context.Channel.SendMessageAsync($"{target.Username} has { account.XP} XP and {account.Points} points.");
        }
    }
}
