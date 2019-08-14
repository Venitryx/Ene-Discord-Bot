using System;
using System.Collections.Generic;
using System.Text;

namespace Ene.SystemLang.MiscCommands.ShouldICommand
{
    public class ShouldI
    {
        public ulong ID { get; set; }
        public string Command { get; set; }
        public string Reply { get; set; }
        public DateTime TimeOfCommand { get; set; }
        public int TimesRun { get; set; }
    }
}
