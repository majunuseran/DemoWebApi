using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using LynxWebApi.Controllers;

namespace LynxWebApi.Models
{
    public class AuditStation
    {
        public long Suid{ get; set; }
        public int CourseId { get; set; }
        public int StationId { get; set; }
        public int Gateway { get; set; }
        public int StationGroup { get; set; }
        public string StationName { get; set; }
        public int DecoderStationOffset { get; set; }
        public string Area { get; set; }
        public string Hole { get; set; }
        public double Pressure { get; set; }
        public string StationTag { get; set; }
        public int NoOfSprinklers { get; set; }
        public SprinklerModel SprinklerModel { get; set; }
        public NozzleModel NozzleModel { get; set; }
        public int Arc { get; set; }
        public Pattern Pattern { get; set; }
        public double SprinklerSpacing { get; set; }
        public double RowSpacing { get; set; }
        public int PrecipRate { get; set; }
        public int TurfGuard { get; set; }
        public SiteCodes SiteCodes { get; set; }
        public int AutoCycleMax { get; set; }
        public int AutoSoakMin { get; set; }
        public int TerminalStation { get; set; }

        public int DecoderAddress { get; set; }
        public static string SerializeDecoders(List<AuditStation> auditStations)
        {
            var serialzier = new DataContractJsonSerializer(typeof(List<AuditStation>));
            var memStream = new MemoryStream();
            serialzier.WriteObject(memStream, auditStations);
            memStream.Position = 0;
            var reader = new StreamReader(memStream);
            return reader.ReadToEnd();
        }
    }

    public class SprinklerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class NozzleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}