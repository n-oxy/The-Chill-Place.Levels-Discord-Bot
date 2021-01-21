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
    class LogHandle
    {
        public static string Dir = @".\Logs";
        private static DirectoryInfo _ = !Directory.Exists(Dir) ? Directory.CreateDirectory(Dir) : null;
        public static void SaveLog(ErrorLog Log)
        {
            string saveTo = Path.Combine(Dir, Log.Uid + ".json");
            Log.Saved = DateTime.Now;
            File.WriteAllText(saveTo, JsonConvert.SerializeObject(Log));
        }
        public static ErrorLog GetLog(string Guid)
        {
            string saveTo = Path.Combine(Dir, Guid + ".json");
            if (File.Exists(saveTo))
            {
                var log = JsonConvert.DeserializeObject<ErrorLog>(File.ReadAllText(saveTo));
                return log;
            }
            else
            {
                return null;
            }
        }
    }
}
