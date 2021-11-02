

using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Text;

namespace MobileAppAuth
{
    public static class MobileAppAuthApiClient
    {
        #region fields

        // currently not used. all apps have auth
        public static bool isMobileApp(string appName)
        {
            


      
            switch (appName)
            {
                case "Lynx Barcode":
                    return true;
                case "Lynx Handheld":
                    return true;
                case "Lynx Map":
                    return true;
// below are old alpha names
                case "HHRI":
                 
                    return true;
                case "HHRI Mobile Beta":
                   
                    return true;
                case "Lynx Hand Held":
                   
                    return true;
                case "Lynx Manual Ops":
                 
                    return true;
                case "Manual Operation":
                 
                    return true;
                case "Manual Ops":
                   
                    return true;
         
                default:
                    
                    return false;
            }

        }

     // old dev url for access    private const string baseUrl2 = "http://api-dev.mytoronsn.com/api/irrmobileappaccess?username={0}";
        private const string baseUrl2Secure = "https://api.mytoronsn.com/api/irrmobileappaccess?username={0}";

        #endregion

        #region methods

        // object for the Toro NSN mobile app user authorization 
        public class RootObject
        {
            public bool IrrigationMobileAppAccessAllowed { get; set; }
            public string SignOnMessage { get; set; }
        }

        /// <summary>
        /// retrieves the premission to use apps and signon message of approriate 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userSignonMessage"></param>
        /// <returns></returns>
        public static bool GetUserPermission(string user, out string userSignonMessage)
        {
            userSignonMessage = string.Empty;
            bool userhasAuth = false;

            // The secret key is a value that both the client and server will know about. 
            // It will be hashed and act like a 'password'.
            string secretKey = "C4C9FDCCD62E42F99271EE6A0E4CEAB8";
            // The client Id identifies the app and acts like a username.
            string clientId = "IrrMobileAppAccess";
            // The secret key is hashed and sent to the server.
            string hash = string.Empty;

            // Hash the secret
            MD5CryptoServiceProvider cryptoServiceProvider = new MD5CryptoServiceProvider();
            DataContractSerializer serializer = new DataContractSerializer(typeof(string));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, secretKey);
                cryptoServiceProvider.ComputeHash(memoryStream.ToArray());
                hash = Convert.ToBase64String(cryptoServiceProvider.Hash);
            }

            // Create the time-bomb (epoch). The server will check to see if the request is stale and if so will deny access.
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;

            // The expected format is [ClientId]|[Epoch]|[Hash]
            string header = string.Format("{0}|{1}|{2}", clientId, secondsSinceEpoch, hash);

            // Call the API (with whichever method you choose)
            string result = string.Empty;

            // Add the header to the request. Must have a name of TTC_AUTH         

            var url = string.Format(baseUrl2Secure, user);



            var syncClient = new WebClient();
            // dont need proxy resoultion
            // causes slowness on first call
            //
            // http://stackoverflow.com/questions/4932541/c-sharp-webclient-acting-slow-the-first-time
            //
            syncClient.Proxy = null;
            // Add the header to the request. Must have a name of TTC_AUTH 
            syncClient.Headers.Add("TTC_AUTH", header);

            try
            {
                var content = syncClient.DownloadString(url);


                // Create the Json serializer and parse the response
                DataContractJsonSerializer userAccessSrializer = new DataContractJsonSerializer(typeof(RootObject));
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(content)))
                {
                    // deserialize the JSON object using the irrigation permission type.
                    var userdata = (RootObject)userAccessSrializer.ReadObject(ms);
                    // pass the desserialized json results to pararesults
                    userhasAuth = userdata.IrrigationMobileAppAccessAllowed;
                    userSignonMessage = userdata.SignOnMessage;

                }
            }
            catch (Exception e)
            {

                userSignonMessage = "Unable to authorize user - Call Toro NSN " + e.Message;
                userhasAuth = false;
            }

            return (userhasAuth);

        }



        #endregion
    }
}
