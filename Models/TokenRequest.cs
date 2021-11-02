using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LynxWebApi.Models
{
    public class TokenRequest
    {
        public string Credentials { get; set; }
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string DevicePlatform { get; set; }
        public string DevicePlatformVersion { get; set; }
        public string DeviceName { get; set; }
        public string DeviceID { get; set; }
    }
}