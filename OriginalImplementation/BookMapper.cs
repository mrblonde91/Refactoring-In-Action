using System;
using System.Data;
using System.Linq;

namespace OriginalImplementation
{
    public class BookMapper
    {
        public Book Map(string isbn, string name, string author, int pageCount, string publisher,
            Genre[] genre, int? rating, string nextInSeries)
        {
            //Add isbn validation
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentException("Invalid isbn used");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid author used");
            }

            if (pageCount < 0)
            {
                throw new ArgumentException("Must be greater than zero pages");
            }

            if (string.IsNullOrWhiteSpace(publisher))
            {
                throw new ArgumentException("Publisher must be populated");
            }

            if (genre == null || genre.Length == 0)
            {
                throw new ArgumentException("Genre must have at least one");
            }

            return new Book
            {
                Name = name,
                Author = author,
                Isbn = isbn,
                PageCount = pageCount,
                Publisher = publisher,
                Genre = genre.ToList(),
                Rating = rating,
                NextInSeries = nextInSeries
            };
        }
    }
}