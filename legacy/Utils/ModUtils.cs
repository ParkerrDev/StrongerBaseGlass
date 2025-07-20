using QModManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongerBaseGlass.Utils
{

    public static class ModUtils
    {

        public static readonly object[] EmptyParams = new object[0];

        public static void LogDebug(string message, bool show = true)
        {
            Logger.Log(Logger.Level.Debug, message, null, show);
        }

        public static void LogInfo(string message, bool show = false)
        {
            Logger.Log(Logger.Level.Info, message, null, show);
        }

        public static void LogOnScreen(string message)
        {
            Logger.Log(Logger.Level.Debug, message, null, true);
        }

    }

}
