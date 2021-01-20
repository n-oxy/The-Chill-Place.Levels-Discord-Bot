using ChillPlaceLevels.Classes;
using ChillPlaceLevels.Misc;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillPlaceLevels.Commands
{
    class Level
    {
        [TCPCommand("Check your level.", Classes.UserTrustLevels.User, "level", "lvl")]
        public static async Task Do(SocketMessage arg, UserState ustate)
        {
            foreach(var guild in ustate.GStates)
                if(guild.GuildId == ((SocketGuildChannel)arg.Channel).Guild.Id)
                {
                    await arg.Channel.SendMessageAsync($"{guild.XP}");
                    break;
                }
        }
    }
}
