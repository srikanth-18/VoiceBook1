using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Start_Page.Resources;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Streams;
using System.IO;

namespace Start_Page
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            PhoneApplicationService.Current.ContractActivated += Application_ContractActivated;
            // Sample code to localize the ApplicationBar

            //BuildLocalizedApplicationBar();
        }
        public Windows.ApplicationModel.Activation.FileOpenPickerContinuationEventArgs FilePickerContinuationArgs { get; set; }
        private void Application_ContractActivated(object sender, Windows.ApplicationModel.Activation.IActivatedEventArgs e)
        {
            var filePickerContinuationArgs = e as FileOpenPickerContinuationEventArgs;
            if (filePickerContinuationArgs != null)
            {
                this.FilePickerContinuationArgs = filePickerContinuationArgs;
            }
            MessageBox.Show("Application contract activated");
        }

       
        private void book_Click(object sender, RoutedEventArgs e)
        {
            /*var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".pdf");
            picker.ContinuationData["Operation"] = "UpdateProfilePicture";*/
            //picker.PickSingleFileAndContinue();

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".doc");
            openPicker.FileTypeFilter.Add(".docx");
            openPicker.ContinuationData["Operation"] = "UpdateProfilePicture";
            // Launch file open picker and caller app is suspended and may be terminated if required
            openPicker.PickSingleFileAndContinue();
            //System.Windows.Controls.OpenFileDialog dialog = new System.Windows.Controls.OpenFileDialog();
            //System.Windows.Forms dlg = new OpenFileDialog();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var app = App.Current as App;
            if (this.FilePickerContinuationArgs != null)
            {
                this.ContinueFileOpenPicker(this.FilePickerContinuationArgs);
            }
        }

        public async void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        {
            if ((args.ContinuationData["Operation"] as string) == "UpdateProfilePicture" &&
                args.Files != null &&
                args.Files.Count > 0)
            {
                StorageFile file = args.Files[0];

                if (file.Name.EndsWith(""))
                {
                    IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    /*BitmapImage bitmapImage = new BitmapImage();

                    bitmapImage.SetSource(fileStream.AsStream());
                    ProfilePic.Source = bitmapImage;*/
                    textbox.Text = fileStream.ToString();
                    byte[] buffer = new byte[500];
                    //var read = fileStream.ReadAsync(buffer, 0, 500);
                    /*byte[] result;
                    UnicodeEncoding uniencoding = new UnicodeEncoding();
                    using (FileStream SourceStream = File.Open(file.Name, FileMode.Open))
                    {
                        result = new byte[SourceStream.Length];
                        await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);
                    }

                    
                    MessageBox.Show(result.ToString());*/
                    //System.IO.Stream inputStream = fileStream.AsStream();
                }
            }
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