using LynxMobileData;
using LynxWebApi.Models;
using LynxWebApi.Utility;
using NLog;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;
using MobileAppAuth;


namespace LynxWebApi.Controllers
{
    public class TokenController : ApiController
    {
        protected LynxMainCentralEntities entities = new LynxMainCentralEntities();
        private static Logger cmdLog = Log.GetCmdLogger();
        private static Logger msgLog = Log.GetMsgLogger();
        /// <summary>
        /// Get token by providing TokenRequest JSON object. 
        /// Required TokenRequest property: "Credentials","AppName","DevicePlatform", "DeviceID"
        /// Optional TokenRequest properties: "DevicePlatformVersion","DeviceName","AppVersion"
        /// No "Authorization" header is needed
        /// 
        /// Response 
        /// 400 Bad Request: tokenRequest is null or tokenRequest.Credentials is null or tokenRequest.Credentials is not in the format of Username:Password
        /// 404 Not Found: the account with indicated username and password does not exist
        /// 500 Internal Server Error: Exceptions thrown out
        /// 
        /// This is the first step for each app using requests requiring "Authorize". 
        /// The app should save the token got from this request, and use it as "Authorization" header.
        ///
        /// jQuery example:
        /// 
        /// $.ajax({
        ///	    url:'https://mobilews.toro.com/lynx/api/token,
        ///     type : 'POST',
        ///     dataType : 'json',
        ///     data : {
        ///         "Credentials" : credentials,
        ///         "AppName":"hhri",
        ///         "DeviceName":"Samsung SPH-D710",
        ///         "DevicePlatform":"Android 4.1.1",
        ///         "DeviceID":"5e51d5706b994dce" 
        ///     }     
        /// });
        /// </summary>
        /// <param name="tokenRequest">
        ///     Required: "Credentials","AppName","DevicePlatform", "DeviceID"
        ///     Optional: "DeviceName","AppVersion"
        /// </param>
        /// <returns></returns>
        public virtual string Post(TokenRequest tokenRequest)
        {
            int siteGuid = 0;
            return getTokenByPost(tokenRequest, out siteGuid);
        }

        private void removeExpiredToken(int siteGuid)
        {
            var expiredTokens = entities.AccountAccessTokens.Where(t => t.LynxSiteGUID == siteGuid && t.ExpireUtcTime <= DateTime.UtcNow).ToList();
            foreach (var token in expiredTokens)
            {
                entities.AccountAccessTokens.Remove(token);
            }
            entities.SaveChanges();
        }

        private string getAccessToken(int guid, TokenRequest tokenRequest)
        {
            try
            {
                checkTokenRequest(tokenRequest);
                var token = new AccountAccessToken();
                token.LynxSiteGUID = guid;
                do
                {
                    token.Token = generateToken();
                } while (entities.AccountAccessTokens.Where(a => a.Token == token.Token).ToList().Count > 0);
                token.IssueUtcTime = System.DateTime.UtcNow;
                double expiringDays = 1;
                //switch (tokenRequest.AppName)
                //{
                //    case "HHRI Mobile Beta":
                //    case "ManualOps":
                //        expiringDays = 1;
                //        break;
                //    default:
                //        break;
                //}
                token.AppName = tokenRequest.AppName;
                token.AppVersion = tokenRequest.AppVersion;
                token.ExpireUtcTime = System.DateTime.UtcNow.AddDays(expiringDays);
                token.DevicePlatform = tokenRequest.DevicePlatform;
                token.DevicePlatformVersion = tokenRequest.DevicePlatformVersion;
                token.DeviceName = tokenRequest.DeviceName;
                token.DeviceID = tokenRequest.DeviceID;
                entities.AccountAccessTokens.Add(token);

                entities.SaveChanges();

                cmdLog.Debug(string.Format("Add token {0} SiteGUID {1} {2} {3} {4} {5} {6} {7} {8}", token.Token, token.LynxSiteGUID, token.AppName, token.AppVersion, token.ExpireUtcTime, token.DevicePlatform, token.DevicePlatformVersion, token.DeviceName, token.DeviceID), Log.Level.Info);
                return token.Token;
            }
            catch (Exception e)
            {
                if (!(e is HttpResponseException))
                {
                    msgLog.Debug("Error occurred while getAccessToken with " + guid + " " + tokenRequest.AppName + " " + tokenRequest.AppVersion + " " + tokenRequest.DevicePlatform + " " + tokenRequest.DevicePlatformVersion + " " + tokenRequest.DeviceName + " " + tokenRequest.DeviceID + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace, Log.Level.Error);
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
                else
                {
                    throw;
                }
            }
        }

        private void checkTokenRequest(TokenRequest tokenRequest)
        {
            if (string.IsNullOrWhiteSpace(tokenRequest.AppName) || string.IsNullOrWhiteSpace(tokenRequest.DeviceID) || string.IsNullOrWhiteSpace(tokenRequest.DevicePlatform))
                throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        private string generateToken()
        {
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789+$-_.+!*";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 15)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            entities.Dispose();
        }

        protected string getTokenByPost(TokenRequest tokenRequest, out int siteGuid)
        {
            if (tokenRequest == null || tokenRequest.Credentials == null)
            {
                string msg = tokenRequest == null ? "TokenRequest object is null" : "Credentials are null";
                msgLog.Debug(msg + " while getting token", Log.Level.Debug);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            try
            {
                var encoding = Encoding.GetEncoding("iso-8859-1");
                string credentials = encoding.GetString(Convert.FromBase64String(tokenRequest.Credentials));
                int separator = credentials.IndexOf(':');
                string[] strs = credentials.Split(':');
                if (strs.Length >= 2)
                {
                    string username = strs[0];
                    string password = strs[1];
                    var accounts = entities.Accounts.Where(a => a.username == username && a.password == password).ToList();
                    if (accounts.Count > 0)
                    {

                        // here we have valid user name and password. Now check if this is for a mobile app
                        // and if so check if authorized. This does not occur for Lynx Mobile Web Site
                        //    if (MobileAppAuthApiClient.isMobileApp(tokenRequest.AppName) == true) // always a mobile app
                        //    {
                        string xuserSignonMessage = string.Empty;
                        
                        bool hasPremission = MobileAppAuthApiClient.GetUserPermission(username, out  xuserSignonMessage);

                        if (hasPremission == false)
                        {
                            string AppNot = string.Format("Unauthorized app user: {0}  {1}  {2}  {3} {4} {5} {6} ", username, tokenRequest.AppName, tokenRequest.AppVersion, tokenRequest.DevicePlatform, tokenRequest.DevicePlatformVersion, tokenRequest.DeviceName, tokenRequest.DeviceID);
                            msgLog.Debug(AppNot, Log.Level.Debug);
                            throw new HttpResponseException(HttpStatusCode.Unauthorized);
                        }
                        else
                        {
                            string AppIs = string.Format("App user: {0}  {1}  {2}  {3} {4} {5} {6} ", username, tokenRequest.AppName, tokenRequest.AppVersion, tokenRequest.DevicePlatform, tokenRequest.DevicePlatformVersion, tokenRequest.DeviceName, tokenRequest.DeviceID);

                            msgLog.Debug(AppIs, Log.Level.Debug);
                        }
                        //     }



                        siteGuid = accounts[0].LynxSiteGUID;
                        removeExpiredToken(siteGuid);
                        string token = getAccessToken(siteGuid, tokenRequest);
                        return token;
                    }
                    else
                    {
                        msgLog.Debug("Unauthorized: " + tokenRequest.Credentials, Log.Level.Debug);
                        throw new HttpResponseException(HttpStatusCode.Unauthorized);
                    }
                }
                else
                {
                    msgLog.Debug("BadRequest: " + tokenRequest.Credentials, Log.Level.Debug);
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception e)
            {
                if (!(e is HttpResponseException))
                {
                    var json = new JavaScriptSerializer().Serialize(tokenRequest);
                    msgLog.Debug("Errors occurred in Post TokenController with data " + json + Environment.NewLine + e.Message + Environment.NewLine + e.InnerException + Environment.NewLine + e.StackTrace, Log.Level.Error);
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
                else
                    throw;
            }
        }
    }
}
