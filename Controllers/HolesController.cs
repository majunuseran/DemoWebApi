using LynxMobileData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LynxWebApi.Controllers
{
    public class HolesController : BaseController
    {
        public IEnumerable Get(int id)
        {
            return from hole in entities.HOLEs.Where(x => x.LynxSiteGUID == requestIdentity.SiteGuid && x.CourseID == id)
                select new
                {
                    hole.HoleID,
                    hole.HoleNumber
                };
        }
    }
}
