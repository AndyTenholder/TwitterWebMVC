﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterWebMVC.Models
{
    public class Tweet
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }

        // foreign key
        public int LanguageID { get; set; }
        // Navigation prop
        public Language Language { get; set; }

        public IList<TweetHashtag> TweetHashtags { get; set; }
    }
}
