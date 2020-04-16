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
        public static int songIndex;
        public static int index = 0;
        public static List<Song> songs;
        internal static Game pickRandomSong()
        {
            Random r = new Random();
            songs = Songs.songConfig.Songs;

            songIndex = r.Next(songs.Count);
            if (Songs.songConfig.UseRomanizedNames)
            {
                Game game = new Game(songs.ElementAt(songIndex).GetRomanizedTitle(), ActivityType.Listening);
                return game;
            }
            else if (!Songs.songConfig.UseRomanizedNames)
            {
                Game game = new Game(songs.ElementAt(songIndex).GetTitle(), ActivityType.Listening);
                return game;
            }
            else return new Game("Invalid song config!", ActivityType.Playing);
        }

        internal static Game pickNextSong()
        {
            songs = Songs.songConfig.Songs;

            songIndex++;
            if (songIndex > songs.Count) songIndex = 0;
            if (Songs.songConfig.UseRomanizedNames)
            {
                Game game = new Game(songs.ElementAt(songIndex).GetRomanizedTitle(), ActivityType.Listening);
                return game;
            }
            else if (!Songs.songConfig.UseRomanizedNames)
            {
                Game game = new Game(songs.ElementAt(songIndex).GetTitle(), ActivityType.Listening);
                return game;
            }
            else return new Game("Invalid song config!", ActivityType.Playing);
        }

        internal static Game pickSongInQueue(string link)
        {
            Song song = Songs.songConfig.Songs.Where(s => s.Uri.Equals(link)).First();
            return new Game(song.GetTitle(), ActivityType.Listening);
        }
    }
}
