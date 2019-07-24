using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.WebSocket;

namespace Ene.SystemLang
{
    internal static class StringManipulation
    {
        internal static string SubstitutePronouns(string previousString) //word by word substitution
        {
            string[] words = previousString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Equals("I"))
                    words[i] = words[i].Replace("I", "you");
                else if (words[i].Equals("me"))
                    words[i] = words[i].Replace("me", "you");
                else if (words[i].Equals("my"))
                    words[i] = words[i].Replace("my", "your");
                else if (words[i].Equals("our"))
                    words[i] = words[i].Replace("our", "your");
                else if (words[i].Equals("mine"))
                    words[i] = words[i].Replace("mine", "yours");
                else if (words[i].Equals("ours"))
                    words[i] = words[i].Replace("ours", "yours");
            }
            return string.Join(" ", words);
        }

        internal static string SubstituteVerbs(string previousString) //word by word substitution
        {
            string[] words = previousString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Equals("am"))
                    words[i] = words[i].Replace("am", "are");
                else if (words[i].Equals("are"))
                    words[i] = words[i].Replace("are", "am");
            }
            return string.Join(" ", words);
        }

        internal static string StripPunctuation(string StringToReplace)
        {
            string newString = new string(StringToReplace.Where(c => !char.IsPunctuation(c)).ToArray());
            return newString;
        }

        internal static string StripSymbols(string StringToReplace)
        {
            string newString = new string(StringToReplace.Where(c => !char.IsSymbol(c)).ToArray());
            return newString;
        }

        internal static string RemoveFanboys(string previousString)
        {
            string[] words = previousString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words[0].Equals("for") || words[0].Equals("and") || words[0].Equals("nor") || words[0].Equals("but")
                || words[0].Equals("or") || words[0].Equals("yet") || words[0].Equals("so"))
            {
                words[0] = "";
            }
            return string.Join(" ", words);
        }

        private static bool isBotOwner(SocketUser user)
        {
            if (user.Id.Equals(229360837318410241)) return true;
            else return false;
        }
        internal static string AddMasterSuffix(string previousString)
        {
            if (isBotOwner(Global.context.User))
            {
                string lastChar = previousString.Substring(previousString.Length - 1);
                string newString = previousString.Substring(0, previousString.Length - 1);
                newString += (", master" + lastChar);
                return newString;
            }
            else return previousString;
        }
        internal static int milisecondsToDelayPerCharacter(string previousString)
        {
            return previousString.Length * 12;
        }
    }
}
