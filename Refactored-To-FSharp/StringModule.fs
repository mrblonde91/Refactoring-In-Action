namespace Refactored_To_FSharp

open System

module StringModule = 
    let tryCreateString candidate =
            if String.IsNullOrWhiteSpace candidate
            then raise (ArgumentException( "String can cannot be empty"))
            else candidate
            
    let validateIsbn(isbn: string): string =
        tryCreateString isbn |> ignore
        match (String.length (isbn.Replace("-", "")) ) with
        | 13 -> isbn
        | 10 -> isbn
        | _ -> raise (ArgumentException("Isbn is invalid"))