using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

using Discord;
using Discord.WebSocket;

namespace Ene.Core
{
    internal static class RepeatingTimer
    {
        internal static Timer loopingSongActivityTimer, loopingAfkTimer;
        private static SocketTextChannel channel;
        private static Game game;

        internal static string[] songNamesEnglish = { "Jinzou Enemy", "Mekakushi Code", "Kagerou Daze", "Headphone Actor",
            "Souzou Forest", "Konoha's State of the World", "Kisaragi Attention", "Children Record", "Yobanashi Deceive",
        "Lost Time Memory", "Ayano's Theory of Happiness", "Otsukimi Recital", "Yuukei Yesterday", "Outer Science",
            "Summertime Record", "Shissou Word", "Additional Memory", "Toumei Answer", "Ene's Cyber Journey", "Gunjou Rain",
        "Shinigami Record", "Dead and Seek", "Mary's Fictional World", "Shounen Brave", "Daze"};
        internal static string[] songNamesJapanese = { "人造エネミー", "メカクシコード", "カゲロウデイズ", "ヘッドフォンアクター",
        "想像フォレスト", "コノハの世界事情", "如月アテンション", "チルドレンレコード", "夜咄ディセイブ", "ロスタイムメモリー",
            "アヤノの幸福理論", "オツキミリサイタル", "夕景イエスタデイ", "アウターサイエンス", "サマータイムレコード", "失想ワアド",
            "アディショナルメモリー", "透明アンサー", "エネの電脳紀行", "群青レイン", "シニガミレコード", "デッドアンドシーク",
            "マリーの架空世界", "少年ブレイヴ", "DAZE"};

        internal static int songIndex;
        internal static int songCount;

        internal static Task StartSongActivityTimer()
        {
            channel = Global.Client.GetGuild(446409245571678208).GetTextChannel(449695976714928140);
            loopingSongActivityTimer = new Timer()
            {
                Interval = 3.5 * 60 * 1000,
                AutoReset = true,
                Enabled = true
            };
            loopingSongActivityTimer.Elapsed += OnSongActivityTimerTicked;

            return Task.CompletedTask;
        }

        internal static Task StartAfkTimer()
        {
            loopingAfkTimer = new Timer()
            {
                Interval = 10 * 60 * 1000,
                AutoReset = false,
                Enabled = true
            };
            loopingAfkTimer.Elapsed += OnAfkTimerTicked;

            return Task.CompletedTask;
        }

        internal static string getNameOfSong(int index, bool isNameInJapanese)
        {
            string songName;
            index = getSongIndex();

            if (isNameInJapanese)
            {
                songName = songNamesJapanese[index];
            }
            else
            {
                songName = songNamesEnglish[index];
            }
            return songName;
        }

        internal static int getSongIndex()
        {
            if (songIndex == songNamesEnglish.Length)
            {
                songIndex = 0;
            }
            return songIndex;
        }

        internal static bool isSongCountEqual()
        {
            if (songNamesEnglish.Length == songNamesJapanese.Length)
            {
                songCount = (songNamesEnglish.Length + songNamesJapanese.Length) / 2;
                return true;
            }
            else
            {
                Console.WriteLine("Song lengths are not equal!");
                return false;
            }
        }

        internal static Game pickRandomSongDisplay()
        {
            if (RepeatingTimer.isSongCountEqual())
            {
                Random r = new Random();
                RepeatingTimer.songIndex = r.Next(0, RepeatingTimer.songCount);
            }
            return new Game(RepeatingTimer.getNameOfSong(RepeatingTimer.songIndex, false), ActivityType.Listening);
        }

        private static async void OnSongActivityTimerTicked(object sender, ElapsedEventArgs e)
        {
            string songName = getNameOfSong(songIndex, true);
            game = new Game(songName, ActivityType.Listening);
            songIndex++;
            await Global.Client.SetActivityAsync(game);
        }

        private static async void OnAfkTimerTicked(object sender, ElapsedEventArgs e)
        {
            await Global.Client.SetStatusAsync(UserStatus.AFK);
        }
    }
}
