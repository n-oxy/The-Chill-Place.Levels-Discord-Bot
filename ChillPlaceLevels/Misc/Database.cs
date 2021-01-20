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
        private static DirectoryInfo _ = !Directory.Exists(@".\UserStates") ? Directory.CreateDirectory(@".\UserStates") : null;
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
    }
}
