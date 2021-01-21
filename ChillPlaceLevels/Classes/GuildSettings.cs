using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillPlaceLevels.Classes
{
    public class GuildSettings
    {
        internal SocketGuild Guild;

        [GS("Ping user when they level up.", "LevelPing", "lvlPing", "PingUserLevel")]
        public bool PingUserOnLevelUp = true;
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class GSAttribute : Attribute
    {
        public string[] Aliases;
        public string Description;
        public GSAttribute(string Description, params string[] Aliases)
        {
            this.Description = Description;
            this.Aliases = Aliases;
        }
    }
}
