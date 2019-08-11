using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static Ene.SystemLang.MiscCommands.LikeCommands.DefaultLikeMessages;

namespace Ene.SystemLang.MiscCommands.LikeCommands
{
    public static class Likes
    {
        
        private static List<Like> likes;
        private static DefaultLikeMessagesData defaultLikeMessagesData;

        private static string likesFile = "Resources/SystemLang/MiscCommands/LikeCommands/Likes.json";
        private static string defaultLikeMessagesFile = "Resources/SystemLang/MiscCommands/LikeCommands/DefaultLikeMessages.json";
        static Likes()
        {
            Initialize();
        }

        public static void SaveObjects()
        {
            LikesStorage.SaveLikes(likes, likesFile);
        }

        public static void LoadObjects()
        {
            likes = LikesStorage.LoadLikes(likesFile).ToList();
        }

        public static void SaveDefaultLikeMessages()
        {
            LikesStorage.SaveDefaultLikeMessages(defaultLikeMessagesData, defaultLikeMessagesFile);
        }

        public static void LoadDefaultLikeMessages()
        {
            defaultLikeMessagesData = LikesStorage.LoadDefaultLikeMessages(defaultLikeMessagesFile);
        }

        public static Like GetLikedObject(string objectName, double likability = 5.0, bool useSpecialMessage = false, string message = null)
        {
            return GetOrCreateLikedObject(objectName, likability, useSpecialMessage, message);

        }

        private static Like GetOrCreateLikedObject(string ObjectName, double likability = 5.0, bool useSpecialMessage = false, string message = null)
        {
            var result = from l in likes
                         where l.Name == ObjectName
                         select l;

            var like = result.FirstOrDefault();
            if (like == null) like = CreateLikedObject(ObjectName, likability, useSpecialMessage, message);
            return like;
        }
        private static Like CreateLikedObject(string name, double likability = 5.0, bool useSpecialMessage = false, string message = null)
        {
            var newLike = new Like()
            {
                Name = name,
                Likability = likability,
                UseSpecialMessage = useSpecialMessage,
                Message = message
            };

            likes.Add(newLike);
            SaveObjects();
            return newLike;
        }

        public static void InitializeLikes()
        {
            likes = new List<Like>();
            var anime = GetLikedObject("anime", likability: 6.9, useSpecialMessage: true, message: "Depends on the anime, if you know what I mean.\n\nAnd by that, I mean Kagerou Project of course! What else did you think I was gonna say? You're so naughty!");
            var song = GetLikedObject("copy", likability: 10.0, useSpecialMessage: true, message: "and paste the above to insert more messages.");
        }

        public static void InitializeDefaultLikeMessages()
        {
            defaultLikeMessagesData = new DefaultLikeMessagesData()
            {
                lessThanZero = new string[] { "Ew, no. Definitely not.", "...|No. Please don't remind me.", "Oh hell no. Please don't trigger any more dark memories." },
                greaterThanTen = new string[] { "Ah. I see you're a man of culture as well.", "Ah yes. This is perfection." },
                zeroToOne = new string[] { "Ew, no." },
                oneToTwo = new string[] { },
                twoToThree = new string[] { },
                threeToFour = new string[] { },
                fourToFive = new string[] { },
                fiveToSix = new string[] { },
                sixToSeven = new string[] { },
                sevenToEight = new string[] { },
                eightToNine = new string[] { },
                nineToTen = new string[] { }
            };
            SaveDefaultLikeMessages();
        }

        public static void Reset()
        {
            InitializeLikes();
            InitializeDefaultLikeMessages();
        }
        public static void Initialize()
        {
            if (LikesStorage.SaveExists(likesFile))
            {
                LoadObjects();
            }
            else
            {
                InitializeLikes();
            }

            if (LikesStorage.SaveExists(defaultLikeMessagesFile))
            {
                LoadDefaultLikeMessages();
            }
            else
            {
                InitializeDefaultLikeMessages();
            }
        }

        public static string GetMessage(string ObjectName)
        {
            ObjectName = ObjectName.ToLower();
            ObjectName = StringManipulation.StripPunctuation(ObjectName);
            ObjectName = StringManipulation.StripSymbols(ObjectName);

            var result = from l in likes
                         where l.Name == ObjectName
                         select l;

            var like = result.FirstOrDefault();
            if (like == null) return "Sorry, but I have no clue what that is!";
            else if (!like.UseSpecialMessage) return PickRandomLikabilityMessage(GetDefaultLikabilityMessages(ObjectName));
            else return like.Message;
        }

        public static double GetLikability(string ObjectName)
        {
            ObjectName = ObjectName.ToLower();
            ObjectName = StringManipulation.StripPunctuation(ObjectName);
            ObjectName = StringManipulation.StripSymbols(ObjectName);

            var result = from l in likes
                         where l.Name == ObjectName
                         select l;

            var like = result.FirstOrDefault();
            return like.Likability;
        }

        private static string PickRandomLikabilityMessage(string[] messages)
        {
            Random r = new Random();
            int index = r.Next(0, messages.Length);
            return messages[index];
        }

        private static string[] GetDefaultLikabilityMessages(string ObjectName)
        {
            int likability = (int)Math.Floor(GetLikability(ObjectName));

            switch (likability)
            {
                case 0:
                    return defaultLikeMessagesData.zeroToOne;
                case 1:
                    return defaultLikeMessagesData.oneToTwo;
                case 2:
                    return defaultLikeMessagesData.twoToThree;
                case 3:
                    return defaultLikeMessagesData.threeToFour;
                case 4:
                    return defaultLikeMessagesData.fourToFive;
                case 5:
                    return defaultLikeMessagesData.fiveToSix;
                case 6:
                    return defaultLikeMessagesData.sixToSeven;
                case 7:
                    return defaultLikeMessagesData.sevenToEight;
                case 8:
                    return defaultLikeMessagesData.eightToNine;
                case 9:
                    return defaultLikeMessagesData.nineToTen;
                default:
                    return LikabilityOutOfRange(ObjectName);
            }
        }

        private static string[] LikabilityOutOfRange(string ObjectName)
        {
            int likability = (int)Math.Floor(GetLikability(ObjectName));

            if (likability < 0)
            {
                return defaultLikeMessagesData.lessThanZero;
            }
            else
            {
                return defaultLikeMessagesData.greaterThanTen;
            }
        }
    }
}
