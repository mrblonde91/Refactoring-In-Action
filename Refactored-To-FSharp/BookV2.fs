namespace Refactored_To_FSharp

open Refactored_To_FSharp

type BookV2 =
    {
        Isbn: string
        Name: string
        Author: string
        PageCount: int
        Publisher: string
        Genre: List<GenreV2>
        NextInSeries: Option<string>
        Rating: Option<int>
    }