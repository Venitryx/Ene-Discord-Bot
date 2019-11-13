using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Ene.Core
{
    public class Song
    {
        public String RomanizedAlbumTitle, AlbumTitle, RomanizedTitle, Title, RomanizedArtist, Artist, RomanizedSinger, Singer, Uri, Tags;
        private static int _songCount;
        public Song(String RomanizedAlbumTitle = null, String AlbumTitle = null, String RomanizedTitle = null, String Title = null, String RomanizedArtist = null, String Artist = null, String RomanizedSinger = null, String Singer = null, String Uri = null, String Tags = null)
        {
            this.RomanizedAlbumTitle = RomanizedAlbumTitle;
            this.AlbumTitle = AlbumTitle;
            this.RomanizedTitle = RomanizedTitle;
            this.Title = Title;
            this.RomanizedArtist = RomanizedArtist;
            this.Artist = Artist;
            this.RomanizedSinger = RomanizedSinger;
            this.Singer = Singer;
            this.Uri = Uri;
            this.Tags = Tags;
            _songCount++;
        }

        public String GetRomanizedAlbumTitle()
        {
            return RomanizedAlbumTitle;
        }

        public String GetAlbumTitle()
        {
            return AlbumTitle;
        }

        public String GetRomanizedTitle()
        {
            return RomanizedTitle;
        }

        public String GetTitle()
        {
            return Title;
        }

        public String GetRomanizedArtist()
        {
            return RomanizedArtist;
        }

        public String GetArtist()
        {
            return Artist;
        }

        public String GetRomanizedSinger()
        {
            return RomanizedSinger;
        }

        public String GetSinger()
        {
            return Singer;
        }

        public String GetUri()
        {
            return Uri;
        }

        public String GetTags()
        {
            return Tags;
        }

        public static int GetSongCount()
        {
            return _songCount;
        }

        public void SetRomanizedAlbumTitle(String RomanizedAlbumTitle)
        {
            this.RomanizedAlbumTitle = RomanizedAlbumTitle;
            if (AlbumTitle is null) AlbumTitle = RomanizedAlbumTitle;
        }

        public void SetAlbumTitle(String AlbumTitle)
        {
            this.AlbumTitle = AlbumTitle;
        }

        public void SetRomanizedTitle(String RomanizedTitle)
        {
            this.RomanizedTitle = RomanizedTitle;
            if (Title is null) Title = RomanizedTitle;
        }

        public void SetTitle(String Title)
        {
            this.Title = Title;
        }

        public void SetRomanizedArtist(String RomanizedArtist)
        {
            this.RomanizedArtist = RomanizedArtist;
            if (Artist is null) Artist = RomanizedArtist;
        }

        public void SetArtist(String Artist)
        {
            this.Artist = Artist;
        }

        public void SetRomanizedSinger(String RomanizedSinger)
        {
            this.RomanizedSinger = RomanizedSinger;
            if (Singer is null) Singer = RomanizedSinger;
        }

        public void SetSinger(String Singer)
        {
            this.Singer = Singer;
        }

        public void SetUri(String Uri)
        {
            this.Uri = Uri;
        }

        public void SetTags(String Tags)
        {
            this.Tags = Tags;
        }
    }
}
