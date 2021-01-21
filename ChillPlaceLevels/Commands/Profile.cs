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
    class Profile
    {
        [TCPCommand("Check your profile.", UserTrustLevels.User, "Profile", "Prf", "Level", "Lvl")]
        public static async Task Do(SocketMessage arg, UserState ustate)
        {
            try
            {
                foreach (var guild in ustate.GStates)
                    if (guild.GuildId == ((SocketGuildChannel)arg.Channel).Guild.Id)
                    {
                        float lvl = guild.XP / 1000;
                        lvl = lvl <= 0 ? 1 : lvl;
                        float percentLeftToNextLevel = guild.XP % 1000 / 10;
                        string bar = $"[";
                        for(int i = 0; i < 100; i += 10)
                            if (i + 9 < percentLeftToNextLevel)
                                bar += "x";
                            else
                                bar += "_";
                        bar += "]";
                        var emb = new EmbedBuilder()
                        {
                            Author = new EmbedAuthorBuilder()
                            {
                                Name = arg.Author.Username,
                                IconUrl = arg.Author.GetAvatarUrl()
                            },
                            Description = $"```\n- {arg.Author.Username} -\n\nlevel {(int)lvl} {bar} level {(int)(lvl + 1)}\n[{(int)percentLeftToNextLevel}%]\n```",
                            Color = new Color(0x36393F)
                        }.Build();
                        await arg.Channel.SendMessageAsync("", false, emb);
                        break;
                    }
            }
            catch(Exception e)
            {
                var uid = Guid.NewGuid();
                await arg.Channel.SendMessageAsync($"Sorry! Something went wrong.\nDM this ID to NOxygen#0001: \n`{uid}`");
                new ErrorLog()
                {
                    State = ErrorLogStates.Unsolved,
                    Uid = uid,
                    Reason = e.ToString(),
                }.Save();
            }
            
        }
    }
}
