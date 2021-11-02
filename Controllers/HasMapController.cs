using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LynxWebApi.Controllers
{
    public class HasMapController : BaseController
    {
        public bool Get()
        {
            return entities.StationLocations.Any(l => l.LynxSiteGUID == requestIdentity.SiteGuid);
        }
    }
}
