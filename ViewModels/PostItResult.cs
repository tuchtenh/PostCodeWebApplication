using System.Collections.Generic;

namespace PostCodeWebApplication.Models
{
    public class PostItResult
    {
        public string status { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public int message_code { get; set; }
        public int total { get; set; }
        public List<PostItModel> data { get; set; }

    }
}
