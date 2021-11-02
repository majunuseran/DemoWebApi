using LynxMobileData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;

namespace LynxWebApi.ODataControllers
{
    [Authorize]
    public class HolesController : EntitySetController<HOLE, int>
    {
        private LynxMainCentralEntities entities = new LynxMainCentralEntities();
        [Queryable]
        public override IQueryable<HOLE> Get()
        {
            RequestIdentity requestIdentity = new RequestIdentity(User.Identity);
            return entities.HOLEs.Where(x => x.LynxSiteGUID == requestIdentity.SiteGuid);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            entities.Dispose();
        }
    }
}
