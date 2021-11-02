using LynxWebApi.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LynxMobileData;


namespace LynxWebApi.Controllers
{
    public class FindDecoderController : BaseController//ApiController
    {
        
        //protected LynxMainCentralEntities entities = new LynxMainCentralEntities();
        public object Get(int id)
        {
            //var addr = address;
            //var decoder = new Decoder(addr, 1, 1);

            //decoder.Offsets.AddRange(new List<DecoderStation>()
            //{
            //    new DecoderStation(0,0),
            //    new DecoderStation(0,0),
            //    new DecoderStation(0,0),
            //    new DecoderStation(0,0)
            //}
            //);
            var result = from station in entities.STATIONs
                         join satellite in entities.SATELLITEs
                         on station.SatelliteID equals satellite.SatelliteID
                         join grp in entities.Groups
                         on satellite.GroupID equals grp.GroupID
                         where station.DECODERADDRESS == id &&
                         station.LynxSiteGUID == requestIdentity.SiteGuid &&
                         satellite.LynxSiteGUID == requestIdentity.SiteGuid &&
                         grp.LynxSiteGUID == requestIdentity.SiteGuid
                         select new
                         {
                             Gateway = grp.GroupNum,
                             StationNumber = station.StationNum,
                             Offset = station.DECODERSTNOFFSET,
                             Satellite = satellite.SatelliteNum
                         };
            var decoder = result.AsEnumerable()
                .GroupBy(
                    station =>
                        new { station.Gateway, station.Satellite },
                    (key, group) =>
                    {
                        var d = new Decoder(id, key.Gateway, key.Satellite);
                        var offsets = group.ToList().Select(s => new DecoderStation(s.StationNumber, s.Offset));
                        d.Offsets.AddRange(offsets);
                        return d;
                    }

                );
               
            
            return decoder;
        }
    }
}
