using System;
using System.Collections.Generic;
using System.Text;

namespace Ene.Core.UserAccounts
{
    public class UserAccount
    {
        public ulong ID { get; set; }
        public uint Points { get; set; }
        public uint XP { get; set; }

        public uint Level
        {
            get
            {
                //XP = LVL ^ 2 * 50
                //xp / 50 = lvl^2
                //sqrt(XP/50) = lvl
                return (uint)Math.Sqrt(XP / 50);
            }
        }
    }
}
