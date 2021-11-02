using LynxMobileData;
using LynxWebApi.Commands;
using LynxWebApi.Utility;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace LynxWebApi.Controllers
{
    [Authorize]
    public class GlobalCmdController : ApiController
    {
        public int Post(Command cmd)
        {
            GlobalCommand globalCmd = new GlobalCommand(cmd, new RequestIdentity(User.Identity));
            return globalCmd.Queue();
        }
    }
}
