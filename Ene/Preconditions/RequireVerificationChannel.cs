using Discord.Commands;
using Ene.Core.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ene.Preconditions
{
    class RequireVerificationChannel : PreconditionAttribute
    {
        public RequireVerificationChannel()
        {

        }
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var serverResult = from s in Servers.servers
                               where s.GuildID == context.Guild.Id
                               select s;
            var serverInfo = serverResult.FirstOrDefault();
            if (serverInfo == null) serverInfo = Servers.GetServerInfo(context.Guild.Id);

            if (context.Channel.Id != serverInfo.VerificationChannelID)
            {
                return Task.FromResult(PreconditionResult.FromError($"Sorry, but you can't use that command in this channel!"));
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
        }
    }
}
