using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using ChillPlaceLevels.Classes;

namespace ChillPlaceLevels.Misc
{
    class Database
    {
        public static string Dir = @".\UserStates";
        private static DirectoryInfo _ = !Directory.Exists(Dir) ? Directory.CreateDirectory(Dir) : null;
        public static void SaveUserState(UserState User)
        {
            string saveTo = Path.Combine(Dir, User.DiscordUser.Id.ToString() + ".json");
            User.DiscordUser = null;
            File.WriteAllText(saveTo, JsonConvert.SerializeObject(User));
        }
        public static UserState GetUser(SocketUser User)
        {
            string saveTo = Path.Combine(Dir, User.Id.ToString() + ".json");
            if (File.Exists(saveTo))
            {
                var user = JsonConvert.DeserializeObject<UserState>(File.ReadAllText(saveTo));
                user.DiscordUser = User;
                return user;
            }
            else
            {
                return new UserState()
                {
                    DiscordUser = User,
                    Trust = UserTrustLevels.User,
                    GStates = new GuildState[] { }
                };
            }
        }
        public static string GDir = @".\GuildSettings";
        private static DirectoryInfo __ = !Directory.Exists(GDir) ? Directory.CreateDirectory(GDir) : null;
        public static void SaveGuildSettings(GuildSettings GS)
        {
            string saveTo = Path.Combine(GDir, GS.Guild.Id.ToString() + ".json");
            GS.Guild = null;
            File.WriteAllText(saveTo, JsonConvert.SerializeObject(GS));
        }
        public static GuildSettings GetGuildSettings(SocketGuild Guild)
        {
            string saveTo = Path.Combine(GDir, Guild.Id.ToString() + ".json");
            if (File.Exists(saveTo))
            {
                var user = JsonConvert.DeserializeObject<GuildSettings>(File.ReadAllText(saveTo));
                user.Guild = Guild;
                return user;
            }
            else
            {
                return new GuildSettings()
                {
                    Guild = Guild
                };
            }
        }
    }
}
