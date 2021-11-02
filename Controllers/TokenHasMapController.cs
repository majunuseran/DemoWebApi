using LynxWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LynxWebApi.Controllers
{
    public class TokenHasMapController : TokenController
    {
        public override string Post(TokenRequest tokenRequest)
        {
            int siteGuid = 0;
            string token = getTokenByPost(tokenRequest, out siteGuid);
            if(siteGuid==0)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            bool hasMap= entities.StationLocations.Any(l => l.LynxSiteGUID == siteGuid);
            return token + " " + hasMap;
        }
    }
}      