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
using System.Windows.Shapes;
using TweetSharp;

namespace twitter_client
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private TwitterService service;
        private OAuthRequestToken requestToken;

        public Login()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            spPin.Visibility = Visibility.Hidden;
            string consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            string consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
            service = new TwitterService(consumerKey, consumerSecret);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Step 1 - Retrieve an OAuth Request Token
            requestToken = service.GetRequestToken();

            // Step 2 - Redirect to the OAuth Authorization URL
            Uri uri = service.GetAuthorizationUri(requestToken);
            Process.Start(uri.ToString());

            spPin.Visibility = Visibility.Visible;
            btnLogin.IsEnabled = false;
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Step 3 - Exchange the Request Token for an Access Token
            // TODO: Check input length and that it's all numbers (regex?)
            string verifier = txtPin.Text; // <-- This is input into your application by your user
            OAuthAccessToken access = service.GetAccessToken(requestToken, verifier);

            // Step 4 - User authenticates using the Access Token
            // TODO: Save tokens for easier login later
            service.AuthenticateWith(access.Token, access.TokenSecret);

            MainWindow main = new MainWindow(service);
            main.Show();
            this.Close();
        }
    }
}
