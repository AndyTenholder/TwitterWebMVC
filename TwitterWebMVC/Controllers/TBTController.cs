using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TwitterWebMVC.Models;
using TwitterWebMVC.Data;
using TwitterWebMVC.ViewModels;
using TwitterWebMVC.Comparers;

namespace TwitterWebMVC.Controllers
{
    public class TBTController : Controller
    {
        private TweetDbContext context;

        public TBTController(TweetDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            // Get Total Number of #TBT tweets
            List<Tweet> tweets = context.Tweets.ToList();
            int totalTweets = tweets.Count;

            // Get total number of languages
            List<Language> languages = context.Languages.ToList();
            int totalLanguages = languages.Count;

            // Sort languages by TimesUsed
            languages.Sort(new LanguageComparer());

            // Get total number of Hashtags
            List<Hashtag> hashtags = context.Hashtags.ToList();
            int totalHashtags = hashtags.Count - 1;

            // Sort hashtags by TimesUsed
            hashtags.Sort(new HashtagComparer());

            // Create array to hold number of tweets in each hour
            int[] tweetsPerHour = new int[24];

            foreach (var tweet in tweets)
            {
                // Get number of Tweets for each hour
                int hour = tweet.DateTime.Hour;
                tweetsPerHour[hour] += 1;
            }

            TBTViewModel tbtViewModel = new TBTViewModel(languages, hashtags, tweetsPerHour,
                totalTweets, totalLanguages, totalHashtags);

            return View(tbtViewModel);
        }
    }
}