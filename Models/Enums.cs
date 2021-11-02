using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;

namespace LynxWebApi.Models
{
    public enum SiteCodeEnum
    {
        PlantSite,
        SoilType,
        SoilComp,
        SoilSlope,
        Shade,
        ClubHouse,
        DrivingRange
    }

    public enum Pattern
    {
        Rectangle,
        Triangle,
        Inline,
        Area
    }

}


