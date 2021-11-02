using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LynxWebApi.Controllers
{
    public class CourseReportController : BaseController
    {
        public IEnumerable Get()
        {
            return entities.GetCourseReport(requestIdentity.SiteGuid);            
        }
    }
}
