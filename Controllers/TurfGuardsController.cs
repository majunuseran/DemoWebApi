using System.Linq;
using System.Collections;

namespace LynxWebApi.Controllers
{
    public class TurfGuardsController : BaseController
    {
        public IEnumerable Get()
        {
            var result = (from station in entities.STATIONs
                         where station.LynxSiteGUID == requestIdentity.SiteGuid
                         select station.tgSensor).Distinct();
            return result;

        }
    }
}
