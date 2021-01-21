using ChillPlaceLevels.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillPlaceLevels.Classes
{
    public enum ErrorLogStates
    {
        Solved,
        Critical,
        Unsolved
    }
    public class ErrorLog
    {
        public string Reason;
        public Guid Uid;
        public DateTime Saved;
        public ErrorLogStates State;
        public void Save()
        {
            LogHandle.SaveLog(this);
        }
    }
}
