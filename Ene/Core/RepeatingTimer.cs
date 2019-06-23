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
        private static Timer loopingSongActivityTimer;
        private static SocketTextChannel channel;
        private static Game game;

        private static string[] songNamesEnglish = { "Jinzou Enemy", "Mekakushi Code", "Kagerou Daze", "Headphone Actor",
            "Souzou Forest", "Konoha's State of the World", "Kisaragi Attention", "Children Record", "Yobanashi Deceive",
        "Lost Time Memory", "Ayano's Theory of Happiness", "Otsukimi Recital", "Yuukei Yesterday", "Outer Science",
            "Summertime Record", "Shissou Word", "Additional Memory", "Toumei Answer", "Ene's Cyber Journey", "Gunjou Rain",
        "Shinigami Record", "Dead and Seek", "Mary's Fictional World", "Shounen Brave", "Daze"};
        private static string[] songNamesJapanese = { "人造エネミー", "メカクシコード", "カゲロウデイズ", "ヘッドフォンアクター",
        "想像フォレスト", "コノハの世界事情", "如月アテンション", "チルドレンレコード", "夜咄ディセイブ", "ロスタイムメモリー",
            "アヤノの幸福理論", "オツキミリサイタル", "夕景イエスタデイ", "アウターサイエンス", "サマータイムレコード", "失想ワアド",
            "アディショナルメモリー", "透明アンサー", "エネの電脳紀行", "群青レイン", "シニガミレコード", "デッドアンドシーク",
            "マリーの架空世界", "少年ブレイヴ", "DAZE"};

        private static int songIndex;

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

        private static string getNameOfSong(int index, bool isNameInJapanese)
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

        private static int getSongIndex()
        {
            if (songIndex == songNamesEnglish.Length)
            {
                songIndex = 0;
            }
            return songIndex;
        }

        private static async void OnSongActivityTimerTicked(object sender, ElapsedEventArgs e)
        {
            string songName = getNameOfSong(songIndex, false);
            game = new Game(songName, ActivityType.Listening);
            songIndex++;
            await Global.Client.SetActivityAsync(game);
        }
    }
}
