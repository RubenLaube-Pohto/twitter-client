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
        private TwitterService service;

        public MainWindow(TwitterService service)
        {
            InitializeComponent();
            this.service = service;
            GetTimeline();
        }

        private void GetTimeline()
        {
            ListTweetsOnHomeTimelineOptions options = new ListTweetsOnHomeTimelineOptions();
            dgTweets.DataContext = service.ListTweetsOnHomeTimeline(options);
        }
    }
}
