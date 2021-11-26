module Refactored.To.FSharp

open Refactored_To_FSharp

//        public string Name { get; init; }
//        public int Established { get; init; }
//        public string Founder { get; init; }
//        public List<Book> Books { get; init; }
//        public string Location { get; init; }
//        public string ParentCompany { get; init; }
[<CLIMutable>]
type PublisherV2 =
    {
        Name: string
        Established: int
        Founder: string
        Books: List<BookV2>
        Location: string
        ParentCompany: Option<string>
    }
