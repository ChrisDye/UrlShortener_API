using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models.Entity
{
    public class Url
    {
        public int Id { get; set; }
        public string ActualUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public int AccessCount { get; set; }
        public DateTime Created { get; set; }
    }
}
