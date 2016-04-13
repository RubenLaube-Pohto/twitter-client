/*
 * This file is part of the Windows Programming end of course project.
 *
 * Modified: 13.04.2016
 * Authors: Ruben Laube-Pohto
 */

using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using TweetSharp;

namespace twitter_client
{
    /// <summary>
    /// Interaction logic for Login.xaml.
    /// Handles the login process.
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

        /// <summary>
        /// Load keys from configuration file and initialize TwitterService
        /// </summary>
        private void Init()
        {
            spPin.Visibility = Visibility.Hidden;
            string consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            string consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
            service = new TwitterService(consumerKey, consumerSecret);
        }

        /// <summary>
        /// Begin login process.
        /// </summary>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Try to see if the user has already logged in
            try
            {
                // Look for the file containing previous tokens
                XmlDocument doc = new XmlDocument();
                doc.Load(ConfigurationManager.AppSettings["LoginDataFile"]);
                XmlNode root = doc.LastChild;
                string token = root.FirstChild.InnerText;
                string tokenSecret = root.LastChild.InnerText;
                service.AuthenticateWith(token, tokenSecret);
                MoveToMain();
            }
            // Login file not found so begin online authorization via browser.
            // Login process continues by inputing the pin recived later.
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
            // Something went wrong
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Exit application
        /// </summary>
        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Confirm pin input
        /// </summary>
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

        /// <summary>
        /// Close login and open main-window
        /// </summary>
        private void MoveToMain()
        {
            MainWindow main = new MainWindow(service);
            main.Show();
            this.Close();
        }

        /// <summary>
        /// Check that the pin input is valid
        /// </summary>
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
