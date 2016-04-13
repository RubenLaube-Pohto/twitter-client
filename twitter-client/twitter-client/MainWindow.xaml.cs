using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
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
        private TwitterService service;

        public MainWindow(TwitterService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void GetTimeline()
        {
            ListTweetsOnHomeTimelineOptions options = new ListTweetsOnHomeTimelineOptions();
            //options.Count = 800; // 800 is max returned
            IEnumerable<TwitterStatus> tweets = service.ListTweetsOnHomeTimeline(options);
            dgTweets.DataContext = tweets;
        }

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
                Login login = new Login();
                login.Show();
                this.Close();
            }
        }

        private void btnGet_timeline_Click(object sender, RoutedEventArgs e)
        {
            GetTimeline();
        }

        private void btnNew_tweet_Click(object sender, RoutedEventArgs e)
        {
            ToggleNewTweet();
        }

        private void btnCancel_tweet_Click(object sender, RoutedEventArgs e)
        {
            ToggleNewTweet();
        }

        private void btnSend_tweet_Click(object sender, RoutedEventArgs e)
        {
            SendTweetOptions options = new SendTweetOptions();
            options.Status = txtTweet.Text;
            service.SendTweet(options);
            MessageBox.Show("Tweet sent!");
            txtTweet.Text = "";
            ToggleNewTweet();
        }

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
