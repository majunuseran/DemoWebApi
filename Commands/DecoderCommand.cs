using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LynxMobileData;

namespace LynxWebApi.Commands
{
    public class DecoderCommand: CommandBase
    {
        public DecoderCommand(Command command, RequestIdentity requestIdentity)
            : base(command, requestIdentity)
        {
            Command.CommandFamily = (int) CommandFamily.Decoder;
            if (Command.CommandType == null)
                Command.CommandType = 0;
            if (Command.Value == null)
                Command.Value = 0;
        }

        public override bool IsValid()
        {
            if (!base.IsValid())
                return false;
            if (Command.CommandType == null ||
                !((int[])Enum.GetValues(typeof(DecoderCommandType))).Contains(Command.CommandType.Value))
            {
                return false;
            }
            return true;
        }

        public enum DecoderCommandType
        {
            Replace,
            Send,
            Test
        }
    }
}