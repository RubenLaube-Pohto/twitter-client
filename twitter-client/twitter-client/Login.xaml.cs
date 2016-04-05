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
                doc.Load(ConfigurationManager.AppSettings["LoginDataFile"]);
                XmlNode root = doc.LastChild;
                string token = root.FirstChild.InnerText;
                string tokenSecret = root.LastChild.InnerText;
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
            service.AuthenticateWith(access.Token, access.TokenSecret);

            // Save tokens to file for faster login the next time
            using (XmlWriter writer = XmlWriter.Create(ConfigurationManager.AppSettings["LoginDataFile"]))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("logindata");
                writer.WriteStartElement("token");
                writer.WriteString(access.Token);
                writer.WriteEndElement();
                writer.WriteStartElement("tokenSecret");
                writer.WriteString(access.TokenSecret);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

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
            // Check that the pin is only numbers and of the right length
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
