using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

using Discord;
using Discord.WebSocket;

namespace Ene.Core.Songs
{
    internal static class SongDisplay
    {
        public static int index;
        public static List<Song> songs;
        internal static Game pickRandomSong()
        {
            Random r = new Random();
            songs = Songs.songConfig.Songs;

            index = r.Next(songs.Count);
            if (Songs.songConfig.UseRomanizedNames)
            {
                Game game = new Game(songs.ElementAt(index).GetRomanizedTitle(), ActivityType.Listening);
                return game;
            }
            else if (!Songs.songConfig.UseRomanizedNames)
            {
                Game game = new Game(songs.ElementAt(index).GetTitle(), ActivityType.Listening);
                return game;
            }
            else return new Game("Invalid song config!", ActivityType.Playing);
        }

        internal static Game pickNextSong()
        {
            songs = Songs.songConfig.Songs;

            index++;
            if (index > songs.Count) index = 0;
            if (Songs.songConfig.UseRomanizedNames)
            {
                Game game = new Game(songs.ElementAt(index).GetRomanizedTitle(), ActivityType.Listening);
                return game;
            }
            else if (!Songs.songConfig.UseRomanizedNames)
            {
                Game game = new Game(songs.ElementAt(index).GetTitle(), ActivityType.Listening);
                return game;
            }
            else return new Game("Invalid song config!", ActivityType.Playing);
        }

        internal static Game pickSongInQueue()
        {
            songs = Songs.songConfig.Songs;

            index++;
            if (index > songs.Count) index = 0;
            if (Songs.songConfig.UseRomanizedNames)
            {
                Game game = new Game(songs.ElementAt(index).GetRomanizedTitle(), ActivityType.Listening);
                return game;
            }
            else if (!Songs.songConfig.UseRomanizedNames)
            {
                Game game = new Game(songs.ElementAt(index).GetTitle(), ActivityType.Listening);
                return game;
            }
            else return new Game("Invalid song config!", ActivityType.Playing);
        }
    }
}
