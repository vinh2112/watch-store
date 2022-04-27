using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Do_An.Areas.Customer.helper
{
    public static class Configuration
    {
        //these variables will store the clientID and clientSecret
        //by reading them from the web.config
        public readonly static string ClientId;
        public readonly static string ClientSecret;
        public static string SDT, DiaChi, ChuThich, MaDH;
        public static int tong;
        static Configuration()
        {
            var config = GetConfig();
            ClientId = "AZjzRs6fuevjlUG2Baqu4mCkiPfgbsdpakO-OuNfdqE3GGK84KTmEjFdR5-RyTj3f0yC4dC9dsG5NbMC";
            ClientSecret = "EP8jjCcDCA_AVyV2_6glgl6PuSl8QJttLAsATVZIFefCVSbaQOsizjf4LdC--owWzH6PSl-ZrifeXUdG";
        }

        // getting properties from the web.config
        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            // getting accesstocken from paypal                
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();

            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}