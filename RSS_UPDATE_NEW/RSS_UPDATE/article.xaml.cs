using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RSS_UPDATE.Resources;
using System.Threading;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.ServiceModel.Syndication;

using System.Windows.Media.Imaging;
using HtmlAgilityPack;
namespace RSS_UPDATE
{
    public partial class article : PhoneApplicationPage
    {
        public string article_link;

        public article(string Link)
        {
            article_link = Link;

            InitializeComponent();
            Loaded += article_Loaded;
        }
        

        public void article_Loaded(object sender, RoutedEventArgs e)
        {
           
          //  this.html.Text = "Please wait while the image url is loading";
           
            var req = WebRequest.Create(article_link);
        req.BeginGetResponse(r =>
        {
            var response = req.EndGetResponse(r);
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream, true);
            string str = reader.ReadToEnd();
            
            
            
            this.html.Dispatcher.BeginInvoke(() => { this.html.Text = str;
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

                        //this.text_description.Inlines.Add(div.InnerText);

                    }
                    catch (Exception q)
                    {
                        //MessageBox.Show(q.Message);
                    }


                }









            HtmlNodeCollection divs = new HtmlNodeCollection(htmlDocument.DocumentNode.ParentNode);
            divs = htmlDocument.DocumentNode.SelectNodes("//div[@id='morecontents']");
            if(divs!=null)
                foreach (HtmlNode div in divs)
            {


                try
                {

                  //  MessageBox.Show(div.InnerText);

                    this.text_description.Inlines.Add(div.InnerText);
                  
                }
                catch (Exception q)
                {
                    //MessageBox.Show(q.Message);
                }


            }








            

                /* 
                 * str --> contains the entire html content of our each news feed
                 * now we need to parse the html content in string using html agility pack
                 * you can use the htmlDocument(of type HtmlDocument ) to get the data
                 * there are several methods for accessing the div element inside a html document
                 * 
                     <div class ="new cont">
                 * <div class ="new cont1">
                 * <div class ="new cont2">
                 * <div class ="ins_wid990">
                 * <div class ="ins_lftcont640 clr" id="newDescriptionContainer">
                 * <div class ="ins_mainimage_big story_pic">
                 *<img itemprop="image" title="Delhi Law Minister Jitender Singh Tomar, Arrested for Allegedly Faking Degrees, Resigns" alt="Delhi Law Minister Jitender Singh Tomar, Arrested for Allegedly Faking Degrees, Resigns" id="story_image_main" src="http://i.ndtvimg.com/i/2015-06/jitender-singh-tomar_650x400_81433837847.jpg">
                 * 
                 * 
                We need to access the above image with its id  "story_image_main" so that we can display it in our news article
                 * http://www.ndtv.com/delhi-news/delhi-law-minister-jitender-singh-tomar-sent-to-4-day-police-custody-770177 --> here is the link for the article
                 * you can cross check the above structure by going to inspect element of this web page
                 * 
                 * 
                 * 
                 * 
            
                 */
            });
        }, null);




        }

        


       

        
    }

    }


