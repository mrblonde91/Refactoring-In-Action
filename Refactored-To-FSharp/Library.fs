namespace Refactored_To_FSharp

open System

module BookMapper =
    let (|GreaterThanZero|_|) a = if a > 0 then Some() else None
    let mapBook(name: string, author: string, isbn: string,
                pageCount: int, publisher: string,
                genres: List<GenreV2>, rating: Option<int>, nextInSeries: Option<string>): BookV2  =
        {
            Author = StringModule.validateString author
            Name = StringModule.validateString name
            Isbn = StringModule.validateIsbn isbn
            PageCount =
                        match pageCount with
                        | GreaterThanZero -> pageCount
                        | _ -> raise (ArgumentException( "Must be greater than 0 pages"))
            Publisher = StringModule.validateString publisher
            Genre = genres
            NextInSeries = nextInSeries
            Rating = rating
        }
        
module BookFunctions =
//    let findBookByAuthor(books: List<BookV2>)(author: string): List<BookV2> =
//         List.filter(fun x -> x.Author.Equals(author)) books
//    let findBookByGenre(books: List<BookV2>)(genre: GenreV2): List<BookV2> =
//         List.filter(fun x -> List.contains(genre) x.Genre) books
//    let findBookByGenreAndAuthor(books:List<BookV2>) (author:string) (genre:GenreV2): List<BookV2> =
//        findBookByAuthor books author |> fun x -> findBookByGenre x genre
    
    let mutable bookCollection = []

    let findBookByGenreAndAuthor(books:  List<BookV2>) (author:string) (genre:GenreV2): List<BookV2> =
        let filteredAuthors = List.filter(fun x -> x.Author.Equals(author)) books
        bookCollection <- List.filter(fun x -> List.contains(genre) x.Genre) filteredAuthors
        bookCollection
        
    let updateNextInSeries(originalBook: BookV2)(nextInSeries: string): BookV2 =
        {
            originalBook with NextInSeries = Some(nextInSeries)
        }