using LynxMobileData;
using LynxWebApi.Utility;
using NLog;
using System;
using System.Configuration;
using System.Net;
using System.Web.Http;

namespace LynxWebApi.Commands
{
    public class CommandBase
    {
        private static Logger cmdLog = Log.GetCmdLogger();
        private static Logger msgLog = Log.GetMsgLogger();
        public CommandBase(Command cmd, RequestIdentity requestIdentity)
        {
            cmd.LynxSiteGUID = requestIdentity.SiteGuid;
            cmd.Token = requestIdentity.Token;
            Command = cmd;
        }
        public virtual bool IsValid()
        {
            if (Command == null || String.IsNullOrWhiteSpace(Command.Ids) || Command.LynxSiteGUID <= 0 || String.IsNullOrWhiteSpace(Command.Token))
                return false;
            return true;
        }
        public Command Command { get; set; }
        public int Queue()
        {
            if (!IsValid())
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            try
            {
                DateTime startUtcTime = DateTime.UtcNow;
                // 1. build command handler and queue
                Command.QueuedTime = Utility.Helper.GetCurrentCentralTime();
                using (var entites = new LynxMainCentralEntities())
                {
                    entites.Commands.Add(Command);
                    entites.SaveChanges();
                    cmdLog.Debug(string.Format("Token {0} SiteGUID {1} add command {2} {3} {4} {5} {6}", Command.Token, Command.LynxSiteGUID, Command.CommandID, Command.CommandFamily, Command.CommandType, Command.Ids, Command.Value), Log.Level.Info);
                }

                // 2. check for processed
                int processStatus = GetProcessStatus();
                while (processStatus == 0)
                {
                    var appSettings = ConfigurationManager.AppSettings;
                    string CommandQueueTimeoutInSecsStr = appSettings["CommandQueueTimeoutInSecs"] ?? "10";
                    TimeSpan? diff = DateTime.UtcNow - startUtcTime;
                    if (diff == null || diff.Value.TotalSeconds >= Convert.ToInt32(CommandQueueTimeoutInSecsStr))
                        break;
                    System.Threading.Thread.Sleep(1000);
                    processStatus = GetProcessStatus();
                }
                if (processStatus == 0)
                {
                    using (var entites = new LynxMainCentralEntities())
                    {
                        var command = entites.Commands.Find(Command.CommandID);
                        if (command != null)
                        {
                            entites.Commands.Remove(command);
                            entites.SaveChanges();
                            cmdLog.Debug(string.Format("SiteGUID {0} remove command {1} {2} {3} {4} {5}", Command.LynxSiteGUID, Command.CommandID, Command.CommandFamily, Command.CommandType, Command.Ids, Command.Value), Log.Level.Info);
                            //cmdLog.Debug(string.Format("Remove command {0}", command.CommandID),Log.Level.Info);
                        }
                    }
                }
                return processStatus;
            }
            catch (Exception e)
            {
                if (!(e is HttpResponseException))
                {
                    msgLog.Debug("Errors occurred while queuing Command. " + Environment.NewLine + e.Message + " " + e.StackTrace, Log.Level.Error);
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
                else
                    throw;
            }
        }

        private int GetProcessStatus()
        {
            using (var entites = new LynxMainCentralEntities())
            {
                if (Command.CommandID > 0)
                {
                    var command = entites.Commands.Find(Command.CommandID);
                    if (command != null)
                        if (command.ProcessedTime != null)
                            return 1;
                        else
                            return 0;
                }
            }
            return -1;
        }
    }
    /// Command Family:
    /// 0 Global,
    /// 1 Manual,
    /// 2 Water Plan Adjust,
    /// 3 Diagnostic,
    /// 4 Download,
    /// 5 HHRI      
    public enum CommandFamily
    {
        Global,
        Manual,
        WaterPlanAdjust,
        Diagnostic,
        Download,
        HHRI,
        Decoder,
        AuditStation
    }
}