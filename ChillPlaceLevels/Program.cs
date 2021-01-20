using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ChillPlaceLevels.Commands;
using ChillPlaceLevels.Classes;
using Discord;
using Discord.WebSocket;
using System.Threading;
using ChillPlaceLevels.Misc;

namespace ChillPlaceLevels
{
    class Program
    {
        private static string Tok = "NzkzNjgwMTA1NzQyNDAxNTQ2.X-vyMw.udbqrcfKJHsiL2v4r529kF4paFo";
        public static string Prefix = "-";
        public static DiscordSocketClient Client = new DiscordSocketClient(); 
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        public static async Task MainAsync()
        {
            Client.Connected += Client_Connected;
            Client.MessageReceived += Client_MessageReceived;
            await Client.LoginAsync(TokenType.Bot, Tok);
            await Client.StartAsync();
            await Task.Delay(-1);
        }

        private static Task Client_Connected()
        {
            Console.WriteLine("Connected");
            return Task.CompletedTask;
        }

        private static List<ulong> lastSent = new List<ulong>();
        private static async Task Client_MessageReceived(SocketMessage arg)
        {
            if(!arg.Author.IsBot)
            {
                var user = Database.GetUser(arg.Author);
                var guild = ((SocketGuildChannel)arg.Channel).Guild;
                if (!lastSent.Contains(arg.Author.Id))
                {
                    lastSent.Add(arg.Author.Id);
                    new Thread(_ =>
                    {
                        Thread.Sleep(30000);
                        lastSent.Remove(arg.Author.Id);
                    }).Start();
                    var tempList = user.GStates.ToList();
                    if (!tempList.Contains(new GuildState() { GuildId = guild.Id, XP = 0 }))
                        tempList.Add(new GuildState()
                        {
                            GuildId = guild.Id,
                            XP = new Random().Next(100, 300)
                        });
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (tempList[i].GuildId == guild.Id)
                            tempList[i].XP = new Random().Next(100, 300);
                    }
                    user.GStates = tempList.ToArray();
                    Database.SaveUserState(user);
                }
                if (arg.Content.ToLower().StartsWith(Prefix))
                {
                    string command = arg.Content.Split(Prefix.ToCharArray())[1].Split(' ')[0];
                    foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                        if (type.Namespace == "ChillPlaceLevels.Commands")
                            foreach (var method in type.GetMethods())
                                if (method != null)
                                {
                                    var attr = (TCPCommandAttribute)method.GetCustomAttribute(typeof(TCPCommandAttribute));
                                    if (attr != null && (int)attr.RequiredTrust >= (int)user.Trust)
                                        foreach (var str in attr.Aliases)
                                            if (str.ToLower() == command.ToLower())
                                            {
                                                var res = method.Invoke(null, new object[] { arg, user });
                                                await (Task)res;
                                                break;
                                            }
                                }
                }
            }
        }
    }
}


namespace ChillPlaceLevels.Commands
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TCPCommandAttribute : Attribute
    {
        public string[] Aliases;
        public string Description;
        public UserTrustLevels RequiredTrust;
        public TCPCommandAttribute(string Description, UserTrustLevels RequiredTrust, params string[] Aliases)
        {
            this.RequiredTrust = RequiredTrust;
            this.Description = Description;
            this.Aliases = Aliases;
        }
    }
}
