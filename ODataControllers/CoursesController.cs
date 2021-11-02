using LynxMobileData;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;

namespace LynxWebApi.ODataControllers
{
    [Authorize]
    public class CoursesController : EntitySetController<GolfCourse, int>
    {
        private LynxMainCentralEntities entities = new LynxMainCentralEntities();
        [Queryable]
        public override IQueryable<GolfCourse> Get()
        {
            RequestIdentity requestIdentity = new RequestIdentity(User.Identity);
            return entities.GolfCourses.Where(x => x.LynxSiteGUID == requestIdentity.SiteGuid);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            entities.Dispose();
        }
    }
}
