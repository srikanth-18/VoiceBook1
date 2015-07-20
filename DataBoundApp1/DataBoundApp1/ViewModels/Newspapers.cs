using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBoundApp1.ViewModels
{
   public class Newspapers
    {
          public List<Categories> Items { get; set; }
        public string Title { get; set; }
        public Newspapers() {

            Items = new List<Categories>();
        }
    }
}
