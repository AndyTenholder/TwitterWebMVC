using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterWebMVC.Models;

namespace TwitterWebMVC.Comparers
{
    public class LanguageComparer : IComparer<Language>
    {
        public int Compare(Language x, Language y)
        {
            if (x.TimesUsed < y.TimesUsed)
            {
                return 1;
            }
            else if (x.TimesUsed > y.TimesUsed)
            {
                return -1;
            }
            return 0;
        }
    }
}