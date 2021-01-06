using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneShop.Models.PayPalConfig
{
    public class PayPalConfiguration
    {
        public readonly static string clientId;
        public readonly static string clientSecret;
        
        private static Dictionary<string,string> getConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        static PayPalConfiguration()
        {
            var config = getConfig();
            clientId = "AY6ZqhKatMIcABi31dsnaivtY8u2Qjk79rcc9v8c1nytpHwEunaN3bH08wKVa4m2oYt1npJIgDjI-hMf";
            clientSecret = "EH1BAGECH9mcTez0D3ODS-sSy_qJxtIqjwkE5tF_oDsvHkDOG-S5x-Ew3LlEtBpQEvMnAH4e7f6N0ULO";
        }
        public static string GetAccessTokenZ()
        {
            string acToken = new OAuthTokenCredential(clientId,clientSecret, getConfig()).GetAccessToken();
            return acToken;
        }
        public static APIContext GetAPIContextZ()
        {
            APIContext apiCon = new APIContext(GetAccessTokenZ());
            apiCon.Config = getConfig();
            return apiCon;
        }

    }
}