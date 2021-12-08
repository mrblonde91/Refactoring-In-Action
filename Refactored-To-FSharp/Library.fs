namespace Refactored_To_FSharp


module BookMapper =
    let mapBook(name: string, author: string, isbn: string,
                pageCount: int, publisher: string,
                genres: List<GenreV2>, rating: Option<int>, nextInSeries: Option<string>): BookV2  =
        ValidationModule.validateString author
        ValidationModule.validateString name
        ValidationModule.validateIsbn isbn
        ValidationModule.validatePageCount pageCount
        ValidationModule.validateString publisher
        {
            Author = author
            Name = name
            Isbn = isbn
            PageCount = pageCount
            Publisher = publisher
            Genre = genres
            NextInSeries = nextInSeries
            Rating = rating
        }
        
module BookFunctions =
    let findBookByAuthor(books: List<BookV2>)(author: string): List<BookV2> =
         List.filter(fun x -> x.Author.Equals(author)) books
    
    let findBookByGenre (genre: GenreV2) (books: List<BookV2>): List<BookV2> =
         List.filter(fun x -> List.contains(genre) x.Genre) books
         
    // partial application
    let findBookByGenreAndAuthor(books:List<BookV2>) (author:string) (genre:GenreV2): List<BookV2> =
        findBookByAuthor books author |> findBookByGenre genre


//    let mutable bookCollection = []
//
//    let findBookByGenreAndAuthor(books:  List<BookV2>) (author:string) (genre:GenreV2): List<BookV2> =
//        let filteredAuthors = List.filter(fun x -> x.Author.Equals(author)) books
//        bookCollection <- List.filter(fun x -> List.contains(genre) x.Genre) filteredAuthors
//        bookCollection
        
    let updateNextInSeries(originalBook: BookV2)(nextInSeries: string): BookV2 =
        {
            originalBook with NextInSeries = Some(nextInSeries)
        }