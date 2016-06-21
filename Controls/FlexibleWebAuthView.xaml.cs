using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth2Manager.Common;
using Xamarin.Forms;

namespace OAuth2Manager.Controls
{
    public partial class FlexibleWebAuthView : ContentPage, IUserAuthorizationViewer
    {
        public EventHandler OAuthCancelled { get; set; }
        public EventHandler OAuthSuccess { get; set; }
        public EventHandler NavigationFailed { get; set; }

        public FlexibleWebAuthView()
        {
            InitializeComponent();
            InitEvents();
        }

        private void InitEvents()
        {
            InternalWebView.Navigating += InternalWebView_Navigating;
            InternalWebView.Navigated += InternalWebView_Navigated;
        }

        private void InternalWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            _currentUri = new Uri(e.Url);
        }

        private void InternalWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (AuthController.IsCallback(new Uri(e.Url)))
                OAuthSuccess?.Invoke(new Uri(e.Url), EventArgs.Empty);
        }

        public void Navigate(Uri uri)
        {
            InternalWebView.Source = uri;
        }

        private Uri _currentUri;

        public Uri GetCurrentUri()
        {
            return _currentUri;
        }

        public Uri AuthorizeUrl { get; set; }
        public IUserConsentHandler AuthController { get; set; }
    }
}
