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
        public int AdvisoryNumber { get; set; }
    }
}
