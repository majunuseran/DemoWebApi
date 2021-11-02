using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LynxWebApi
{
    public class RequestIdentity
    {
        public RequestIdentity(System.Security.Principal.IIdentity id)
        {
            identity = id;
        }
        private System.Security.Principal.IIdentity identity{get;set;}
        private int siteGuid = 0;
        private string token;
        public int SiteGuid
        {
            get
            {
                if (this.siteGuid == 0)
                {
                    string[] strs = identity.Name.Split('@');
                    if (strs.Length > 1)
                        int.TryParse(strs[1], out this.siteGuid);
                }
                return this.siteGuid;
            }
        }
        public string Token
        {
            get
            {
                if(String.IsNullOrEmpty(this.token))
                    this.token=identity.Name.Split('@')[0];
                return this.token;
            }
        }
    }
}