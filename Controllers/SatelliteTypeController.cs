using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LynxMobileData;

namespace LynxWebApi.Controllers
{   
    public class SatelliteTypeController : BaseController
    {
        /// <summary>
        /// 404 Not Found if Satellite Type not found
        /// 
        /// Return Satellite Type (int). -1 
        /// 
        /// Please refer table SatelliteModelType in Lynx_Main database
        /// SatelliteModelTypeID	Name	
        ///     0	                NotSet			
        ///     2	            OsmacSatellite			
        ///     7	            LTCPlusSatellite			
        ///     8	            VPSatellite			
        ///     9	            GDCSationGroup	
        ///     
        /// jQuery example:
        /// 
        /// $.ajax({
        ///     url: 'https://mobilews.toro.com/lynx/api/satellitetype',
        ///     type : 'GET',
        ///     headers : {
        ///         'Authorization' : token
        ///     }
        /// }); 	
        /// </summary>
        /// <returns>       
        /// </returns>
        public int GetSatelliteType()
        {            
            var satelliteType=entities.SATELLITEs.Where(s => s.LynxSiteGUID == requestIdentity.SiteGuid).First();
            if (satelliteType != null)
                return satelliteType.SatelliteModelTypeID;
            else
                throw new HttpResponseException(HttpStatusCode.NotFound);
            
        }
    }
}
