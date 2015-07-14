using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RSS_UPDATE.Resources;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.IO;
using System.Threading;

namespace RSS_UPDATE
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += loadFeedButton_Click;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void loadFeedButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Button clicked");
            string value = "http://feeds.feedburner.com/NdtvNews-TopStories";
            string fixedString;
            fixedString = Regex.Replace(value.ToString(), "<[^>]+>", string.Empty);

            // Remove newline characters.
            fixedString = fixedString.Replace("\r", "").Replace("\n", "");

            // Remove encoded HTML characters.
            fixedString = HttpUtility.HtmlDecode(fixedString);

            

            WebClient client = new WebClient();

            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);

            // Download the RSS feed. DownloadStringAsync was used instead of OpenStreamAsync because we do not need 
            // to leave a stream open, and we will not need to worry about closing the channel. 
            client.DownloadStringAsync(new System.Uri(fixedString), UriKind.Absolute);

        }


        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var RSSData = from rss in XElement.Parse(e.Result).Descendants("item")

                          select new RssItem
                          {

                              Title = rss.Element("title").Value,
                              PubDate = rss.Element("pubDate").Value,
                              Description = Regex.Replace(rss.Element("description").Value, "<.*?>", string.Empty),
                              Link = rss.Element("link").Value



                          };

            myList.ItemsSource = RSSData;

        }
        //private static ManualResetEvent allDone = new ManualResetEvent(false);
        //public ManualResetEvent waitHandle = new ManualResetEvent(false);
        //public ManualResetEvent mrse = new ManualResetEvent(false);
        
        string html = "";
        private void myList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
        
       

            ListBoxItem item = this.myList.ItemContainerGenerator.ContainerFromIndex(myList.SelectedIndex) as ListBoxItem;
            TextBlock tagregCheckBox = FindLastElementInVisualTree<TextBlock>(item);
            MessageBox.Show(tagregCheckBox.Text);

            article mynewPage = new article(tagregCheckBox.Text); //newPage is the name of the newPage.xaml file
            this.Content = mynewPage;
            //            NavigationService.Navigate(new Uri("/artcile.xaml?link="+tagregCheckBox.Text,UriKind.Relative));

            
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(tagregCheckBox.Text+"l"));
            IAsyncResult result = request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallback), request);
            
          
            MessageBox.Show("waiting for a signal");
            //MessageBox.Show(html);
            
            
        }

        public void ReadWebRequestCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
            try
            {
                MessageBox.Show("started");
                using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult))
                {
                    using (StreamReader httpwebStreamReader = new StreamReader(myResponse.GetResponseStream()))
                    {
                        string results = httpwebStreamReader.ReadToEnd();
                        //execute UI stuff on UI thread.


          
                        
                        //  MessageBox.Show("Finally got data"+val);
                        MessageBox.Show(results);



                    }
                }



            }
            catch { }

        }


        

        private T FindLastElementInVisualTree<T>(DependencyObject parentElement) where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(parentElement);
            if (count == 0)
                return null;

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parentElement, count - i - 1);

                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    var result = FindLastElementInVisualTree<T>(child);
                    if (result != null)
                        return result;

                }
            }
            return null;
        }



        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}