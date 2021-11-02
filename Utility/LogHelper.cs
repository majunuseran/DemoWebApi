using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LynxWebApi.Utility
{
    public static class LogHelper
    {
        public static void AddEntry(this Logger logger, string msg, LynxWebApi.Utility.Log.Level lvl)
        {
            string fmtMsg = string.Format("{0}", msg);
            switch (lvl)
            {
                case LynxWebApi.Utility.Log.Level.Info:
                    logger.Info(fmtMsg);
                    break;
                case LynxWebApi.Utility.Log.Level.Error:
                    logger.Error(fmtMsg);
                    break;
                case LynxWebApi.Utility.Log.Level.Debug:
                    logger.Debug(fmtMsg);
                    break;
            }

        }
    }
}