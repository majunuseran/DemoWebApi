using LynxMobileData;
using System.Collections.Generic;
using System.Linq;

namespace LynxWebApi.Controllers
{
    public class SatellitesController : BaseController
    {
        public IEnumerable<SATELLITE> Get(int id)
        {
            return entities.SATELLITEs.Where(x => x.LynxSiteGUID == requestIdentity.SiteGuid && x.GroupID == id).OrderBy(x => x.SatelliteNum);
        }
    }
}
