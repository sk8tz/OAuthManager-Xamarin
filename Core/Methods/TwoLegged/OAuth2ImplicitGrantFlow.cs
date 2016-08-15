using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth2Manager.Common;
using OAuth2Manager.Extensions;

namespace OAuth2Manager.Core.Methods.TwoLegged
{
    public class OAuth2ImplicitGrantFlow : OAuth2Base, IUserConsentHandler
    {
        public OAuth2ImplicitGrantFlow(string clientId, string redirectUrl, string scope, string authorizationUrl) :
            base(clientId, redirectUrl, scope, authorizationUrl)
        {

        }

        public virtual Uri GetUserTokenUrl()
        {
            //initialize the underlying OAuthorizer
            InitAuthorizer();

            base.OAuthState = OAuthState.AUTH_TOKEN_WAIT;
            var authorizeUrlResponse = Authorizer.BuildAuthorizeUrl(AuthorizationUrl, OAuthConstants.RESPONSE_TYPE_TOKEN);
            return new Uri(authorizeUrlResponse);
        }

        public virtual void InvokeUserAuthorization(IUserAuthorizationViewer viewer)
        {
            //initialize the underlying OAuthorizer
            InitAuthorizer();

            base.OAuthState = OAuthState.AUTH_TOKEN_WAIT;
            var authorizeUrlResponse = Authorizer.BuildAuthorizeUrl(AuthorizationUrl, OAuthConstants.RESPONSE_TYPE_TOKEN);
            viewer.AuthController = this;
            viewer.AuthorizeUrl = new Uri(authorizeUrlResponse);
        }

        public virtual bool IsCallback(Uri currentUri)
        {
            if (RedirectUrl.Equals(OAuthConstants.OUT_OF_BOUNDS))
                return false;

            Uri callBackUrl = new Uri(RedirectUrl);
            var components = (UriComponents.SchemeAndServer | UriComponents.Path);

            string value1 = currentUri.GetComponents(components, UriFormat.Unescaped);
            string value2 = callBackUrl.GetComponents(components, UriFormat.Unescaped);
            return string.Equals(value1, value2, StringComparison.Ordinal);
        }

        public virtual async Task<bool> ProcessAuthorizationAsync(Uri currentUri)
        {
            try
            {
                var authorizedUrl = currentUri.ProcessIEUrlErrors();
                var result = Authorizer.GetAccessTokenFromResponse(authorizedUrl);

                if (result != null)
                {
                    OAuthState = OAuthState.SUCCEEDED;
                    AccessToken = result.Token;
                    return true;
                }
                OAuthState = OAuthState.FAILED;
                return false;
            }
            catch (Exception)
            {
                OAuthState = OAuthState.FAILED;
                throw;
            }
        }
    }
}
