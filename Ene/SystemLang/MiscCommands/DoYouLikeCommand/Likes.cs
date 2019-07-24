using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Ene.SystemLang.MiscCommands.DoYouLikeCommand
{
    public static class Likes
    {
        
        private static List<Like> likes;

        private static string likesFile = "Resources/SystemLang/MiscCommands/DoYouLikeCommand/Likes.json";
        static Likes()
        {
            if (LikesStorage.SaveExists(likesFile))
            {
                likes = LikesStorage.LoadLikes(likesFile).ToList();
            }
            else
            {
                Initialize();
            }
        }

        public static void SaveObjects()
        {
            LikesStorage.SaveLikes(likes, likesFile);
        }

        public static Like GetLikedObject(string objectName, double likability = 5.0, bool useSpecialMessage = false, string message = null)
        {
            return GetOrCreateLikedObject(objectName, likability, useSpecialMessage, message);

        }

        public static void Initialize()
        {
            likes = new List<Like>();
            var anime = GetLikedObject("anime", likability: 6.9, useSpecialMessage: true, message: "Depends on the anime, if you know what I mean.\n\nAnd by that, I mean Kagerou Project of course! What else did you think I was gonna say?");
            var song = GetLikedObject("copy", likability: 10.0, useSpecialMessage: true, message: "and paste the above to insert more messages.");

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
            else if (!like.UseSpecialMessage) return "I am using a placeholder message here.";
            else return like.Message;
        }

        public static string GetLikability(string ObjectName)
        {
            ObjectName = ObjectName.ToLower();
            ObjectName = StringManipulation.StripPunctuation(ObjectName);
            ObjectName = StringManipulation.StripSymbols(ObjectName);

            var result = from l in likes
                         where l.Name == ObjectName
                         select l;

            var like = result.FirstOrDefault();
            if (like == null) return "Sorry, but I have no clue what that is!";
            else if (!like.UseSpecialMessage) return "I am using a placeholder message here.";
            else return String.Format("On a scale of 1-10: {0}", like.Likability);
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
        
    }
}
