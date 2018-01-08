﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterWebMVC.Models;

namespace TwitterWebMVC.Data
{
    public class TweetDbContext : DbContext
    {
        public DbSet<Language> Languages { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<TweetHashtag> TweetHashtags { get; set; }

        public TweetDbContext(DbContextOptions<TweetDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TweetHashtag>()
                .HasKey(t => new { t.HashtagID, t.TweetID });
        }
    }
}
