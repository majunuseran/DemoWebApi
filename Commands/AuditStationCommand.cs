using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LynxMobileData;

namespace LynxWebApi.Commands
{
    public class AuditStationCommand : CommandBase
    {
        public AuditStationCommand(Command cmd, RequestIdentity requestIdentity) : base(cmd, requestIdentity)
        {
            Command.CommandFamily = (int)CommandFamily.AuditStation;
            if (Command.Value == null)
                Command.Value = 0;
        }
        public override bool IsValid()
        {
            if (!base.IsValid())
                return false;
            if (Command.CommandType == null || !((int[])Enum.GetValues(typeof(AuditStationCommandType))).Contains(Command.CommandType.Value))
            {
                return false;
            }
            return true;
        }

        // AuditStation
        // 0 Update
        public enum AuditStationCommandType
        {
            Update
        }
    }
}