using System.Collections.Generic;

namespace TwitterWebMVC.Models
{
    public class Hashtag
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TimesUsed { get; set; }

        public IList<TweetHashtag> TweetHashtags { get; set; }
    }
}
