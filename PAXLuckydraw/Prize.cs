using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAXLuckydraw
{
    public class Prize
    {
        public int id
        {
            get; set;
        }
        public Image VendorLogo { get; set;}
        public Image PrizePicture { get; set; }
        public String VendorName { get; set; }
        public String PrizeDescription { get; set; }


    }
}
