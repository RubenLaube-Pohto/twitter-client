using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TweetSharp;

namespace twitter_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetConnection();
        }

        private void GetConnection()
        {
            string consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            string consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
            // Pass your credentials to the service
            TwitterService service = new TwitterService(consumerKey, consumerSecret);

            // Step 1 - Retrieve an OAuth Request Token
            OAuthRequestToken requestToken = service.GetRequestToken();

            // Step 2 - Redirect to the OAuth Authorization URL
            Uri uri = service.GetAuthorizationUri(requestToken);
            Process.Start(uri.ToString());

            // Step 3 - Exchange the Request Token for an Access Token
            string verifier = "123456"; // <-- This is input into your application by your user
            OAuthAccessToken access = service.GetAccessToken(requestToken, verifier);

            // Step 4 - User authenticates using the Access Token
            service.AuthenticateWith(access.Token, access.TokenSecret);
            //IEnumerable<TwitterStatus> mentions = service.ListTweetsMentioningMe(ListTweetsMentioningMeOptions.Equals(;
        }
    }
}
