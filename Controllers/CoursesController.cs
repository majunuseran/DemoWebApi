using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LynxWebApi.Models;
using LynxWebApi.Utility;
using NLog;

namespace LynxWebApi.Controllers
{
    public class CoursesController : BaseController
    {
        private static Logger _messageLog = Log.GetMsgLogger();
        public IEnumerable Get()
        {
            return from course in entities.GolfCourses
                   where (course.LynxSiteGUID == requestIdentity.SiteGuid)
                   select new
                   {
                       course.CourseID,
                       CourseName = course.Name,
                       course.SystemUniqueID
                   };

        }
        public AuditStation Get(int stationid, int courseid)
        {
            var result = (from station in entities.STATIONs
                          join satellite in entities.SATELLITEs
                          on station.SatelliteID equals satellite.SatelliteID
                          join grp in entities.Groups
                          on satellite.GroupID equals grp.GroupID
                          join nozzle in entities.Nozzles
                          on station.NozzleID equals nozzle.NozzleID
                          join sprinkler in entities.Sprinklers
                          on nozzle.SprinklerID equals sprinkler.SprinklerID
                          join holearea in entities.HoleAreas
                          on station.HoleAreaID equals holearea.HoleAreaID
                          join areatype in entities.AreaTypes
                          on holearea.AreaTypeID equals areatype.AreaTypeID
                          join hole in entities.HOLEs
                          on holearea.HoleID equals hole.HoleID
                          where (station.StationID == stationid
                          && station.LynxSiteGUID == requestIdentity.SiteGuid
                          && satellite.LynxSiteGUID == requestIdentity.SiteGuid
                          && grp.LynxSiteGUID == requestIdentity.SiteGuid
                          && holearea.LynxSiteGUID == requestIdentity.SiteGuid
                          && areatype.LynxSiteGUID == requestIdentity.SiteGuid
                          && hole.LynxSiteGUID == requestIdentity.SiteGuid
                          && grp.CourseID == courseid)
                          select new AuditStation()
                          {
                              //Suid = 0,
                              CourseId = courseid,
                              StationId = stationid,
                              DecoderAddress = station.DECODERADDRESS,
                              DecoderStationOffset = station.DECODERSTNOFFSET,
                              StationName = station.Name,
                              Area = areatype.Name,
                              Hole = hole.Name,
                              // TODO - Majunu - check if the correct field is being used to return the pressure value
                              Pressure = station.Act_Pres,
                              // Majunu - parse the last letter out of station descriptor to get station tag value
                              StationTag = (station.StationDescriptor.Length > 0) ? station.StationDescriptor.Substring(station.StationDescriptor.Length - 1, station.StationDescriptor.Length) : station.StationDescriptor,
                              NoOfSprinklers = station.Nbr_Noz,
                              SprinklerModel = new SprinklerModel() { Id = sprinkler.SprinklerID, Name = sprinkler.Name },
                              NozzleModel = new NozzleModel() { Id = nozzle.NozzleID, Name = nozzle.Nozzle_Nbr },
                              Arc = station.Arc,
                              Pattern = (Pattern)station.Sprink_Pat,
                              SprinklerSpacing = station.Head_Spacing,
                              RowSpacing = station.Row_Spacing,
                              TurfGuard = station.tgSensor,
                              SiteCodes = new LynxWebApi.Models.SiteCodes()
                              {
                                  siteCodesList = new List<LynxWebApi.Models.SiteCode>() {
                                    new LynxWebApi.Models.SiteCode() { SiteTypeId = SiteCodeEnum.PlantSite, SiteTypeValue = station.Plant_Type },
                                    new LynxWebApi.Models.SiteCode() { SiteTypeId = SiteCodeEnum.SoilType, SiteTypeValue = station.Soil_Type },
                                    new LynxWebApi.Models.SiteCode() { SiteTypeId = SiteCodeEnum.SoilComp, SiteTypeValue = station.Soil_Comp },
                                    new LynxWebApi.Models.SiteCode() { SiteTypeId = SiteCodeEnum.SoilSlope, SiteTypeValue = station.Soil_Slope },
                                    new LynxWebApi.Models.SiteCode() { SiteTypeId = SiteCodeEnum.Shade, SiteTypeValue = station.User_Code_1 },
                                    new LynxWebApi.Models.SiteCode() { SiteTypeId = SiteCodeEnum.ClubHouse, SiteTypeValue = station.User_Code_2 },
                                    new LynxWebApi.Models.SiteCode() { SiteTypeId = SiteCodeEnum.DrivingRange, SiteTypeValue = station.User_Code_3 }
                                 }
                              },
                              AutoCycleMax = station.Max_Cycle_Time,
                              AutoSoakMin = station.Soak_Time,
                              TerminalStation = station.LinePosition
                          }).ToList().FirstOrDefault();
            AuditStation auditStation = LynxWebApi.Utility.Helper.SetSuids((AuditStation)result, requestIdentity.SiteGuid);
            return auditStation;

        }

    }
}
