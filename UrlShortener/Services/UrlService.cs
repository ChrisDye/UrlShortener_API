using Entities.Context;
using Entities.Interfaces;
using Entities.Models.DTO;
using Entities.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Services.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Services
{
    public class UrlService : IUrlService
    {
        private readonly int defaultPage = 1;
        private readonly int defaultPageSize = 5;

        protected readonly UrlContext _context;

        public UrlService(UrlContext context)
        {
            _context = context;
        }

        public async Task DeleteUrl(int id)
        {
            // Delete the Url from the database with the specified ID
            var url = _context.Urls.Where(u => u.Id == id).FirstOrDefault();

            if (url == null)
            {
                throw new Exception("Not found");
            }

            _context.Urls.Remove(url);

            await _context.SaveChangesAsync();
        }

        public async Task<Paginated<UrlDto>> GetAll(int? page, int? pageSize)
        {
            page = page ?? defaultPage;
            pageSize = pageSize ?? defaultPageSize;

            // Get all the urls from the database, use default paging if none provided
            var urls = await _context.Urls.OrderByDescending(o => o.Id)
                .Skip(((int)page - 1) * (int)pageSize)
                .Take((int)pageSize).ToArrayAsync();

            List<UrlDto> finalUrls = new List<UrlDto>();
            foreach (var u in urls)
            {
                finalUrls.Add(new UrlDto
                {
                    Id = u.Id,
                    ActualUrl = u.ActualUrl,
                    ShortenedUrl = u.ShortenedUrl,
                    AccessCount = u.AccessCount
                });
            }

            var totalCount = await _context.Urls.CountAsync();

            return new Paginated<UrlDto>(finalUrls, totalCount, (int)page, (int)pageSize);
        }

        public async Task<UrlDto> GetUrl(string shortened)
        {
            // Return the individual url specified. 
            var url = await _context.Urls.Where(u => u.ShortenedUrl == shortened).FirstOrDefaultAsync();

            if (url == null)
            {
                throw new Exception("Not found");
            }

            // We should also update the access count here, as we can presume this is someone being redirected
            url.AccessCount = url.AccessCount + 1;
            _context.Urls.Update(url);
            await _context.SaveChangesAsync();

            UrlDto finalUrl = new UrlDto
            {
                Id = url.Id,
                ActualUrl = url.ActualUrl,
                ShortenedUrl = url.ShortenedUrl,
                AccessCount = url.AccessCount
            };

            return finalUrl;
        }

        public async Task<UrlDto> AddUrl(UrlCreate url)
        {
            // Add the new url to the database
            if (url == null || url.ActualUrl == null || url.ActualUrl.Trim() == string.Empty)
            {
                throw new Exception("Invalid url");
            }

            // Now generate the short url.
            string shortenedUrl = string.Empty;
            while (shortenedUrl == string.Empty)
            {
                var shortenedValue = Shortener.CreateShortCode();
                var existing = _context.Urls.Where(u => u.ShortenedUrl == shortenedValue).FirstOrDefault();
                if (existing == null)
                {
                    shortenedUrl = shortenedValue;
                }
            }

            _context.Urls.Add(new Url
            {
                ActualUrl = url.ActualUrl,
                ShortenedUrl = shortenedUrl,
                Created = DateTime.Now
            });

            await _context.SaveChangesAsync();

            return new UrlDto
            {
                ActualUrl = url.ActualUrl,
                ShortenedUrl = shortenedUrl,
                AccessCount = 0
            };
        }

        public async Task<UrlDto> UpdateUrl(UrlDto url)
        {
            if (url == null)
            {
                throw new Exception("Not found");
            }
            var currentUrl = await _context.Urls.Where(u => u.Id == url.Id).FirstOrDefaultAsync();

            if (currentUrl == null)
            {
                throw new Exception("Not found");
            }

            currentUrl.ShortenedUrl = url.ShortenedUrl;
            currentUrl.ActualUrl = url.ActualUrl;

            // An update should potentially clear down the access count? Left as is for now

            await _context.SaveChangesAsync();

            return new UrlDto
            {
                Id = url.Id,
                ActualUrl = url.ActualUrl,
                ShortenedUrl = url.ShortenedUrl,
                AccessCount = currentUrl.AccessCount
            };
        }
    }
}
