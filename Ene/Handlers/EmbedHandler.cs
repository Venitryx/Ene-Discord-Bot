using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ene.Handlers
{
    public static class EmbedHandler
    {

        public static async Task<Embed> CreateBasicEmbedWTitle(string title, string description, Color color, string imageURL = null)
        {
            var embed = await Task.Run(() => (new EmbedBuilder()
                .WithTitle(title)
                .WithDescription(description)
                .WithColor(color)
                .WithImageUrl(imageURL)
                .WithCurrentTimestamp().Build()));
            return embed;
        }

        public static async Task<Embed> CreateBasicEmbedWOTitle(string description, Color color, string imageURL = null)
        {
            var embed = await Task.Run(() => (new EmbedBuilder()
                .WithDescription(description)
                .WithColor(color)
                .WithImageUrl(imageURL)
                .WithCurrentTimestamp().Build()));
            return embed;
        }

        public static async Task<Embed> CreateBasicEmbedTitleOnly(string title, Color color, string imageURL = null)
        {
            var embed = await Task.Run(() => (new EmbedBuilder()
                .WithTitle(title)
                .WithColor(color)
                .WithImageUrl(imageURL)
                .WithCurrentTimestamp().Build()));
            return embed;
        }

        public static async Task<Embed> CreateErrorEmbed(string source, string error)
        {
            var embed = await Task.Run(() => new EmbedBuilder()
                .WithTitle($"ERROR OCCURED FROM - {source}")
                .WithDescription($"**Error Details**: \n{error}")
                .WithColor(Color.DarkRed)
                .WithCurrentTimestamp().Build());
            return embed;
        }
    }
}
