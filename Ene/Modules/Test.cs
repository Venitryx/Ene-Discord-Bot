using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;

namespace Ene.Modules
{
    public class Test : ModuleBase<SocketCommandContext>
    {
        [Command("hello.")]
        public async Task task()
        {
            await Context.Channel.SendMessageAsync("Hello.");

        }
    }
}
