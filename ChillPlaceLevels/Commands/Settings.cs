using ChillPlaceLevels.Classes;
using ChillPlaceLevels.Misc;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChillPlaceLevels.Commands
{
    class Settings
    {
        [TCPCommand("Change guild settings.", UserTrustLevels.User, "Settings", "Config", "Configuration")]
        public static async Task Do(SocketMessage arg, UserState ustate)
        {
            try
            {
                if (((SocketGuildUser)arg.Author).GuildPermissions.ManageGuild)
                {
                    var GS = Database.GetGuildSettings(((SocketGuildUser)arg.Author).Guild);
                    if (arg.Content.Contains(' '))
                    {
                        foreach (var fi in GS.GetType().GetFields())
                        {
                            var attr = (GSAttribute)fi.GetCustomAttribute(typeof(GSAttribute));
                            foreach (var str in attr.Aliases)
                            {
                                if(str.ToLower() == arg.Content.Split(' ')[1].ToLower())
                                {
                                    bool setTo = arg.Content.Split(' ').Length > 2 ? bool.Parse(arg.Content.Split(' ')[2]) : !(bool)fi.GetValue(GS);
                                    fi.SetValue(GS, setTo);
                                    Database.SaveGuildSettings(GS);
                                    await arg.Channel.SendMessageAsync($"Set {fi.Name} to {setTo}");
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        string cmds = "";
                        foreach (var fi in GS.GetType().GetFields())
                            if (fi.IsPublic)
                            {
                                var attr = (GSAttribute)fi.GetCustomAttribute(typeof(GSAttribute));
                                cmds += $"{attr.Description}\n`{Program.HP.Prefix}settings {fi.Name} <true/false>`\n\n";
                            }
                        await arg.Channel.SendMessageAsync($"Usage:\n\n{cmds}");
                    }
                }
                else
                    await arg.Channel.SendMessageAsync("You don't have the required permission to do this.");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
