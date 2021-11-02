using System.Collections;
using System.Linq;

namespace LynxWebApi.Controllers
{
    public class StationLocationsController : BaseController
    {
        public IEnumerable Get()
        {
            return from stnLoc in entities.StationLocations
                   join stn in entities.STATIONs
                   on stnLoc.StationSUID equals stn.SystemUniqueID
                   join satellite in entities.SATELLITEs
                   on stn.SatelliteID equals satellite.SatelliteID
                   join grp in entities.Groups
                   on satellite.GroupID equals grp.GroupID
                   where (stnLoc.LynxSiteGUID == requestIdentity.SiteGuid && stn.LynxSiteGUID == requestIdentity.SiteGuid && satellite.LynxSiteGUID == requestIdentity.SiteGuid && grp.LynxSiteGUID == requestIdentity.SiteGuid)
                   select new
                   {
                       stnLoc.Latitude,
                       stnLoc.Longitude,
                       stnLoc.StationSUID,
                       grp.CourseID
                   };
        }
        public IEnumerable Get(int courseId)
        {
            return from stnLoc in entities.StationLocations
                   join stn in entities.STATIONs
                   on stnLoc.StationSUID equals stn.SystemUniqueID
                   join satellite in entities.SATELLITEs
                   on stn.SatelliteID equals satellite.SatelliteID
                   join grp in entities.Groups
                   on satellite.GroupID equals grp.GroupID
                   where (stnLoc.LynxSiteGUID == requestIdentity.SiteGuid && stn.LynxSiteGUID == requestIdentity.SiteGuid && satellite.LynxSiteGUID == requestIdentity.SiteGuid && grp.LynxSiteGUID == requestIdentity.SiteGuid && grp.CourseID==courseId)
                   select new
                   {
                       stnLoc.Latitude,
                       stnLoc.Longitude,
                       stnLoc.StationSUID,
                       grp.CourseID
                   };
        }
        //public IEnumerable Get()
        //{
        //    var stations =
        //       from station in entities.STATIONs
        //       join satellite in entities.SATELLITEs
        //       on station.SatelliteID equals satellite.SatelliteID
        //       join grp in entities.Groups
        //       on satellite.GroupID equals grp.GroupID
        //       where (station.LynxSiteGUID == requestIdentity.SiteGuid && satellite.LynxSiteGUID == requestIdentity.SiteGuid && grp.LynxSiteGUID == requestIdentity.SiteGuid)
        //       select new
        //       {
        //           station.SystemUniqueID,
        //           station.HoleAreaID,
        //           station.StationDescriptor,
        //           station.StationNum,
        //           //station.Name,
        //           satellite.SatelliteNum,
        //           satellite.SatelliteID,
        //           grp.GroupNum,
        //           grp.CourseID
        //       };
        //    var stationlocations = entities.StationLocations.Where(x => x.LynxSiteGUID == requestIdentity.SiteGuid );
        //    var stnloc= from station in stations
        //           join stationlocation in stationlocations
        //           on station.SystemUniqueID equals stationlocation.StationSUID
        //           into sl
        //           from subStationLocation in sl.DefaultIfEmpty()
        //           select new
        //           {
        //               Latitude = (subStationLocation == null ? 1000 : subStationLocation.Latitude),
        //               Longitude = (subStationLocation == null ? 1000 : subStationLocation.Longitude),
        //               station.SystemUniqueID,
        //               station.HoleAreaID,
        //               station.StationDescriptor,
        //               station.StationNum,
        //               //station.Name,
        //               station.SatelliteNum,
        //               station.SatelliteID,
        //               station.GroupNum,
        //               station.CourseID
        //           };
        //    return stnloc;
        //}
        //public IEnumerable Get(int id)
        //{
        //    var stations =
        //       from station in entities.STATIONs
        //       join satellite in entities.SATELLITEs
        //       on station.SatelliteID equals satellite.SatelliteID
        //       join grp in entities.Groups
        //       on satellite.GroupID equals grp.GroupID
        //       where (station.LynxSiteGUID == requestIdentity.SiteGuid && satellite.LynxSiteGUID == requestIdentity.SiteGuid && grp.LynxSiteGUID == requestIdentity.SiteGuid && grp.CourseID == id)
        //       select new
        //       {
        //           station.SystemUniqueID,
        //           station.HoleAreaID,
        //           station.StationDescriptor,
        //           station.StationNum,
        //           //station.Name,
        //           satellite.SatelliteNum,
        //           satellite.SatelliteID,
        //           grp.GroupNum,
        //           grp.CourseID
        //       };
        //    var stationlocations = entities.StationLocations.Where(x => x.LynxSiteGUID == requestIdentity.SiteGuid );
        //    var stnloc= from station in stations
        //           join stationlocation in stationlocations
        //           on station.SystemUniqueID equals stationlocation.StationSUID
        //           into sl
        //           from subStationLocation in sl.DefaultIfEmpty()
        //           select new
        //           {
        //               Latitude = (subStationLocation == null ? 1000 : subStationLocation.Latitude),
        //               Longitude = (subStationLocation == null ? 1000 : subStationLocation.Longitude),
        //               station.SystemUniqueID,
        //               station.HoleAreaID,
        //               station.StationDescriptor,
        //               station.StationNum,
        //               //station.Name,
        //               station.SatelliteNum,
        //               station.SatelliteID,
        //               station.GroupNum,
        //               station.CourseID
        //           };
        //    return stnloc;
        //}
    }
}
