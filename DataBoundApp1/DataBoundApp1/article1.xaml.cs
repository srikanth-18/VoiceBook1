using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Phone.Speech.Synthesis;
using Windows.Foundation;
using HtmlAgilityPack;
using System.Windows.Media.Imaging;
using System.IO;
using System.IO.IsolatedStorage;
using Coding4Fun.Toolkit.Audio;

namespace DataBoundApp1
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
    public partial class article1 : PhoneApplicationPage
    {
        SpeechUtility test = null;
        public article1()
        {
            InitializeComponent();
            Loaded += article_Loaded;
            // BuildLocalizedApplicationBar();
            text_description.Style = (Style)App.Current.Resources["CopyTextBlock"];
     
        }

        private void article_Loaded(object sender, RoutedEventArgs e)
        {

            var req = WebRequest.Create(article_link);
            req.BeginGetResponse(r =>
            {
                var response = req.EndGetResponse(r);
                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream, true);
                string str = reader.ReadToEnd();



                this.html.Dispatcher.BeginInvoke(() =>
                {
                    this.html.Text = str;
                    //MessageBox.Show(str);
                    //str = "<div class='Hai'> <div class='inner hai'/> </div>";                  



                    WebClient webclient = new WebClient();

                    HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                    MessageBox.Show(str);

                    htmlDocument.LoadHtml(str);
                    HtmlNodeCollection imgs = new HtmlNodeCollection(htmlDocument.DocumentNode.ParentNode);
                    imgs = htmlDocument.DocumentNode.SelectNodes("//meta[@property='og:image']");

                    foreach (HtmlNode img in imgs)
                    {

                        try
                        {
                            this.ImageAppLogo.Source = new BitmapImage(new Uri(img.Attributes["content"].Value, UriKind.Absolute));

                        }
                        catch (Exception q)
                        {
                            MessageBox.Show(q.Message);
                        }


                        //urls.Add(src.Value);
                    }

                    HtmlNodeCollection metas = new HtmlNodeCollection(htmlDocument.DocumentNode.ParentNode);
                    metas = htmlDocument.DocumentNode.SelectNodes("//meta[@name='description']");
                    foreach (HtmlNode meta in metas)
                    {


                        try
                        {

                            this.text_description.Text = meta.Attributes["content"].Value;


                            //   MessageBox.Show(meta.Attributes["content"].Value);
                        }
                        catch (Exception q)
                        {
                            //MessageBox.Show(q.Message);
                        }


                    }


                    HtmlNodeCollection texts = new HtmlNodeCollection(htmlDocument.DocumentNode.ParentNode);
                    texts = htmlDocument.DocumentNode.SelectNodes("//div[(@class='stry-para')]");
                    if (texts != null)
                        foreach (HtmlNode text in texts)
                        {


                            try
                            {
                                MessageBox.Show("inside stry-para tag");

                                //  MessageBox.Show(div.InnerText);
                                this.text_description.Text += text.InnerText;
                                //this.text_description += text.InnerText;
                                //this.text_description.Inlines.Add(div.InnerText);

                            }
                            catch (Exception q)
                            {
                                //MessageBox.Show(q.Message);
                            }


                        }

                    HtmlNodeCollection divs = new HtmlNodeCollection(htmlDocument.DocumentNode.ParentNode);
                    divs = htmlDocument.DocumentNode.SelectNodes("//div[@id='morecontents']");
                    if (divs != null)
                        foreach (HtmlNode div in divs)
                        {


                            try
                            {

                                //  MessageBox.Show(div.InnerText);

                                //this.text_description.Inlines.Add(div.InnerText);
                                this.text_description.Text += div.InnerText;

                            }
                            catch (Exception q)
                            {
                                //MessageBox.Show(q.Message);
                            }


                        }


                });
            }, null);




        }

        private void text_box_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("Taped textbox");
            stop.IsEnabled = true;
            pause.IsEnabled = true;
            int selected_index = text_description.SelectionStart;
            int number_of_spaces = 0;
            for (int i = selected_index; i > 0; i--)
            {

                if (text_description.Text[i] == ' ')
                {
                    number_of_spaces++;

                }

            }

            test = new SpeechUtility(text_description.Text);
            test.Speak(number_of_spaces);
            BuildLocalizedApplicationBar();
    
        }

        string article_link = "";
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string parameter = string.Empty;
            if (NavigationContext.QueryString.TryGetValue("link", out parameter))
            {
                article_link = parameter;
            }

        }
        ApplicationBarIconButton pause = new ApplicationBarIconButton();
        ApplicationBarIconButton record = new ApplicationBarIconButton();
        ApplicationBarIconButton stop = new ApplicationBarIconButton();
        public void BuildLocalizedApplicationBar()
        {

            ApplicationBar appBar = new ApplicationBar();
            appBar.IsMenuEnabled = true;
            /*appBar.Buttons.Add(new ApplicationBarIconButton()
            {
                Text = "Record",
                IconUri = new Uri("/Assets/Appbar/microphone.png", UriKind.Relative)
            });
           */


            pause.IconUri = new Uri("/Assets/Appbar/button_black_pause.png", UriKind.Relative);
            pause.Text = "Pause";
            appBar.Buttons.Add(pause);
            pause.Click += pause_Click;


            stop.IconUri = new Uri("/Assets/Appbar/stop.png", UriKind.Relative);
            stop.Text = "Stop";
            appBar.Buttons.Add(stop);
            stop.Click += stop_Click;




            this.ApplicationBar = appBar;
        }

        void stop_Click(object sender, System.EventArgs e)
        {
            test.Stop();

            stop.IsEnabled = false;
            pause.IsEnabled = false;

        }

        void pause_Click(object sender, System.EventArgs e)
        {
            ApplicationBarIconButton btn = (ApplicationBarIconButton)ApplicationBar.Buttons[0];

            if (btn.Text == "Play")
            {
                btn.Text = "Pause";
                btn.IconUri = new Uri("Assets/Appbar/button_black_pause.png", UriKind.Relative);
            }
            else if (btn.Text == "Pause")
            {
                btn.Text = "Play";
                btn.IconUri = new Uri("Assets/Appbar/button_play.png", UriKind.Relative);
            }
            test.Pause();

            pause.IsEnabled = false;


        }
        private IsolatedStorageFileStream _audioStream;
        private MicrophoneRecorder _recorder = new MicrophoneRecorder();



        private void RecordAudioChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            _recorder.Start();
        }

        private void RecordAudioUnChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            _recorder.Stop();


        }

    }
}