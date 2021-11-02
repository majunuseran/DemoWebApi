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
    public class AreasController : BaseController
    {
        public IEnumerable Get(int id)
        {
            var areas = from holeArea in entities.HoleAreas
                        join area in entities.AreaTypes
                        on holeArea.AreaTypeID equals area.AreaTypeID
                        where (holeArea.LynxSiteGUID == requestIdentity.SiteGuid && area.LynxSiteGUID==requestIdentity.SiteGuid && holeArea.HoleID == id)
                        select new
                        {
                            area.Name,
                            holeArea.HoleAreaID,
                            area.AreaNum
                        };
            return areas;
        }
    }

}
