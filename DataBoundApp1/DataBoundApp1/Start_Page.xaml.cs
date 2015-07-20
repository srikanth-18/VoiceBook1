using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Storage.Pickers;
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
using Windows.Storage.Pickers;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Streams;
using System.IO;

namespace DataBoundApp1
{
    public partial class Start_Page : PhoneApplicationPage
    {
        public Start_Page()
        {
            InitializeComponent();
        //    PhoneApplicationService.Current.ContractActivated += Application_ContractActivated;
        }
        
       

        private void news_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new System.Uri("/MainPage.xaml", UriKind.Relative));            
        }

    }
}