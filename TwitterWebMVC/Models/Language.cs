using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterWebMVC.Models
{
    public class Language
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TimesUsed { get; set; }

        public IList<Tweet> Tweets { get; set; }


    }
}
