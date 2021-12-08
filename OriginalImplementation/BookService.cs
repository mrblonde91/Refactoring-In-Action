using System.Collections.Generic;
using System.Linq;

namespace OriginalImplementation
{
    public class BookService
    {
        public IEnumerable<Book> FindBooksByGenreAndAuthor(List<Book> books,
            string author, Genre genre)
        {
            return books.Where(x => x.Author == author
                                    && Equals(x.Genre, genre));
        }

        //Alternative Approach
        public IEnumerable<Book> FindBooksByAuthor(List<Book> books,
            string author)
        {
            return books.Where(x => x.Author == author);
        }

        public IEnumerable<Book> FindBooksByGenre(List<Book> books,
            Genre genre)
        {
            return books.Where(x => Equals(x.Genre, genre));
        }

        public IEnumerable<Book> FindBooksByGenreAndAuthorV2(List<Book> books,
            string author, Genre genre)
        {
            var booksByAuthor = FindBooksByAuthor(books, author).ToList();
            return FindBooksByGenre(booksByAuthor, genre);
        }
    }
}