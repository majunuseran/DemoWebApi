using LynxMobileData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using LynxWebApi.Commands;
using LynxWebApi.Models;
using LynxWebApi.Utility;
using System.Threading.Tasks;
using NLog;


namespace LynxWebApi.Controllers
{
   
    public class StationsController : BaseController
    {

        private static Logger _messageLog = Log.GetMsgLogger();

        //public IEnumerable Get(int id)
        //{
        //    RequestIdentity requestIdentity = new RequestIdentity(User.Identity);
        //    var stations = from station in entities.STATIONs
        //                   join stationAssignment in entities.StationAssignments
        //                   on station.StationID equals stationAssignment.StationID
        //                   where (station.LynxSiteGUID == requestIdentity.SiteGuid && stationAssignment.LynxSiteGUID == requestIdentity.SiteGuid && station.HoleAreaID == id)
        //                   select new
        //                   {
        //                       station.StationDescriptor,
        //                       stationAssignment.SystemUniqueID
        //                   };
        //    return stations;

        //    //return entities.STATIONs.Where(s => s.LynxSiteGUID == requestIdentity.SiteGuid && s.HoleAreaID == id).Select(x => new { x.StationID, x.StationNum, x.StationDescriptor});
        //}
        //public IEnumerable Get(int id)
        //{
        //    RequestIdentity requestIdentity = new RequestIdentity(User.Identity);
        //    var stations = from station in entities.STATIONs
        //                   join sat in entities.SATELLITEs
        //                   on station.SatelliteID equals sat.SatelliteID
        //                   where (station.LynxSiteGUID == requestIdentity.SiteGuid && sat.LynxSiteGUID == requestIdentity.SiteGuid && station.HoleAreaID == id)
        //                   select new
        //                   {
        //                       station.StationID,
        //                       sat.SatelliteID,
        //                       sat.GroupID
        //                   };
        //    return stations;
        //}
        public IEnumerable Get(string category, int type)
        {
            if (category == "holearea")
            {
                return from station in entities.STATIONs
                       join satellite in entities.SATELLITEs
                       on station.SatelliteID equals satellite.SatelliteID
                       join grp in entities.Groups
                       on satellite.GroupID equals grp.GroupID
                       join course in entities.GolfCourses
                       on grp.CourseID equals course.CourseID
                       where (station.LynxSiteGUID == requestIdentity.SiteGuid && satellite.LynxSiteGUID == requestIdentity.SiteGuid && grp.LynxSiteGUID == requestIdentity.SiteGuid && course.LynxSiteGUID == requestIdentity.SiteGuid && station.HoleAreaID == type && station.StationDescriptor != "*")
                       select new
                       {
                           station.StationDescriptor,
                           station.SystemUniqueID,
                           station.StationNum,
                           station.HoleAreaID,
                           //station.Name,
                           DecoderAddress = station.DECODERADDRESS,
                           DecoderStationOffset = station.DECODERSTNOFFSET,
                           satellite.SatelliteNum,
                           satellite.SatelliteID,
                           grp.GroupNum,
                           course.CourseID
                       };
            }
            else
            {
                return from station in entities.STATIONs
                       join satellite in entities.SATELLITEs
                       on station.SatelliteID equals satellite.SatelliteID
                       join grp in entities.Groups
                       on satellite.GroupID equals grp.GroupID
                       join course in entities.GolfCourses
                       on grp.CourseID equals course.CourseID
                       where (station.LynxSiteGUID == requestIdentity.SiteGuid && satellite.LynxSiteGUID == requestIdentity.SiteGuid && grp.LynxSiteGUID == requestIdentity.SiteGuid && course.LynxSiteGUID == requestIdentity.SiteGuid && station.SatelliteID == type)
                       select new
                       {
                           station.StationDescriptor,
                           station.SystemUniqueID,
                           station.StationNum,
                           station.HoleAreaID,
                           //station.Name,
                           DecoderAddress = station.DECODERADDRESS,
                           DecoderStationOffset = station.DECODERSTNOFFSET,
                           satellite.SatelliteNum,
                           satellite.SatelliteID,
                           grp.GroupNum,
                           course.CourseID
                       };
            }
        }
        

        public IEnumerable Get(int id)
        {
            return
              from station in entities.STATIONs
              join satellite in entities.SATELLITEs
              on station.SatelliteID equals satellite.SatelliteID
              join grp in entities.Groups
              on satellite.GroupID equals grp.GroupID
              where (station.LynxSiteGUID == requestIdentity.SiteGuid && satellite.LynxSiteGUID == requestIdentity.SiteGuid && grp.LynxSiteGUID == requestIdentity.SiteGuid && grp.CourseID == id)
              select new
              {
                  station.SystemUniqueID,
                  station.HoleAreaID,
                  station.StationDescriptor,
                  station.StationNum,
                  DecoderAddress = station.DECODERADDRESS,
                  DecoderStationOffset = station.DECODERSTNOFFSET,
                  satellite.SatelliteNum,
                  satellite.SatelliteID,
                  grp.GroupNum,
                  grp.CourseID
              };
        }
        public IEnumerable Get()
        {
            return
               from station in entities.STATIONs
               join satellite in entities.SATELLITEs
               on station.SatelliteID equals satellite.SatelliteID
               join grp in entities.Groups
               on satellite.GroupID equals grp.GroupID
               where (station.LynxSiteGUID == requestIdentity.SiteGuid && satellite.LynxSiteGUID == requestIdentity.SiteGuid && grp.LynxSiteGUID == requestIdentity.SiteGuid)
               select new
               {
                   station.SystemUniqueID,
                   station.HoleAreaID,
                   station.StationDescriptor,
                   station.StationNum,
                   DecoderAddress = station.DECODERADDRESS,
                   DecoderStationOffset = station.DECODERSTNOFFSET,
                   satellite.SatelliteNum,
                   satellite.SatelliteID,
                   grp.GroupNum,
                   grp.CourseID
               };
        }
        public async Task<int> Post(AuditStation auditStation)
        {
            //_messageLog.Debug(await Request.Content.ReadAsStringAsync());
            AuditStation newAuditStation = LynxWebApi.Utility.Helper.SetSuids((AuditStation)auditStation, requestIdentity.SiteGuid);
            //ids field from command table will be overloaded to contain
            //station id
            var ids = AuditStation.SerializeDecoders(new List<AuditStation> { newAuditStation });
            //_messageLog.Debug("Audit Stations :" + ids);
            var command = new Command()
            {
                CommandFamily = (int)CommandFamily.AuditStation,
                CommandType = (int)AuditStationCommand.AuditStationCommandType.Update,
                Ids = ids
            };
            AuditStationCommand auditStationCommand = new AuditStationCommand(command, requestIdentity);
            return auditStationCommand.Queue();
        }

    }
}
