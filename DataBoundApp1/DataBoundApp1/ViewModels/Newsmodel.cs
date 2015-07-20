using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBoundApp1.ViewModels
{
    public class Newsmodel
    {
        public Newspapers NDTV { get; set; }
        public Newspapers TOI { get; set; }
        public Newspapers HINDHU { get; set; }
        
        public bool IsDataLoaded { get; set; }

        

        public void LoadData()
        {
            ///LOad data in the model
           NDTV = createNDTVGroup();
            TOI = createTOIGroup();
            HINDHU = createHINDHUGroup();
            IsDataLoaded = true;
        }

        private Newspapers createNDTVGroup()
        {
            Newspapers data = new Newspapers();
            data.Title = "NDTV";
            data.Items.Add(new Categories() { Title = "NDTV Top News", Path = "http://feeds.feedburner.com/NdtvNews-TopStories" });
            data.Items.Add(new Categories() { Title = "NDTV Latest News", Path = "http://feeds.feedburner.com/NDTV-LatestNews" });
            data.Items.Add(new Categories() { Title = "NDTV Trending News", Path = "http://feeds.feedburner.com/NDTV-Trending" });
            return data;
        }

        private Newspapers createHINDHUGroup()
        {

            Newspapers data = new Newspapers();
            data.Title = "HINDHU";
            data.Items.Add(new Categories() { Title = " HINDHU Top News", Path = "link for THE HINDHU Top News" });
            data.Items.Add(new Categories() { Title = " HINDHU Latest News", Path = "link for THE HINDHU  Latest News" });
            data.Items.Add(new Categories() { Title = " HINDHU Trending News", Path = "link for THE HINDHU Trending News" });
            return data;
        }

        private Newspapers createTOIGroup()
        {
            Newspapers data = new Newspapers();
            data.Title = "TOI";
            data.Items.Add(new Categories() { Title = " TOI Top News", Path = "http://dynamic.feedsportal.com/pf/555218/http://toi.timesofindia.indiatimes.com/rssfeedstopstories.cms" });
            data.Items.Add(new Categories() { Title = " TOI Latest News", Path = "http://timesofindia.feedsportal.com/c/33039/f/533965/index.rss" });
            data.Items.Add(new Categories() { Title = " TOI Trending News", Path = "link for TOI Trending News" });
            return data;
        }


        
        
    }
}
