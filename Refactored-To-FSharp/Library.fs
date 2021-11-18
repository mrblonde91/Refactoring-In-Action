namespace Refactored_To_FSharp

open System

module BookMapper =
    let (|GreaterThanZero|_|) a = if a > 0 then Some() else None
    let mapBook(name: string, author: string, isbn: string,
                pageCount: int, publisher: string,
                genres: List<GenreV2>, rating: Option<int>, nextInSeries: Option<string>): BookV2  =
        {
            Author = StringModule.tryCreateString author
            Name = StringModule.tryCreateString name
            Isbn = StringModule.validateIsbn isbn
            PageCount =
                        match pageCount with
                        | GreaterThanZero -> pageCount
                        | _ -> raise (ArgumentException( "Must be greater than 0 pages"))
            Publisher = StringModule.tryCreateString publisher
            Genre = genres
            NextInSeries = nextInSeries
            Rating = rating
        }
    let updateNextInSeries(originalBook: BookV2)(nextInSeries: string): BookV2 =
        {
            originalBook with NextInSeries = Some(nextInSeries)
        }