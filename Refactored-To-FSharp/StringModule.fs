namespace Refactored_To_FSharp

open System

module StringModule = 
    let validateString candidate =
            if String.IsNullOrWhiteSpace candidate
            then raise (ArgumentException( "String can cannot be empty"))
            else candidate
            
    let validateIsbn(isbn: string): string =
        validateString isbn |> ignore
        match (String.length (isbn.Replace("-", "")) ) with
        | 13 | 10 -> isbn
        | _ -> raise (ArgumentException("Isbn is invalid"))