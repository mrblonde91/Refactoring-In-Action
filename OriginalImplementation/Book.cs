using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OriginalImplementation
{
    public record Book
    {
        public string Isbn { get; init; }
        public string Name { get; init; }
        public string Author { get; init; }
        public int PageCount { get; init; }
        public string Publisher { get; init; }
        public List<Genre> Genre { get; init; }
        public string NextInSeries { get; init; }
        public int? Rating { get; init; }
    }
}