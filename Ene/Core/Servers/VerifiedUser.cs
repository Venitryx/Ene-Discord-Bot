using System;
using System.Collections.Generic;
using System.Text;

namespace Ene.Core.Servers
{
    public class VerifiedUser
    {
        public ulong ID { get; set; }
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Grade { get; set; }
        public string DateVerified { get; set; }

        public int Credits { get; set; }
        public int Level { get; set; }
    }
}
