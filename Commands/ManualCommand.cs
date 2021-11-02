using LynxMobileData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LynxWebApi.Commands
{
    public class ManualCommand:CommandBase
    {
        public ManualCommand(Command cmd,RequestIdentity requestIdentity):base(cmd,requestIdentity)
        {
            Command.CommandFamily = (int)CommandFamily.Manual;
        }
        public override bool IsValid()
        {
            if (!base.IsValid())
                return false;
            if (Command.CommandType == null || !((int[])Enum.GetValues(typeof(ManulCommandType))).Contains(Command.CommandType.Value))
            {
                return false;
            }
            return true;
        }
    }
    // Manual
    // 0 Run Seconds
    // 1 Stop
    // 2 Hold Days
    // 3 Resume
    // 4 Percent Adjust
    // 5 Force Site Update
    // 6 Force Status Update
    public enum ManulCommandType
    {
        Run,
        Stop,
        Hold,
        Resume,
        PercentAdjust,
        ForceSiteUpdate,
        ForceStatusUpdate
    }
}