using ChillPlaceLevels.Classes;
using ChillPlaceLevels.Misc;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillPlaceLevels.Commands
{
    class Shop
    {
        [TCPCommand("Buy stuff.", UserTrustLevels.User, "Shop", "Store", "Shopp")]
        public static async Task Do(SocketMessage arg, UserState ustate)
        {
            Console.WriteLine("A");
        }
    }
}
