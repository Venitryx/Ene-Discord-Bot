using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord.WebSocket;
using Discord;
using static Ene.Core.Songs.SongDataStorage;

namespace Ene.Core.Songs
{
    public static class Songs
    {
        internal static SongConfig songConfig;
        internal static List<Song> songs;
        private static string songConfigPath = "Resources/songs.json";

        static Songs()
        {
            LoadSongConfig();
        }

        //saves file
        public static void SaveSongConfig()
        {
            SongDataStorage.SaveSongConfig(songConfig, songConfigPath);
        }

        //loads file
        public static void LoadSongConfig()
        {
            songConfig = SongDataStorage.LoadSongConfig(songConfigPath);
        }
        public static void Initialize()
        {
            songConfig = new SongConfig();

            songs = new List<Song>();
            Song lostTimePrologue = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Days", AlbumTitle: "メカクシティデイズ", RomanizedTitle: "Lost Time Prologue", Title: "ロスタイムプロローグ", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", Uri: "https://www.youtube.com/watch?v=K8khTs32yAk");
            Song kaienPanzermast = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Days", AlbumTitle: "メカクシティデイズ", RomanizedTitle: "Kaien Panzermast", Title: "カイエンパンザマスト", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", Uri: "https://www.youtube.com/watch?v=iIrpmqAiO3E");
            Song summerEndRoll = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Records", AlbumTitle: "メカクシティレコーズ", RomanizedTitle: "Summer End Roll", Title: "サマーエンドロール", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", Uri: "https://www.youtube.com/watch?v=s3Jwm7YUEsk");
            Song cryingPrologue = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Records", AlbumTitle: "メカクシティレコーズ", RomanizedTitle: "Crying Prologue", Title: "クライングプロローグ", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", Uri: "https://www.youtube.com/watch?v=aHu7R3G_VXI");
            Song jinzouEnemy = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Days", AlbumTitle: "メカクシティデイズ", RomanizedTitle: "Jinzou Enemy", Title: "人造エネミー", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "Hatsune Miku", Singer: "初音ミク", Uri: "https://www.youtube.com/watch?v=hN_UJHwDFk0");
            Song mekakushiCode = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Days", AlbumTitle: "メカクシティデイズ", RomanizedTitle: "Mekakushi Code", Title: "メカクシコード", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "Hatsune Miku", Singer: "初音ミク", Uri: "https://www.youtube.com/watch?v=wM-U279Z03c");
            Song kagerouDaze = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Days", AlbumTitle: "メカクシティデイズ", RomanizedTitle: "Kagerou Daze", Title: "カゲロウデイズ", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "Hatsune Miku", Singer: "初音ミク", Uri: "https://www.youtube.com/watch?v=A7KNYk4f3XQ");
            Song headphoneActors = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Days", AlbumTitle: "メカクシティデイズ", RomanizedTitle: "Headphone Actor", Title: "ヘッドフォンアクター", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=Hsc4LW-la-M");
            Song kuusouForest = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Days", AlbumTitle: "メカクシティデイズ", RomanizedTitle: "Kuusou Forest", Title: "空想フォレスト", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=vXB2tFFYr58");
            Song konohaStateOfTheWorld = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Days", AlbumTitle: "メカクシティデイズ", RomanizedTitle: "Konoha's State of the World", Title: "コノハの世界事情", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "Hatsune Miku & IA", Singer: "初音ミク & イア", Uri: "https://www.youtube.com/watch?v=u6K9gv487BQ");
            Song kisaragiAttention = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Days", AlbumTitle: "メカクシティデイズ", RomanizedTitle: "Kisaragi Attention", Title: "如月アテンション", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=RkT3UP9oNzA");
            Song childrenRecord = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Records", AlbumTitle: "メカクシティレコーズ", RomanizedTitle: "Children Record -Re Ver.-", Title: "チルドレンレコード -Re Ver.-", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=ZwuaYUer00U");
            Song yobanashiDeceive = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Records", AlbumTitle: "メカクシティレコーズ", RomanizedTitle: "Yobanashi Deceive", Title: "夜咄ディセイブ", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=98YWS7WuB0o");
            Song lostTimeMemory = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Records", AlbumTitle: "メカクシティレコーズ", RomanizedTitle: "Lost Time Memory", Title: "ロスタイムメモリー", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=v9wrDGfYCWA");
            Song ayanoTheoryOfHappiness = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Records", AlbumTitle: "メカクシティレコーズ", RomanizedTitle: "Ayano's Theory of Happiness", Title: "アヤノの幸福理論", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=aifXu5kcETI");
            Song otsukimiRecital = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Records", AlbumTitle: "メカクシティレコーズ", RomanizedTitle: "Otsukimi Recital", Title: "オツキミリサイタル", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=soDEYtSkC90");
            Song yuukeiYesterday = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Records", AlbumTitle: "メカクシティレコーズ", RomanizedTitle: "Yuukei Yesterday", Title: "夕景イエスタデイ", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=rE-yOTrPgpg");
            Song outerScience = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Records", AlbumTitle: "メカクシティレコーズ", RomanizedTitle: "Outer Science", Title: "アウターサイエンス", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=E0q7B1W6ANs");
            Song summertimeRecord = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Records", AlbumTitle: "メカクシティレコーズ", RomanizedTitle: "Summertime Record", Title: "サマータイムレコード", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=tOY2EtbYI1U");
            Song shissouWord = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Reload", AlbumTitle: "メカクシティリロード", RomanizedTitle: "Shissou Word", Title: "失想ワアド", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "Hatsune Miku", Singer: "初音ミク", Uri: "https://www.youtube.com/watch?v=g4Qqu16ffVk");
            Song additionalMemory = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Reload", AlbumTitle: "メカクシティリロード", RomanizedTitle: "Additional Memory", Title: "アディショナルメモリー", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "Hatsune Miku", Singer: "初音ミク", Uri: "https://www.youtube.com/watch?v=q2QfH1JqtkY");
            Song toumeiAnswer = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Days", AlbumTitle: "メカクシティデイズ", RomanizedTitle: "Toumei Answer", Title: "透明アンサー", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=oIQqGdfdR8Y");
            Song eneCyberJourney = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Days", AlbumTitle: "メカクシティデイズ", RomanizedTitle: "Ene's Cyber Journey", Title: "エネの電脳紀行", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=45kYbjrM4fg");
            Song gunjouRain = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Records", AlbumTitle: "メカクシティレコーズ", RomanizedTitle: "Gunjou Rain", Title: "群青レイン -Re Ver.-", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=k-q1dFOrWao");
            Song shinigamiRecord = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Days", AlbumTitle: "メカクシティデイズ", RomanizedTitle: "Shinigami Records", Title: "シニガミレコード", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=BH98UpNGiT4");
            Song deadAndSeek = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Days", AlbumTitle: "メカクシティデイズ", RomanizedTitle: "Dead and Seek", Title: "デッドアンドシーク", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=99PzEsFYQk8");
            Song maryFictionalWorld = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Records", AlbumTitle: "メカクシティレコーズ", RomanizedTitle: "Mary's Fictional World", Title: "マリーの架空世界", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=rtwJXauNisw");
            Song shounenBrave = new Song(Tags: "Kagerou Project", RomanizedAlbumTitle: "Mekakucity Records", AlbumTitle: "メカクシティレコーズ", RomanizedTitle: "Shounen Brave", Title: "少年ブレイヴ", RomanizedArtist: "Jin / Shizen no Teki-P", Artist: "じん / 自然の敵P", RomanizedSinger: "IA", Singer: "イア", Uri: "https://www.youtube.com/watch?v=glWTmQhCb4I");
            Song daze = new Song(Tags: "Kagerou Project", Title: "daze", Singer: "MARiA", Uri: "https://www.youtube.com/watch?v=9d2iLvb3mwQ");
            Song days = new Song(Tags: "Kagerou Project", Title: "days", Singer: "Lia", Uri: "https://www.youtube.com/watch?v=gBJvkhobGAs");
            Song red = new Song(Tags: "Kagerou Project", Title: "RED", Artist: "GOUACHE", Uri: "https://www.youtube.com/watch?v=p5l_9vUGhi4");

            songs.Add(lostTimePrologue);
            songs.Add(kaienPanzermast);
            songs.Add(summerEndRoll);
            songs.Add(cryingPrologue);
            songs.Add(jinzouEnemy);
            songs.Add(mekakushiCode);
            songs.Add(kagerouDaze);
            songs.Add(headphoneActors);
            songs.Add(kuusouForest);
            songs.Add(konohaStateOfTheWorld);
            songs.Add(kisaragiAttention);
            songs.Add(childrenRecord);
            songs.Add(yobanashiDeceive);
            songs.Add(lostTimeMemory);
            songs.Add(ayanoTheoryOfHappiness);
            songs.Add(otsukimiRecital);
            songs.Add(yuukeiYesterday);
            songs.Add(outerScience);
            songs.Add(summertimeRecord);
            songs.Add(shissouWord);
            songs.Add(additionalMemory);
            songs.Add(toumeiAnswer);
            songs.Add(eneCyberJourney);
            songs.Add(gunjouRain);
            songs.Add(shinigamiRecord);
            songs.Add(deadAndSeek);
            songs.Add(maryFictionalWorld);
            songs.Add(shounenBrave);
            songs.Add(daze);
            songs.Add(days);
            songs.Add(red);

            songConfig.UseRomanizedNames = true;
            songConfig.Songs = songs;
            
            SaveSongConfig();
        }
    }
}
