using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostCodeWebApplication.Models
{
    public class PostItModel
    {
        public string post_code { get; set; }
        public string address { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string only_number { get; set; }
        public string housing { get; set; }
        public string city { get; set; }
        public string municipality { get; set; }
        public string post { get; set; }
        public string mailbox { get; set; }
    }
}
