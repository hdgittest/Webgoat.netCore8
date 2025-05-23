﻿using WebGoatCore.Models;
using WebGoat.NET.Logger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebGoatCore.Data
{
    public class BlogEntryRepository
    {
        private readonly NorthwindContext _context;

        public BlogEntryRepository(NorthwindContext context)
        {
            _context = context;
        }

        public BlogEntry CreateBlogEntry(string title, string contents, string username)
        {
            DummyLogger.Log($"Adding blog entry - title: {title} | user: {username}");
            var entry = new BlogEntry
            {
                Title = title,
                Contents = contents,
                Author = username,
                PostedDate = DateTime.Now,
            };

            entry = _context.BlogEntries.Add(entry).Entity;
            _context.SaveChanges();
            DummyLogger.Log("Entry added");
            return entry;
        }

        public BlogEntry GetBlogEntry(int blogEntryId)
        {
            return _context.BlogEntries.Single(b => b.Id == blogEntryId);
        }

        public List<BlogEntry> GetTopBlogEntries()
        {
            return GetTopBlogEntries(4, 0);
        }

        public List<BlogEntry> GetTopBlogEntries(int numberOfEntries, int startPosition)
        {
            var blogEntries = _context.BlogEntries
                .OrderByDescending(b => b.PostedDate)
                .Skip(startPosition)
                .Take(numberOfEntries);

            blogEntries = check(numberOfEntries, startPosition, blogEntries);

            return blogEntries.ToList();
        }

        private IQueryable<BlogEntry> check(int numberOfEntries, int startPosition, IQueryable<BlogEntry> blogEntries)
        {
            if(numberOfEntries > startPosition)
                return blogEntries;

            return null;
        }
    }
}
