using LynxMobileData;

namespace LynxWebApi.Commands
{
    public class HhriCommand : CommandBase
    {
        public HhriCommand(Command cmd,RequestIdentity requestIdentity):base(cmd,requestIdentity)
        {
            Command.CommandFamily = (int)CommandFamily.HHRI;
            Command.Ids = Command.Ids.Replace("#", "");
            if (Command.CommandType == null)
                Command.CommandType = 0;
            if (Command.Value == null)
                Command.Value = 0;
        }
    }
}