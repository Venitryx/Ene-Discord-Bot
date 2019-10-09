using Discord.Commands;
using Ene.Core.Servers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ene.Preconditions
{
    public class RequireMusicChannel : PreconditionAttribute
    {
        public RequireMusicChannel()
        {

        }
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var serverResult = from s in Servers.servers
                               where
                               s.GuildID == context.Guild.Id
                               select s;
            var serverInfo = serverResult.FirstOrDefault();
            if (serverInfo == null) serverInfo = Servers.GetServerInfo(context.Guild.Id);

            if (context.Channel.Id != serverInfo.MusicTextChannelID)
            {
                return Task.FromResult(PreconditionResult.FromError($"Wrong channel."));
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
        }
    }
}
