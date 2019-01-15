using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Services
{
    public class ServicesMiddlewareHelpers
    {
        public Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        private string GetAccessToken(string clientId, string clientSecret)
        {
            string accessToken = new OAuthTokenCredential(clientId, clientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }

        public APIContext GetAPIContext(string clientId, string clientSecret)
        {
            APIContext apiContext = new APIContext(GetAccessToken(clientId, clientSecret));
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}
