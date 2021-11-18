# Refactoring C# to F# in Action
## Introduction
As part of this demonstration of refactoring C# to F#, I've decided to utilise C# 9.0 functionality. This illustrates how C# continues to adopt F# functionality and much of the time the code is almost indistinguishable from the F# variant. However during my time experimenting with C# 9.0, pitfalls became pretty apparent. C# 9.0 simply isn't supported on many legacy projects so a shared library for example is not necessarily achievable with it. Meanwhile F# is backward compatible, you might not always get the latest and greatest features if supporting particularly old .Net Framework projects however it is possible to get a nice balance.

## Records
Records are effectively the F# equivalent of a class except they are immutable. C# 9.0 also introduced immutable records.

##### C# Record

    public record Book
    {
        public string Isbn { get; init; }
        public string Name { get; init; }
        public string Author { get; init; }
        public int PageCount { get; init; }
        public string Publisher { get; init; }
        public List<Genre> Genre { get; init; }
        public string NextInSeries { get; init; }
        public int? Rating { get; init; }
    }

The key difference between a class and a record and a class in terms of defining them is the use of `record` instead of `class`. And the introduction of `init` in addition to standard getters and setters. The init prevents the modification of a field after the creation of a record. 

##### F# Record

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

The F# equivalent record has slight different. The composition being the first example, there's less boilerplate. The other most apparent one here is the use of options. This is similar to a nullable value, if null the type will be `None` and if populated it will be `Some` with the value inside it. This allows for easy checks on if a value is entirely missing and allows for simply representations of optional values.

#### Updating a record

The only possibly way to update an record is via `non destructive mutation`. Effectively we create a new object based upon the existing one.

##### C# implementation

        public Book UpdateNextInSeries(Book originalBook, string nextInSeries)
        {
            return originalBook with {NextInSeries = nextInSeries};
        }

In this example, I have simply updated a single field in the book but I haven't destroyed or modified the original. Our returned record is a shallow copy of the previous version of the book. 

##### F# implementation
    
    let updateNextInSeries(originalBook: BookV2)(nextInSeries: string): BookV2 =
        {
            originalBook with NextInSeries = Some(nextInSeries)
        }

The two parameters in this case are put in two separate pairs of brackets and the return type is after the colon. But as mentioned previously, the syntax is surprisingly similar to C# 9.0 variant with the omission of braces after the with and treating the string as an option.

## Validation
Validation can be handled in many ways in the dotnet world however the cleanest ways to achieve it often requires custom validation of some kind. Constructor level checks is one of the potential approaches. For the example, I've simply utilised a mapping class.


#### C# example

            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentException("Isbn must be populated");
            }

The above is pretty much the most basic way of handling strings. It throws if it's not populated. Checks of this kind are required on each of these properties. 

#### F# example 

    let tryCreateString candidate =
            if String.IsNullOrWhiteSpace candidate
            then raise (ArgumentException( "String can cannot be empty"))
            else candidate

So F# does require a slightly custom approach to verifying that a string is populated. In this case, I have introduced a basic null or white space check. It'll throw in the event of the value being null or whitespace.

            Author = StringModule.tryCreateString author
            Name = StringModule.tryCreateString name

At the mapping level the function can be called to verify required strings are populated. 

The ISBN scenario requires some more checks. Eg it should only be 10 or 13 characters long.

### C# example

        var strippedIsbn = isbn.Replace("-", "");
        return isbn.Length == 13 || isbn.Length == 10;

The null or whitespace checks are omitted but the logic is pretty self explanatory.

### F# example

    let validateIsbn(isbn: string): string =
        tryCreateString isbn |> ignore
        match (String.length (isbn.Replace("-", "")) ) with
        | 13 -> isbn
        | 10 -> isbn
        | _ -> raise (ArgumentException("Isbn is invalid"))

So in this case we take advantage of a match condition which allows for pattern matching. So we throw in any scenario where it doesn't match 10 or 13. 

Another example of the use of it is for the page count. The C# is a basic check of if it's greater than zero. We can use a match for a more concise and clear representation of the validation. 

### F# example
    let (|GreaterThanZero|_|) a = if a > 0 then Some() else None
    ...
    PageCount =
        match pageCount with
        | GreaterThanZero -> pageCount
        | _ -> raise (ArgumentException( "Must be greater than 0 pages")

As this all comes together, we end up with the following mapper.

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

It's much more simple and self explanatory than what we'd end up with in C# however there is an additional benefit which can be illustrated with the creation of a record in C#

### C# Gone Wrong

            var blah = new Book
            {
            };

If the constructor of Book is parameterless, nothing prevents us from creating an entirely blank record on the fly. Meanwhile the F# code simply won't build if not all values are provided.