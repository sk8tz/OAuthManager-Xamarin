using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth2Manager.Common;

namespace OAuth2Manager.Core.Methods.TwoLegged
{
    public class OAuth2ResourceOwnerPassword : OAuth2Base
    {
        private string userName;
        private string password;
        private string clientSecret;
        public OAuth2ResourceOwnerPassword(string clientId, string clientSecret, string redirectUrl, string scope, string authorizationUrl, string userName, string password) :
            base(clientId, redirectUrl, scope, authorizationUrl)
        {
            this.userName = userName;
            this.password = password;
            this.clientSecret = clientSecret;
        }

        public virtual async Task<bool> InvokeUserAuthorization()
        {
            //initialize the underlying OAuthorizer
            InitAuthorizer(clientSecret);

            base.OAuthState = OAuthState.INITIALIZED;
            var parameters = new List<KeyValuePair<string, string>>(capacity: 6)
                {
                    new KeyValuePair<string,string>(OAuthConstants.USERNAME, this.userName),
                    new KeyValuePair<string,string>(OAuthConstants.PASSWORD, this.password),
                    new KeyValuePair<string,string>(OAuthConstants.CLIENT_SECRET, this.clientSecret)
                };

            var accessTokenUrl = Authorizer.BuildAuthorizeUrl(AuthorizationUrl, OAuthConstants.GRANT_TYPE_PASSWORD, parameters);
            Uri authUri = new Uri(accessTokenUrl);

            OAuthState = OAuthState.ACCESS_TOKEN_WAIT;
            try
            {
                var result = await Authorizer.GetAccessTokenAsync(accessTokenUrl);

                if (result != null)
                {
                    OAuthState = OAuthState.SUCCEEDED;
                    AccessToken = result.Token;
                    return true;
                }
                else
                {
                    OAuthState = OAuthState.FAILED;
                    return false;
                }
            }
            catch (Exception)
            {
                OAuthState = OAuthState.FAILED;
                throw;
            }
        }
    }
}
