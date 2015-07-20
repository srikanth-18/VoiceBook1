using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DataBoundApp1.Resources;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Windows.Media;
using System.IO;

namespace DataBoundApp1
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        // Constructor
        public DetailsPage()
        {
            InitializeComponent();
            Loaded += loadFeedButton_Click;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }
        string link_string = string.Empty;
        private void loadFeedButton_Click(object sender, RoutedEventArgs e)
        {
            //string value = "http://feeds.feedburner.com/NdtvNews-TopStories";
            string value = link_string;
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
            string comparing = "http://";
            if (fixedString.Contains(comparing))
                client.DownloadStringAsync(new System.Uri(fixedString), UriKind.Absolute);
            else
            {
                MessageBox.Show("Sorry this is not implemented yet ");
                this.NavigationService.Navigate(new Uri("/MainPage.xaml" , UriKind.Relative));
                
            }
                

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
        string html = "";
        

        
        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                string selectedIndex = "";
                if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
                {
                    /*link_string = selectedIndex.ToString();
                    MessageBox.Show(link_string);*/
                    //MessageBox.Show(selectedIndex as DataBoundApp1.ViewModels.Categories);
                    link_string = selectedIndex;
 //                   Link_box.Text = link_string;
                    /*int index = int.Parse(selectedIndex);
                    DataContext = App.ViewModel.Items[index];*/
                }
            }
        }

        private void myList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {


            ListBoxItem item = this.myList.ItemContainerGenerator.ContainerFromIndex(myList.SelectedIndex) as ListBoxItem;
            TextBlock tagregCheckBox = FindLastElementInVisualTree<TextBlock>(item);
            //MessageBox.Show(tagregCheckBox.Text);

            //article mynewPage = new article(tagregCheckBox.Text); //newPage is the name of the newPage.xaml file
            //this.Content = mynewPage;

            //            NavigationService.Navigate(new Uri("/artcile.xaml?link="+tagregCheckBox.Text,UriKind.Relative));
            this.NavigationService.Navigate(new Uri("/article1.xaml?link=" + tagregCheckBox.Text, UriKind.Relative));

            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(tagregCheckBox.Text+"l"));
            //IAsyncResult result = request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallback), request);


            //MessageBox.Show("waiting for a signal");
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
   
    }
}