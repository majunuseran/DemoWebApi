using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LynxWebApi.Controllers
{
    [Authorize]
    public class SiteGuidController : ApiController
    {
        public int Get()
        {
            RequestIdentity requestIdentity = new RequestIdentity(User.Identity);
            return requestIdentity.SiteGuid;
        }
    }
}
