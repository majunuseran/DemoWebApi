using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LynxWebApi.Controllers
{
    public class DemoController : ApiController
    {
        // GET api/values
        /// <summary>
        /// Return new string[] { "value1", "value2" };
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //// GET api/values/5
        /// <summary>
        /// Authorization header needed
        /// Return Token@LynxSiteGUID
        /// </summary>
        /// <param name="id">Any integer</param>
        /// <returns></returns>
        [Authorize]
        public string Get(int id)
        {
            return User.Identity.Name;
        }
        
        // POST api/values
        /// <summary>
        /// Do nothing
        /// </summary>
        /// <param name="value"></param>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        /// <summary>
        /// Do nothing
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        /// <summary>
        /// Do nothing
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
        }
    }
}