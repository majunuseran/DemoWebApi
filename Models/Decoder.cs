using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;

namespace LynxWebApi.Models
{
    [Serializable]
    public class DecoderStation
    {
        public DecoderStation(int station, int offset)
        {
            this.Station = station;
            this.Offset = offset;

        }

        public long Suid;
        public int Station;
        public int Offset;
    }
    [Serializable]
    public class Decoder
    {
        public readonly int Address;
        public readonly List<DecoderStation> Offsets;
        public readonly int Gateway;
        public readonly int StationGroup;
        public readonly float Latitude;
        public readonly float Longitude;

        public Decoder(int address, int gateway, int stationGroup,
            float latitude = 0f,float longitude = 0f)
        {
            Address = address;
            Offsets = new List<DecoderStation>();
            Gateway = gateway;
            StationGroup = stationGroup;
            Latitude = latitude;
            Longitude = longitude;
        }
        public static string SerializeDecoders(List<Decoder> decoders)
        {
                var serialzier = new DataContractJsonSerializer(typeof(List<Decoder>));
                var memStream = new MemoryStream();
                serialzier.WriteObject(memStream, decoders);
                memStream.Position = 0;
                var reader = new StreamReader(memStream);
                return reader.ReadToEnd();
        }
    }
}