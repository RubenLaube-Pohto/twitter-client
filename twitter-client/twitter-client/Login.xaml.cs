using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
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
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("logindata.xml");
                XmlNode root = doc.FirstChild;
                string token = root.ChildNodes[0].InnerText;
                string tokenSecret = root.ChildNodes[1].InnerText;
                service.AuthenticateWith(token, tokenSecret);
                MoveToMain();
            }
            catch (FileNotFoundException ex)
            {
                // Step 1 - Retrieve an OAuth Request Token
                requestToken = service.GetRequestToken();

                // Step 2 - Redirect to the OAuth Authorization URL
                Uri uri = service.GetAuthorizationUri(requestToken);
                Process.Start(uri.ToString());

                spPin.Visibility = Visibility.Visible;
                btnLogin.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Step 3 - Exchange the Request Token for an Access Token
            string verifier = txtPin.Text; // <-- This is input into your application by your user
            OAuthAccessToken access = service.GetAccessToken(requestToken, verifier);

            // Step 4 - User authenticates using the Access Token
            // TODO: Save tokens for easier login later
            service.AuthenticateWith(access.Token, access.TokenSecret);

            MoveToMain();
        }

        private void MoveToMain()
        {
            MainWindow main = new MainWindow(service);
            main.Show();
            this.Close();
        }

        private void txtPin_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]{7}$");
            if (regex.IsMatch(txtPin.Text))
            {
                btnSubmit.IsEnabled = true;
            }
            else
            {
                btnSubmit.IsEnabled = false;
            }
        }
    }
}
