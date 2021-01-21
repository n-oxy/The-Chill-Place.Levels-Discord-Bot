using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace ChillPlaceLevels.Classes
{
    public enum UserTrustLevels
    {
        User,
        Trusted,
        MaxTrusted,
        Developer,
    }
    public class UserState
    {
        public SocketUser DiscordUser;
        public GuildState[] GStates;
        public UserTrustLevels Trust;
    }
    public class GuildState
    {
        public ulong GuildId;
        public float XP;
    }
}
