using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LynxWebApi.Controllers
{
    public class PingController : ApiController
    {
        /// <summary>
        /// This is to test if the token in the "Authorization" header of a request is valid and not expired
        /// Check the response status code for validility of a token: 200 OK or 204 No Content: token valid; 401 Unauthorized: token invalid  
        /// No response data for request method Head
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public void Head()
        {
            return;
        }
    }
}
