using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using LynxWebApi.Utility;
using NLog;

namespace LynxWebApi.Controllers
{
    public class ReportController : ApiController
    {
        public int Post(string category, string type)
        {
            NameValueCollection c=Request.Content.ReadAsFormDataAsync().Result;
            StringBuilder sb=new StringBuilder();
            foreach(string k in c.AllKeys)
                sb.AppendLine(k+":"+c[k]);
            Logger logger;
            if (category == "hhri" && type == "android")
                logger = Log.GetHhriAndroidReportLogger();
            else
                logger = Log.GetReportLogger();
            logger.Debug(Environment.NewLine+sb.ToString(), Log.Level.Info);
            return 1;
        }
    }
}
