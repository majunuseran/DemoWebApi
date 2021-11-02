using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace LynxWebApi.Utility
{
    public static class Log
    {
        private static Logger msgLogger;
        private static Logger cmdLogger;
        private static Logger hhriAndroidReportLogger;
        private static Logger reportLogger;
        public static Logger GetMsgLogger()
        {
            if (msgLogger == null)
            {
                msgLogger = LogManager.GetLogger("MsgLogger"); 
            }
            return msgLogger;
        }
        public static Logger GetCmdLogger()
        {
            if (cmdLogger == null)
            {
                cmdLogger = LogManager.GetLogger("CmdLogger");
            }
            return cmdLogger;
        }
        public static Logger GetReportLogger()
        {
            if (reportLogger == null)
            {
                reportLogger = LogManager.GetLogger("ReportLogger");
            }
            return reportLogger;
        }
        public static Logger GetHhriAndroidReportLogger()
        {
            if (hhriAndroidReportLogger == null)
            {
                hhriAndroidReportLogger = LogManager.GetLogger("HhriAndroidReportLogger");
            }
            return hhriAndroidReportLogger;
        }
        public enum Level
        {
            Info,
            Error,
            Debug,
        }
        
    }
}