using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Start_Page.ViewModels
{
    public class Newsmodel
    {
        public Newspapers NDTV { get; set; }
        public Newspapers TOI { get; set; }
        public Newspapers ECONOMICTIMES { get; set; }
        public bool IsDataLoaded { get; set; }

        public void LoadData()
        {
            NDTV = createNDTVGroup();
            TOI = createTOIGroup();
            ECONOMICTIMES = createECONOMICTIMESGroup();


            IsDataLoaded = true;
        }

        private Newspapers createTOIGroup()
        {
            Newspapers data = new Newspapers();
            data.Title = "TOI";
            data.Items.Add(new Categories() { Title = "Breaking News", Link = "http://feeds.feedburner.com/NdtvNews-TopStories" });
            return data;

        }

        private Newspapers createECONOMICTIMESGroup()
        {
            Newspapers data = new Newspapers();
            data.Title = "ECONOMIC TIMES";
            data.Items.Add(new Categories() { Title = "Breaking News", Link = "http://feeds.feedburner.com/NdtvNews-TopStories" });
            return data;

        }

        private Newspapers createNDTVGroup()
        {
            Newspapers data = new Newspapers();
            data.Title = "NDTV";
            data.Items.Add(new Categories() { Title = "Top Stories", Link = "http://feeds.feedburner.com/NdtvNews-TopStories" });
            data.Items.Add(new Categories() { Title = "Latest News", Link = "http://feeds.feedburner.com/NDTV-LatestNews" });
            data.Items.Add(new Categories() { Title = "Trending News", Link = "http://feeds.feedburner.com/NDTV-Trending" });


            return data;

        }
    }
}
