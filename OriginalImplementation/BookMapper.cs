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
            if (!ValidateIsbn(isbn))
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

        public Book UpdateNextInSeries(Book originalBook, string nextInSeries)
        {
            return originalBook with {NextInSeries = nextInSeries};
        }
        
        private bool ValidateIsbn(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentException("Isbn must be populated");
            }
            var strippedIsbn = isbn.Replace("-", "");
            return isbn.Length == 13 || isbn.Length == 10;
        }
    }
}