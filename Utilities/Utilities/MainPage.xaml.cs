using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Utilities.Resources;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Windows.Foundation;
using Windows.Phone.Speech.Synthesis;
using System.Windows.Media;
using Coding4Fun.Toolkit.Audio;
using System.IO.IsolatedStorage;
using System.IO;
using Coding4Fun.Toolkit.Audio.Helpers;
using Coding4Fun.Toolkit.Controls;
using SilverlightMessageBoxLibrary;

namespace Utilities
{


    public class SpeechUtility : IDisposable
    {
        protected SpeechSynthesizer synth;

        protected string text;

        protected string[] parts;

        protected int max;

        protected IAsyncAction speakTask;

        public int Count { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="SpeechUtility"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public SpeechUtility(string text)
        {
            synth = new SpeechSynthesizer();
            this.text = text;
            // MessageBox.Show(text);
            parts = this.text.Split(' '); // create a bookmark after each word
            max = this.parts.Length;
            //Speak(0);
            synth.BookmarkReached += BookmarkReached;
        }


        /// <summary>
        /// Speaks the text.
        /// </summary>
        /// <param name="start">The start count.</param>
        /// <returns></returns>
        public void Speak(int? start = null)
        {
            if (start.HasValue)
            {
                Count = start.Value;
                //MessageBox.Show(Count.ToString());
            }

            speakTask = synth.SpeakSsmlAsync(CreateSSML());
        }


        /// <summary>
        /// Pauses the speak task.
        /// </summary>
        /// <returns></returns>
        /// 

        public void Pause()
        {

            speakTask.Cancel();
            // MessageBox.Show("count=" + Count);
        }

        public void Resume()
        {
            //MessageBox.Show("Count=" + Count);
            Speak(Count);

        }

        public void Stop()
        {
            speakTask.Cancel();
            Count = 0;



        }

        int i = 0;
        /// <summary>
        /// Creates the SSML.
        /// </summary>
        /// <returns></returns>
        protected string CreateSSML()
        {
            // restart reading when the END was reached before
            if (Count == max)
            {
                Count = 0;
            }


            String ssmlText = "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\">";
            ssmlText += "<![CDATA[";

            for (i = Count; i < max; i++)
            {
                ssmlText += parts[i];
                ssmlText += "]]><mark name=\"PART" + i.ToString() + "\"/><![CDATA[";
            }
            ssmlText += "]]><mark name=\"END\"/>";
            ssmlText += "</speak>";
            //  MessageBox.Show(ssmlText);
            return ssmlText;
        }


        /// <summary>
        /// Reached a new bookmark.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SpeechBookmarkReachedEventArgs"/> instance containing the event data.</param>
        protected void BookmarkReached(object sender, SpeechBookmarkReachedEventArgs e)
        {
            Count++;

            if (e.Bookmark == "END")
            {
                Stop();
            }
        }



        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            synth.Dispose();
        }
    }
    public partial class MainPage : PhoneApplicationPage
    {
        SpeechUtility test = null;

        public MainPage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
           
            //text_box.IsEnabled = false;
            // text_box.Foreground = new SolidColorBrush(Colors.White);
            text_box.Style = (Style)App.Current.Resources["CopyTextBlock"];
            Play.IsEnabled = false;
            Pause.IsEnabled = false;
            Stop.IsEnabled = false;
            Record.IsEnabled = false;
        }




        private void Play_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            test.Resume();
            Play.IsEnabled = false;
            Pause.IsEnabled = true;
            Stop.IsEnabled = true;

        }

        private void Pause_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
            test.Pause();

            Pause.IsEnabled = false;
            Play.IsEnabled = true;

        }

        private void Stop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            test.Stop();

            Stop.IsEnabled = false;
            Pause.IsEnabled = false;


        }

        private void text_box_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Record.IsEnabled = true;
            int selected_index = text_box.SelectionStart;
            int number_of_spaces = 0;
            for (int i = selected_index; i > 0; i--)
            {

                if (text_box.Text[i] == ' ')
                {
                    number_of_spaces++;

                }

            }

            

            
            test = new SpeechUtility(text_box.Text);
            Play.IsEnabled = false;
            Pause.IsEnabled = true;
            Stop.IsEnabled = true;
            test.Speak(number_of_spaces);

        }
        
        private IsolatedStorageFileStream _audioStream;
        private MicrophoneRecorder _recorder = new MicrophoneRecorder();


        private void RecordAudioChecked(object sender, RoutedEventArgs e)
        {
            
            _recorder.Start();

        }

        private void RecordAudioUnChecked(object sender, RoutedEventArgs e)
        {
            _recorder.Stop();
            ApplicationBar.IsVisible = true;
            SaveTempAudio(_recorder.Buffer);

        }
        private string tempfilename = "temp.wav";
        private void SaveTempAudio(MemoryStream buffer)
        {
            if (_audioStream != null)
            {
                AudioPlayer.Stop();
                AudioPlayer.Source = null;
                _audioStream.Dispose();

            }
            if (buffer == null)
                throw new ArgumentNullException("Attempting to save an empty sound buffer...!!!!");

            var bytes = buffer.GetWavAsByteArray(_recorder.SampleRate);
            using (IsolatedStorageFile isostore = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (isostore.FileExists(tempfilename))
                    isostore.DeleteFile(tempfilename);

                tempfilename = string.Format("{0}.wav", DateTime.Now.ToFileTime());
                _audioStream = isostore.CreateFile(tempfilename);
                _audioStream.Write(bytes, 0, bytes.Length);

                AudioPlayer.SetSource(_audioStream);

            }
        }

        void recordAudioAppBar_Click(object sender, EventArgs e)
        {
            string text_entered = "test";
            //MessageBox.Show("clicked on record audio app bar");
            MessageBox.Show("About to play the temporary recorded file");
            AudioPlayer.Play();
            MessageBox.Show("done playing");
            InputPrompt fileName = new InputPrompt();
            fileName.Background = new SolidColorBrush(Colors.Blue);
            fileName.Title = "Input title";
            fileName.Message = "Give a title for your note";
            fileName.Completed += fileName_Completed;
            fileName.Show();



          
        }

        private void fileName_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {

            if (e.PopUpResult == PopUpResult.Ok)
            { 
            
            
            }

        }

        

        

            // Sample code for building a localized ApplicationBar
            private void BuildLocalizedApplicationBar()
            {
                // Set the page's ApplicationBar to a new instance of ApplicationBar.
                ApplicationBar = new ApplicationBar();
                ApplicationBar.IsVisible = false;
                // Create a new button and set the text value to the localized string from AppResources.
                ApplicationBarIconButton recordAudioAppBar = new ApplicationBarIconButton(new Uri("/Assets/Appbar/save.png", UriKind.Relative));
                recordAudioAppBar.Text = AppResources.AppBarButtonText;
                recordAudioAppBar.Click += recordAudioAppBar_Click;
                ApplicationBar.Buttons.Add(recordAudioAppBar);

                // Create a new menu item with the localized string from AppResources.
                ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
                ApplicationBar.MenuItems.Add(appBarMenuItem);
            }


        }
    }
