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
using Discord.Net;
using System.IO;
using Newtonsoft.Json;

namespace ChillPlaceLevels
{
    class Program
    {
        public static HostingProperties HP;
        public static DiscordSocketClient Client = new DiscordSocketClient(); 
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        public static async Task MainAsync()
        {
            Client.Connected += Client_Connected;
            Client.MessageReceived += Client_MessageReceived;
            try
            {
                string hpLoc = @".\hostingproperties.json";
                if (File.Exists(hpLoc))
                    HP = JsonConvert.DeserializeObject<HostingProperties>(File.ReadAllText(hpLoc));
                else
                {
                    Console.WriteLine("Type token and then type prefix");
                    HP = new HostingProperties()
                    {
                        Token = Console.ReadLine(),
                        Prefix = Console.ReadLine(),
                    };
                    File.WriteAllText(hpLoc, JsonConvert.SerializeObject(HP));
                    Console.WriteLine($"{Path.GetFullPath(hpLoc)} created.");
                    await MainAsync();
                }
                await Client.LoginAsync(TokenType.Bot, HP.Token);
                await Client.StartAsync();
                await Task.Delay(-1);
            }
            catch
            {
                Console.WriteLine("Check auth token.");
            }
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
                var guser = (SocketGuildUser)arg.Author;
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
                    bool addNewG = true;
                    foreach(var aguild in tempList)
                    {
                        if(aguild.GuildId == guild.Id)
                        {
                            int toAdd = new Random().Next(50, 200);
                            int firstDecOfToAddPlusXP = int.Parse((toAdd + aguild.XP).ToString().First().ToString());
                            int firstDecOfXP = int.Parse(aguild.XP.ToString().First().ToString());
                            aguild.XP += toAdd;
                            if (firstDecOfToAddPlusXP != firstDecOfXP & aguild.XP > 999 & Database.GetGuildSettings(guild).PingUserOnLevelUp)
                                await arg.Channel.SendMessageAsync($"{arg.Author.Mention} you levelled up to level {(int)(aguild.XP / 1000)}!");
                            addNewG = false;
                            break;
                        }
                    }
                    if (addNewG)
                        tempList.Add(new GuildState()
                        {
                            GuildId = guild.Id,
                            XP = new Random().Next(50, 200)
                        });
                    user.GStates = tempList.ToArray();
                    Database.SaveUserState(user);
                }
                if (arg.Content.ToLower().StartsWith(HP.Prefix))
                {
                    string command = arg.Content.Split(HP.Prefix.ToCharArray())[1].Split(' ')[0];
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
