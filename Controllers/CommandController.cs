using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LynxMobileData;

namespace LynxWebApi.Controllers
{
    [Authorize]
    public class CommandController : ApiController
    {
        /// <summary>
        /// Example - HHRI command&lt; T &gt;
        /// data : {
		///	commandFamily : 5,
		///	commandType : cmdType,
		///	ids : cmd,
		///	value : 0
		/// };
        ///
        /// Command Family:
        /// 0 Global,
        /// 1 Manual,
        /// 2 Water Plan Adjust,
        /// 3 Diagnostic,
        /// 4 Download,
        /// 5 HHRI,
        /// -1 Unknown
        /// </summary>
        /// <param name="command"></param>
        /// <returns>1: Processed; 0: Not Processed; -1: Error</returns>
        public short QueueCommand([FromBody] Command command)
        {           

            // Global
            // 0 Stop All
            // 1 Rain Hold
            // 2 Remove Rain Hold

            // Manual
            // 0 Run Seconds
            // 1 Stop
            // 2 Hold Days
            // 3 Resume
            // 4 Percent Adjust
            // 5 Force Site Update
            // 6 Force Status Update
            short processStatus = 0;

            int siteGuid = 0;
            int.TryParse(User.Identity.Name, out siteGuid);
            if (siteGuid > 0)
            {
                // 1. build command handler and queue
                Command cmd = new Command();
                cmd.LynxSiteGUID = siteGuid;
                cmd.CommandFamily = command.CommandFamily;
                cmd.CommandType = command.CommandType;
                cmd.Ids = command.Ids;
                cmd.Value = command.Value;
                cmd.QueuedTime = DateTime.UtcNow;
                using (var entites = new LynxMainCentralEntities())
                {
                    entites.Commands.Add(cmd);
                    entites.SaveChanges();
                }
                
                // 2. check for processed
                processStatus = GetProcessStatus(cmd);
                while (processStatus==0)
                {
                    TimeSpan? diff = DateTime.UtcNow - cmd.QueuedTime;
                    if (diff == null || diff.Value.TotalSeconds > 5)
                        break;
                    System.Threading.Thread.Sleep(1000);
                    processStatus = GetProcessStatus(cmd);
                }                
            }
            return processStatus;
        }

        private short GetProcessStatus(Command cmd)
        {
            using (var entites = new LynxMainCentralEntities())
            {
                if (cmd.CommandID > 0)
                {
                    var command = entites.Commands.Find(cmd.CommandID);
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
}
