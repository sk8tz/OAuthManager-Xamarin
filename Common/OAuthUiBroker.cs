using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth2Manager.Controls;
using OAuth2Manager.Core.Methods.ThreeLegged;
using OAuth2Manager.Core.Methods.TwoLegged;
using Xamarin.Forms;

namespace OAuth2Manager.Common
{
    public class OAuthUiBroker
    {
        public static EventHandler<AuthenticatedEventArgs> OnAuthenticated;

        private static Page currentPage;
        public static void ShowOAuthView(OAuth2WebServerFlow oAuth)
        {
            currentPage = Application.Current.MainPage;

            var oAuthView = new FlexibleWebAuthView();
            oAuth.InvokeUserAuthorization(oAuthView);
            Application.Current.MainPage = oAuthView;
            oAuthView.Navigate(oAuthView.AuthorizeUrl);

            oAuthView.OAuthSuccess += async (sender, args) =>
            {
                await oAuth.ProcessAuthorizationAsync(sender as Uri);
                await Application.Current.MainPage.DisplayAlert("Access Token", oAuth.AccessToken.Code, "OK", "Cancel");
                RestoreView();
                OnAuthenticated?.Invoke(oAuth, new AuthenticatedEventArgs(oAuth.AccessToken.Code, oAuth.AccessToken.RefreshToken, oAuth.AccessToken.Expires));
            };
        }

        public static void ShowOAuthView(OAuth2BrowserBasedFlow oAuth)
        {
            currentPage = Application.Current.MainPage;
            var oAuthView = new FlexibleWebAuthView();
            oAuth.InvokeUserAuthorization(oAuthView);
            Application.Current.MainPage = oAuthView;
            oAuthView.Navigate(oAuthView.AuthorizeUrl);

            oAuthView.OAuthSuccess += async (sender, args) =>
            {
                await oAuth.ProcessAuthorizationAsync(sender as Uri);
                await Application.Current.MainPage.DisplayAlert("Access Token", oAuth.AccessToken.Code, "OK");
                RestoreView();
            };
        }

        public static void ShowOAuthView(OAuth2PinBasedFlow oAuth)
        {
            currentPage = Application.Current.MainPage;
            var contentDialog = new ContentPage();
            var stackPanel = new StackLayout() { Orientation = StackOrientation.Vertical };
            var urlTextBlock = new Label()
            {
                Text = "Open URL in Browser: " + oAuth.GetUserTokenUrl(),
                VerticalTextAlignment = TextAlignment.Center
            };
            var pinTextBox = new Entry { WidthRequest =150.0f };
            var okButton = new Button { Text = "OK" };
            okButton.Clicked += async (sender, args) =>
            {
                if (string.IsNullOrWhiteSpace(pinTextBox.Text))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please Enter Pin Code", "OK");
                }
                if (await oAuth.ProcessUserAuthorizationAsync(pinTextBox.Text))
                {
                    RestoreView();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid Pin", "OK");
                }
            };

            stackPanel.Children.Add(urlTextBlock);
            stackPanel.Children.Add(pinTextBox);
            stackPanel.Children.Add(okButton);

            contentDialog.Content = stackPanel;
            Application.Current.MainPage = contentDialog;
        }

        private static void RestoreView()
        {
            Application.Current.MainPage = currentPage;
        }
    }
}
