using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LynxWebApi.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LynxWebApi.Utility;
using NLog;
using LynxMobileData;

namespace LynxWebApi.Utility
{
    public class Helper
    {
        private static Logger _messageLog = Log.GetMsgLogger();
        public static DateTime GetCurrentCentralTime()
        {
            DateTime utcTime = DateTime.UtcNow;
            //convert to central time
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, cstZone);
        }

        public static AuditStation SetSuids(AuditStation auditStation, int SiteGuid)
        {
            LynxMainCentralEntities entities = new LynxMainCentralEntities();
            var stations = from station in entities.STATIONs
                           join satellite in entities.SATELLITEs
                               on station.SatelliteID equals satellite.SatelliteID
                           join grp in entities.Groups
                               on satellite.GroupID equals grp.GroupID
                           where station.LynxSiteGUID == SiteGuid &&
                                 satellite.LynxSiteGUID == SiteGuid &&
                                 grp.LynxSiteGUID == SiteGuid &&
                                 station.StationID == auditStation.StationId &&
                                 grp.CourseID == auditStation.CourseId
                           //grp.GroupNum == auditStation.Gateway &&
                           //satellite.SatelliteNum == auditStation.StationGroup
                           select station;

            var updatedSuids = 0;
            var stationsList = stations.ToList();
            List<AuditStation> auditStationList = new List<AuditStation>();
            auditStationList.Add(auditStation);

            var filteredStation = stationsList.Where(
                r => auditStationList.Any(s =>
                {

                    if (r.StationNum != s.StationId)
                    {
                        return false;
                    }

                    s.Suid = r.SystemUniqueID;
                    ++updatedSuids;
                    return true;


                }
                )).ToList();

            if (updatedSuids == 0)
            {
                _messageLog.Debug("Did not find any SUIDs for those stations");
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            }

            return auditStation;
        }
    }
}