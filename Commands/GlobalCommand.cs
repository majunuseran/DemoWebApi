using LynxMobileData;
using System;
using System.Linq;

namespace LynxWebApi.Commands
{
    public class GlobalCommand : CommandBase
    {
        public GlobalCommand(Command cmd,RequestIdentity requestIdentity):base(cmd,requestIdentity)
        {
            Command.CommandFamily = (int)CommandFamily.Global;
            if (Command.Value == null)
                Command.Value = 0;
        }
        public override bool IsValid()
        {
            if (!base.IsValid())
                return false;
            if (Command.CommandType == null || !((int[])Enum.GetValues(typeof(GlobalCommandType))).Contains(Command.CommandType.Value))
            {
                return false;
            }
            return true;
        }
    }
    // Global
    // 0 Stop All
    // 1 Rain Hold
    // 2 Remove Rain Hold
    public enum GlobalCommandType
    {
        StopAll,
        RainHold,
        RemoveRainHold
    }
}