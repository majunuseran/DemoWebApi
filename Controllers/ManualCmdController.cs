using LynxMobileData;
using LynxWebApi.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LynxWebApi.Controllers
{
    [Authorize]
    public class ManualCmdController : ApiController
    {
        public int Post(Command cmd)
        {
            ManualCommand manualCmd = new ManualCommand(cmd, new RequestIdentity(User.Identity));
            return manualCmd.Queue();
        }
    }
}
