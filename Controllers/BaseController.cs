using LynxMobileData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LynxWebApi.Controllers
{
    [Authorize]
    public class BaseController : ApiController
    {
         protected LynxMainCentralEntities entities = new LynxMainCentralEntities();
         protected RequestIdentity requestIdentity;
        public BaseController()
        {
            requestIdentity = new RequestIdentity(User.Identity);
        }
         protected override void Dispose(bool disposing)
         {
             base.Dispose(disposing);
             entities.Dispose();
         }
    }
}
