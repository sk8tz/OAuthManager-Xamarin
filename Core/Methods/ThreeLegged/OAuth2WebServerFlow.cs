using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth2Manager.Common;
using OAuth2Manager.Extensions;

namespace OAuth2Manager.Core.Methods.ThreeLegged
{
    public class OAuth2WebServerFlow : OAuth2ThreeLeggedBase, IUserConsentHandler
    {
        public OAuth2WebServerFlow(string clientId, string clientSecret, string redirectUrl, string scope, string authorizationUrl, string accessTokenUrl) :
            base(clientId, clientSecret, redirectUrl, scope, authorizationUrl, accessTokenUrl)
        {
        }

        public virtual void InvokeUserAuthorization(IUserAuthorizationViewer viewer)
        {
            //initialize the underlying OAuthorizer
            InitAuthorizer(clientSecret);

            base.OAuthState = OAuthState.AUTH_TOKEN_WAIT;
            var authorizeUrlResponse = Authorizer.BuildAuthorizeUrl(AuthorizationUrl, OAuthConstants.RESPONSE_TYPE_CODE);
            viewer.AuthController = this;
            viewer.AuthorizeUrl = new Uri(authorizeUrlResponse);
        }

        public bool IsCallback(Uri currentUri)
        {
            var processedUrl = currentUri.ProcessIEUrlErrors();
            if (RedirectUrl.Equals(OAuthConstants.OUT_OF_BOUNDS))
                return false;

            string value1 = processedUrl.ToString().ToLowerInvariant();
            string value2 = RedirectUrl.ToString().ToLowerInvariant();
            return value1.StartsWith(value2);
        }

        public async Task<bool> ProcessAuthorizationAsync(Uri currentUri)
        {
            var authorizedUrl = currentUri.ProcessIEUrlErrors();
            OAuthState = OAuthState.AUTH_TOKEN_WAIT;
            var output = Authorizer.GetAuthTokenFromResponse(authorizedUrl);
            AuthToken = output.Token;

            OAuthState = OAuthState.ACCESS_TOKEN_WAIT;
            try
            {
                var result = await Authorizer.GetAccessTokenAsync(accessTokenUrl, AuthToken, OAuthConstants.GRANT_TYPE_AUTH_CODE);

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
            catch (Exception ex)
            {
                OAuthState = OAuthState.FAILED;
                throw;
            }
        }

    }
}
