using System;
using System.Net;
using System.Web.Http;

using LynxMobileData;
using LynxWebApi.Commands;
using LynxWebApi.Utility;

namespace LynxWebApi.Controllers
{
    [Authorize]
    public class HhriCmdController : ApiController
    {
        /// <summary>
        /// Queue HHRI command to Lynx Mobile database
        /// Specify Ids as command string, which starts with *
        /// Only Ids(string) is required in the request, while CommandType(int) and Value(int) are optional
        /// 
        /// Response: 
        /// 400 Bad Request: cmd is null or cmd.Ids is null
        /// Return 1: Processed; 0: Not Processed; -1: Error occurred while sending command to NSN server; 
        /// 
        /// jQuery example: 
        /// 
        /// $.ajax({
		///     url:'https://mobilews.toro.com/lynx/api/HhriCmd',
		///     type : 'POST',
		///     headers : {
		///	       'Authorization' : token
		///     },
        ///     data : {
        ///         'Ids': '*830'
        ///     }
        /// });
        /// </summary>
        /// <param name="cmd">Ids (string reqired); CommandType (int optional); Value (int optional); Other (not needed) 
        /// </param>
        /// <returns></returns>
        public int Post(Command cmd)
        {            
            HhriCommand hhriCmd = new HhriCommand(cmd, new RequestIdentity(User.Identity));
            return hhriCmd.Queue();            
        } 
    }
}
