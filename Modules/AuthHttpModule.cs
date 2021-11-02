using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Web;
using LynxMobileData;

namespace LynxWebApi.Modules
{
    public class AuthHttpModule : IHttpModule
    {
        private const string Realm = "mobilews.toro.com/lynx";

        public void Init(HttpApplication context)
        {
            // Register event handlers
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }

        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private static bool AuthenticateAccount(string tokenStr)
        {
            bool validated = false;
            try
            {
                using (var entities = new LynxMainCentralEntities())
                {
                    var token = entities.AccountAccessTokens.Find(tokenStr);
                    if (token != null && token.LynxSiteGUID>0)
                    {
                        if (token.ExpireUtcTime > DateTime.UtcNow)
                        {
                            var identity = new GenericIdentity(token.Token+"@"+token.LynxSiteGUID.ToString());
                            SetPrincipal(new GenericPrincipal(identity, null));
                            token.LastAccessUtcTime = DateTime.UtcNow;
                            entities.SaveChanges();
                            validated = true;
                        }
                        else
                        {
                            entities.AccountAccessTokens.Remove(token);
                            entities.SaveChanges();
                        }
                    }
                }
            }
            catch (FormatException)
            {
                // Credentials were not formatted correctly.
                validated = false;
            }
            return validated;
        }

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            HttpRequest request = HttpContext.Current.Request;
            var authHeader = request.Headers["Authorization"];
            if (authHeader != null)
            {
                string authHeaderVal = AuthenticationHeaderValue.Parse(authHeader).ToString();

                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                if (authHeaderVal!= null)
                {
                    AuthenticateAccount(authHeaderVal);
                }
                //else if (request.HttpMethod == "OPTIONS")
                //{
                //    var identity = new GenericIdentity("PreflightRequest");
                //    SetPrincipal(new GenericPrincipal(identity, null));
                //}
            }
        }

        // If the request was unauthorized, add the WWW-Authenticate header 
        // to the response.
        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
            {
                response.Headers.Add("WWW-Authenticate",
                    string.Format("Basic realm=\"{0}\"", Realm));
            }
        }

        public void Dispose()
        {
        }

        public static object[] token { get; set; }
    }
}