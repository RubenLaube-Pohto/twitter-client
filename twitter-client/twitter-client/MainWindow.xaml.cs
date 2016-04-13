/*
 * This file is part of the Windows Programming end of course project.
 *
 * Modified: 13.04.2016
 * Authors: Ruben Laube-Pohto
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TweetSharp;

namespace twitter_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// Contains most of the programs functionality.
    /// </summary>
    public partial class MainWindow : Window
    {
        private TwitterService service;

        public MainWindow(TwitterService service)
        {
            InitializeComponent();
            this.service = service;
        }

        /// <summary>
        /// Logout the user by deleting creditentials from the disk
        /// </summary>
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.Delete(ConfigurationManager.AppSettings["LoginDataFile"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Display login-window
                Login login = new Login();
                login.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Get user's home timeline
        /// </summary>
        private void btnGet_timeline_Click(object sender, RoutedEventArgs e)
        {
            ListTweetsOnHomeTimelineOptions options = new ListTweetsOnHomeTimelineOptions();
            IEnumerable<TwitterStatus> tweets = service.ListTweetsOnHomeTimeline(options);
            dgTweets.DataContext = tweets;
        }

        /// <summary>
        /// Open 'new tweet'-form
        /// </summary>
        private void btnNew_tweet_Click(object sender, RoutedEventArgs e)
        {
            ToggleNewTweet();
        }

        /// <summary>
        /// Close 'new tweet'-form
        /// </summary>
        private void btnCancel_tweet_Click(object sender, RoutedEventArgs e)
        {
            ToggleNewTweet();
        }

        /// <summary>
        /// Send new tweet / status update
        /// </summary>
        private void btnSend_tweet_Click(object sender, RoutedEventArgs e)
        {
            SendTweetOptions options = new SendTweetOptions();
            options.Status = txtTweet.Text;
            service.SendTweet(options);
            MessageBox.Show("Tweet sent!");
            txtTweet.Text = "";
            ToggleNewTweet();
        }

        /// <summary>
        /// Display user's own tweets in the datagrid
        /// </summary>
        private void btnGet_my_tweets_Click(object sender, RoutedEventArgs e)
        {
            ListTweetsOnUserTimelineOptions options = new ListTweetsOnUserTimelineOptions();
            IEnumerable<TwitterStatus> tweets = service.ListTweetsOnUserTimeline(options);
            dgTweets.DataContext = tweets;
        }

        /// <summary>
        /// On input to txtTweet check that the length of the tweet is
        /// 140 characters or less. This is not an accurate method
        /// and the check should propably be more complex.
        /// </summary>
        private void txtTweet_TextChanged(object sender, TextChangedEventArgs e)
        {
            const int MAX_CHARS = 140;

            int charCount = txtTweet.Text.Length;
            tbCharacter_count.Text = string.Format("{0} / {1}", charCount, MAX_CHARS);

            if (charCount > MAX_CHARS)
                btnSend_tweet.IsEnabled = false;
            else
                btnSend_tweet.IsEnabled = true;
        }

        /// <summary>
        /// Switch 'new tweet'-form on and off
        /// </summary>
        private void ToggleNewTweet()
        {
            if (btnNew_tweet.IsEnabled)
                btnNew_tweet.IsEnabled = false;
            else
                btnNew_tweet.IsEnabled = true;

            if (spSend_tweet.Visibility == Visibility.Collapsed)
                spSend_tweet.Visibility = Visibility.Visible;
            else
                spSend_tweet.Visibility = Visibility.Collapsed;
        }
    }
}
