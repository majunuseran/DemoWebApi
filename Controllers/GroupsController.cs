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
    public class GroupsController : BaseController
    {
        public IEnumerable Get(int id)
        {
            return entities.Groups.Where(x => x.LynxSiteGUID == requestIdentity.SiteGuid && x.CourseID == id).OrderBy(x => x.GroupNum).Select(x=>new{x.GroupID,x.GroupNum});
        }
    }
}
