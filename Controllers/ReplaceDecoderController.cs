using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Management;
using System.Web.Script.Serialization;
using LynxMobileData;
using LynxWebApi.Commands;
using Microsoft.Ajax.Utilities;
using NLog;
using NLog.LayoutRenderers.Wrappers;
using Decoder = LynxWebApi.Models.Decoder;
using LynxWebApi.Utility;

namespace LynxWebApi.Controllers
{
    public class ReplaceDecoderController : BaseController
    {
        //Decoder has updated adress and station offset numbers
        private static Logger _messageLog = Log.GetMsgLogger();
        public async Task<int> Post(Decoder decoder)
        {
            //_messageLog.Debug(await Request.Content.ReadAsStringAsync());
            SetSuids(decoder);
            //ids field from command table will be overloaded to contain
            //serialized decoders
            var ids = Decoder.SerializeDecoders(new List<Decoder> { decoder });
            _messageLog.Debug("ReplaceDecoder sending decoder:" + ids);
            var command = new Command()
            {
                CommandFamily = (int)CommandFamily.Decoder,
                CommandType = (int)DecoderCommand.DecoderCommandType.Replace,
                Ids = ids
            };
            DecoderCommand decoderCommand = new DecoderCommand(command, requestIdentity);
            return decoderCommand.Queue();
        }
        //public async Task Post()
        //{
        //    _messageLog.Debug(await Request.Content.ReadAsStringAsync());
        //}
        private void SetSuids(Decoder decoder)
        {
            var results = from station in entities.STATIONs
                          join satellite in entities.SATELLITEs
                              on station.SatelliteID equals satellite.SatelliteID
                          join grp in entities.Groups
                              on satellite.GroupID equals grp.GroupID
                          where station.LynxSiteGUID == requestIdentity.SiteGuid &&
                                satellite.LynxSiteGUID == requestIdentity.SiteGuid &&
                                grp.LynxSiteGUID == requestIdentity.SiteGuid &&
                                grp.GroupNum == decoder.Gateway &&
                                satellite.SatelliteNum == decoder.StationGroup
                          select station;
            //var stationNumbers = decoder.Offsets.Select(so => so.Station);
            var updatedSuids = 0;
            var resultsAsList = results.ToList();
            var filtered = resultsAsList.Where(
                r => decoder.Offsets.Any(s =>
                {
                    if (r.StationNum != s.Station)
                    {
                        return false;
                    }
                    
                    s.Suid = r.SystemUniqueID;
                    ++updatedSuids;
                    return true;
                })).ToList();
            
            if (updatedSuids == 0)
            {
                _messageLog.Debug("Did not find any SUIDs for those stations");
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
               
            }
        }

        //private int UpdateStations(IQueryable<STATION> stations, Decoder decoder)
        //{
        //    var updatedStations = 0;
            
        //    foreach (var stationOffset in decoder.Offsets)
        //    {
        //        var oldStation = stations
        //            .SingleOrDefault(s => s.StationNum == stationOffset.Station);
        //        if (oldStation != default(STATION))
        //        {

        //            if (stationOffset.Offset == 0)
        //            {
        //                //delete record
        //                entities.STATIONs.Remove(oldStation);
        //                ++updatedStations;
        //            }
        //            else
        //            {
        //                oldStation.DECODERADDRESS = decoder.Address;
        //                oldStation.DECODERSTNOFFSET = stationOffset.Offset;
        //                ++updatedStations;
        //            }
        //        }

        //    }
        //    return updatedStations;
        //}
    }
}
