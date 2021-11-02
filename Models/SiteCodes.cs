using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LynxWebApi.Models
{
    public class SiteCodes
    {
        public List<SiteCode> siteCodesList { get; set; }
    }

    public class SiteCode
    {
        public SiteCodeEnum SiteTypeId { get; set; }
        public string SiteTypeValue { get; set; }
    }
}