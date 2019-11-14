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
using Ene.Preconditions;
using Ene.SystemLang;

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
        [RequireBotChannel()]
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

        [RequireBotChannel()]
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

        [RequireBotChannel()]
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

        //keep Jikan updated to avoid null exception
        [RequireBotChannel()]
        [Alias("anime")]
        [Command("anime:")]
        public async Task GetAnime([Remainder]string anime)
        {
            IJikan jikan = new Jikan(true);
            var animeSearchResult = await jikan.SearchAnime(anime);

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

        [RequireBotChannel()]
        [Alias("character")]
        [Command("character:")]
        public async Task GetAnimeCharacter([Remainder]string character)
        {
            IJikan jikan = new Jikan(true);
            var characterSearchResult = await jikan.SearchCharacter(character);

            var embed = new EmbedBuilder();
            var fieldAltNames = new EmbedFieldBuilder()
                    .WithName("Alternate Names")
                    .WithValue(GetAltCharacterNames(characterSearchResult))
                    .WithIsInline(true);
            /*
            var fieldA = new EmbedFieldBuilder()
                    .WithName("Anime")
                    .WithValue(GetAnimeography(characterSearchResult))
                    .WithIsInline(true);
                    */


            embed.WithTitle(characterSearchResult.Results.First().Name + "\n" + characterSearchResult.Results.First().URL);
            embed.AddField(fieldAltNames);
            embed.WithThumbnailUrl(characterSearchResult.Results.First().ImageURL);
            embed.WithColor(Global.mainColor);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        public string GetAltCharacterNames(CharacterSearchResult characterSearchResult)
        {
            if (characterSearchResult.Results.First().AlternativeNames != null /*|| characterSearchResult.Results.First().AlternativeNames.Count != 0*/)
            {
                string names = "";
                foreach (string name in characterSearchResult.Results.First().AlternativeNames)
                {
                    names += (name + "\n");
                }
                return names;
            }
            else return "N/A";
        }

        /*
        public static string GetAnimeography(CharacterSearchResult characterSearchResult)
        {
            if (characterSearchResult.Results.First().Animeography != null)
            {
                string animes = "";
                foreach (var anime in characterSearchResult.Results.First().Animeography)
                {
                    animes += (anime.Name + "\n");
                }
                return animes;
            }
            else return "N/A";
        }
        */

        [Alias("subreddit")]
        [Command("subreddit:")]
        public async Task GetSubreddit(string type = "hot", string sub = "/r/animemes")
        {
            var reddit = new Reddit();
            var user = reddit.LogIn(Config.bot.redditUsername, Config.bot.redditPassword);
            var subreddit = reddit.GetSubreddit(sub);
            //subreddit.Subscribe();

            type = StringManipulation.StripPunctuation(type);
            type = StringManipulation.StripSymbols(type);

            int postsCount = 10;

            switch (type)
            {
                case "hot":
                    foreach (var post in subreddit.Hot.Take(postsCount))
                    {
                        if(!post.NSFW)
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
                            embed.WithTitle(StringManipulation.CensorSwearWords(post.Title));
                            embed.WithDescription(post.Url.AbsoluteUri);
                            embed.WithTimestamp(post.CreatedUTC);
                            embed.WithColor(Global.mainColor);
                            await Context.Channel.SendMessageAsync("", false, embed.Build());
                        }
                    }
                    break;
                case "new":
                    foreach (var post in subreddit.New.Take(postsCount))
                    {
                        if(!post.NSFW)
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
                            embed.WithTitle(StringManipulation.CensorSwearWords(post.Title));
                            embed.WithDescription(post.Url.AbsoluteUri);
                            embed.WithTimestamp(post.CreatedUTC);
                            embed.WithColor(Global.mainColor);
                            await Context.Channel.SendMessageAsync("", false, embed.Build());
                        }
                    }
                    break;
                case "controversial":
                    foreach (var post in subreddit.Controversial.Take(postsCount))
                    {
                        if (!post.NSFW)
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
                            embed.WithTitle(StringManipulation.CensorSwearWords(post.Title));
                            embed.WithDescription(post.Url.AbsoluteUri);
                            embed.WithTimestamp(post.CreatedUTC);
                            embed.WithColor(Global.mainColor);
                            await Context.Channel.SendMessageAsync("", false, embed.Build());
                        }
                    }
                    break;
                case "rising":
                    foreach (var post in subreddit.Rising.Take(postsCount))
                    {
                        if (!post.NSFW)
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
                            embed.WithTitle(StringManipulation.CensorSwearWords(post.Title));
                            embed.WithDescription(post.Url.AbsoluteUri);
                            embed.WithTimestamp(post.CreatedUTC);
                            embed.WithColor(Global.mainColor);
                            await Context.Channel.SendMessageAsync("", false, embed.Build());
                        }
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

        [RequireBotChannel()]
        [Command("stats")]
        public async Task GetStats([Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            var account = UserAccounts.GetAccount(target);
            await Context.Channel.SendMessageAsync($"{target.Username} has { account.XP} XP and {account.Points} points.");
        }

        [RequireBotChannel()]
        [Command("data count.")]
        public async Task GetData()
        {
            await Context.Channel.SendMessageAsync("Data has " + DataStorage.GetPairsCount() + " pairs.");
            DataStorage.AddPairToStorage("Count" + DataStorage.GetPairsCount() + " " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(), "TheCount" + DataStorage.GetPairsCount());
        }
    }
}
