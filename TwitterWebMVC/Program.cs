using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tweetinvi;
using Microsoft.Extensions.DependencyInjection;
using TwitterWebMVC.Data;
using TwitterWebMVC.Models;

namespace TwitterWebMVC
{
    public class Program
    {
        private static TweetDbContext context;

        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            var scope = host.Services.GetService<IServiceScopeFactory>().CreateScope();
            context = scope.ServiceProvider.GetRequiredService<TweetDbContext>();

            // Set up your credentials (https://apps.twitter.com)
            // Applies credentials for the current thread.If used for the first time, set up the ApplicationCredentials
            Auth.SetUserCredentials("5Za8wv3dg9cL1JpMkZ69xHiYR", "q3ieBRSIQGJOXzXjtAuSySHcFQwiKHgKCEccDSbyMYxGc5GbbT", "858816969759444992-ZOBHLzMEq4V9TV6XbYVFk9z9auNRt8v", "gEUnPo24s7dMUbvr0QyfHCbFKbWbzF8XV6F9WXtaudRYc");
            var user = User.GetAuthenticatedUser();

            // Enable Automatic RateLimit handling
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            var stream = Tweetinvi.Stream.CreateFilteredStream();
            stream.AddTrack("#Trump");
            stream.MatchingTweetReceived += (sender, recievedTweet) =>
            {
                // if language is in DB retrieve it else create new langauge object
                Language tweetLanguage;

                if (GetLanguage(recievedTweet.Tweet.Language.ToString()) != null)
                {
                    tweetLanguage = GetLanguage(recievedTweet.Tweet.Language.ToString());
                    tweetLanguage.TimesUsed += 1;
                }
                else
                {
                    Language newLanguage = new Language
                    {
                        Name = recievedTweet.Tweet.Language.ToString()
                    };
                    context.Languages.Add(newLanguage);
                    context.SaveChanges();

                    tweetLanguage = newLanguage;
                }

                // Create new tweet object and add to db
                Models.Tweet newTweet = new Models.Tweet
                {
                    Language = tweetLanguage,
                    DateTime = recievedTweet.Tweet.CreatedAt
                };
                context.Tweets.Add(newTweet);
                context.SaveChanges();

                // if hashtag is in DB retrieve it else create new hashtag object
                List<Hashtag> hashtagList = new List<Hashtag>();

                foreach (var hashtag in recievedTweet.Tweet.Hashtags)
                {
                    if (GetHashtag(hashtag.ToString()) != null)
                    {
                        if (hashtag.ToString() != "#Trump")
                        {
                            Hashtag tweetHashtag = GetHashtag(hashtag.ToString());
                            hashtagList.Add(tweetHashtag);
                            tweetHashtag.TimesUsed += 1;
                        }
                    }
                    else
                    {
                        Hashtag newHashtag = new Hashtag
                        {
                            Name = hashtag.ToString()
                        };
                        context.Hashtags.Add(newHashtag);
                        context.SaveChanges();
                        hashtagList.Add(newHashtag);
                    }
                }

                // Create TweetHashtag object for each hashtag
                foreach(var hashtag in hashtagList)
                {
                    TweetHashtag tweetHashtag = new TweetHashtag
                    {
                        Tweet = newTweet,
                        TweetID = newTweet.ID,
                        Hashtag = hashtag,
                        HashtagID = hashtag.ID
                    };
                    context.TweetHashtags.Add(tweetHashtag);
                    context.SaveChanges();
                }
            };

            // TODO 8 Add Comments about difficult promblems
            stream.StartStreamMatchingAnyConditionAsync();

            host.Run();

            Hashtag GetHashtag(string hashtag)
            {
                Hashtag existingHashtag = context.Hashtags.FirstOrDefault(h => h.Name == hashtag);
                return existingHashtag;
            }

            Language GetLanguage(string language)
            {
                Language existingLangauge = context.Languages.FirstOrDefault(l => l.Name == language);
                return existingLangauge;
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }


}
