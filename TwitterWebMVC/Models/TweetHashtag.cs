using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterWebMVC.Models
{
    public class TweetHashtag
    {
        public int TweetID { get; set; }
        public Tweet Tweet { get; set; }

        public int HashtagID { get; set; }
        public Hashtag Hashtag { get; set; }

        public TweetHashtag() { }
    }
}
