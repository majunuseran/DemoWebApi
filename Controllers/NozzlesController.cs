using System.Linq;
using System.Collections;

namespace LynxWebApi.Controllers
{
    public class NozzlesController : BaseController
    {
        public IEnumerable Get(int sprinklerid)
        {
            var result = (from station in entities.STATIONs
                          join nozzle in entities.Nozzles
                          on station.NozzleID equals nozzle.NozzleID
                          join sprinkler in entities.Sprinklers
                          on nozzle.SprinklerID equals sprinkler.SprinklerID
                          where (station.LynxSiteGUID == requestIdentity.SiteGuid)
                          select new
                          {
                              NozzleId = nozzle.NozzleID,
                              NozzleNumber = nozzle.Nozzle_Nbr,
                              LowPressure = nozzle.Pres_Low,
                              HighPressure = nozzle.Pres_Hi
                          }).Distinct();
            return result;

        }
    }
}
