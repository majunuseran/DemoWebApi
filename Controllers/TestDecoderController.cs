using LynxWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LynxWebApi.Controllers
{
    public class TestDecoderController : ApiController
    {
        public int Post(Decoder decoder)
        {
            return 1;
        }
    }
}
