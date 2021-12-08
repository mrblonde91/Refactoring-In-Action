namespace Refactored_To_FSharp

open System

module ValidationModule = 
    let validateString (candidate: string) : unit =
            if String.IsNullOrWhiteSpace candidate
            then invalidArg(nameof candidate) ""
   
    let validateIsbn(isbn: string): unit =
        validateString isbn
        match (String.length (isbn.Replace("-", "")) ) with
        | 13 | 10 -> ()
        | _ -> invalidArg(nameof isbn) ""
        
    let (|GreaterThanZero|_|) a = if a > 0 then Some() else None
  
    let validatePageCount(pageCount: int): unit =
        match pageCount with
                        | GreaterThanZero -> ()
                        | _ -> raise (invalidArg(nameof pageCount) "Must be greater than 0 pages") 