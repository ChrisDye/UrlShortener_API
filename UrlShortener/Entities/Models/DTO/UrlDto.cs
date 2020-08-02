using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models.DTO
{
    public class UrlDto
    {
        public int Id { get; set; }
        public string ActualUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public int AccessCount { get; set; }
    }
}
