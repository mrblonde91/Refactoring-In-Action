# Refactoring C# to F# in Action
## Introduction
As part of this demonstration of refactoring C# to F#, I've decided to utilise C# 9.0 functionality. This illustrates how C# continues to adopt F# functionality and much of the time the code is almost indistinguishable from the F# variant. However during my time experimenting with C# 9.0, pitfalls became pretty apparent. C# 9.0 simply isn't supported on many legacy projects so a shared library for example is not necessarily achievable with it. Meanwhile F# is backward compatible, you might not always get the latest and greatest features if supporting particularly old .Net Framework projects however it is possible to get a nice balance.

At times, there were issues around F# where things didn't behave as we expected. Hopefully this piece will help other developers to bypass these issues entirely.

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

The key difference between a class and a record in terms of defining them is the use of `record` instead of `class`. And the introduction of `init` in addition to standard getters and setters. The init prevents the modification of a field after the creation of a record. 

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

The F# equivalent record has a few slight differences. The composition being the first example, there's less boilerplate. The other most apparent one here is the use of the `Option` keyword. This is similar to a nullable value, if null the type will be `None` and if populated it will be `Some` with the value inside it. This allows for easy checks on if a value is entirely missing and allows for simply representations of optional values.

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

The two parameters in this case are put in two separate pairs of brackets and the return type is after the colon. But as mentioned previously, the syntax is surprisingly similar to C# 9.0 variant with the omission of braces after the `with` and treating the string as an `Option`.

## Validation
Validation can be handled in many ways in the dotnet world however the cleanest ways to achieve it often requires custom validation of some kind. Constructor level checks are one of the potential approaches. For the example, I've simply utilised a mapping class.


#### C# example

            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentException("Isbn must be populated");
            }

The above is pretty much the most basic way of handling strings. It throws if it's not populated. Checks of this kind are required on each of these properties. 

#### F# example 

    let validateString candidate =
            if String.IsNullOrWhiteSpace candidate
            then raise (ArgumentException( "String can cannot be empty"))
            else candidate

So F# does require a slightly custom approach to verifying that a string is populated. In this case, I have introduced a basic null or white space check. It'll throw in the event of the value being null or whitespace.

            Author = StringModule.validateString author
            Name = StringModule.validateString name

At the mapping level the function can be called to verify required strings are populated. 

The ISBN scenario requires some more checks. Eg it should only be 10 or 13 characters long.

### C# example

        var strippedIsbn = isbn.Replace("-", "");
        return isbn.Length == 13 || isbn.Length == 10;

The null or whitespace checks are omitted but the logic is pretty self explanatory.

### F# example

    let validateIsbn(isbn: string): string =
        validateString isbn |> ignore
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

It's much more simple and self explanatory than what we'd end up with in C# however there is an additional benefit which can be illustrated with the creation of a record in C#.

### C# Gone Wrong

            var blah = new Book
            {
            };

If the constructor of Book is parameterless, nothing prevents us from creating an entirely blank record on the fly. Meanwhile the F# code simply won't build if not all values are provided. The key advantage here is, F# implicitly defines the expectation of all fields being explicitly populated. We can catch common errors like the above at build. Eg forgetting a field when you have a dozen or more to define simply isn't allowed. This maintains rules around the domain by default and allows for type inference.

Other examples include the simple fact that most C# projects do not use `records` either because of lack of awareness or that projects do not support it. Some projects might do something like the below and require explicit constructors. But this often can result in pretty large and messy classes. 

        public string Publisher { get; private set; }    

Invariably this will lead to random properties omitting the private setter out of lazy coding. Meanwhile in F#, we default to immutable properties. Instead of explicitly defining a property as immutable, the developer must state it is `mutable`. In code review, this is far more apparent than omitting a private settter so even if it is required, a discussion can start around it.

        mutable Rating: Option<int>

### F# Gotchas
One issue that I have encountered as part of migrating existing code to F# is the `[<CliMutable>]` attribute. For example with dapper you need a parameterless constructor. The `CliMutable` attribute effectively does that. In F#, these records will behave in the exact same way and they will remain immutable. However what it effectively does in the background is it creates getters and setters result in mutability being introduced when it gets to the C#.

            var publisherV2 = new PublisherV2()
            {

            };
            publisherV2.Founder = "2";

Four potential approaches I have considered for managing this are as follows.
1. Maintain a DTO model and a separate one to return to C#. 
2. Use named constructors however this is pretty time consuming
3. Accept the loss of immutability in the likes of C#

This issue unfortunately applies to many standard forms of serialization with one exception which is our fourth option.

4. Handling our serialization with .Net's serialization or Newtonsoft's will allow for model's to be deserialized into an object in an immutable way.

            var result = await connection.QueryAsync<object>(commandDefinition);
            var serializeObject = JsonConvert.SerializeObject(result);
            return
                JsonConvert.DeserializeObject<IEnumerable<ProductV2>>(serializeObject,
                    new OptionsConverter.OptionConverter());

Performance wise, this appears to be okay, the below results are from the Benchmark dotnet with a result set of 170 unique dapper items. To get a clearer picture, the newtonsoft version is skipping the sql request, dapper handles serialization as part of the request so serialization alone can't be tested. Overall though, the performance hit is minimal and just an addition of a few milliseconds to the existing request. This [converter](https://github.com/haf/Newtonsoft.Json.FSharp) is needed to handle options in Newtonsoft.

|                         Method |       Mean |      Error |    StdDev |
|------------------------------- |-----------:|-----------:|----------:|
| RunCliMutablePureDapperVersion | 483.037 ms | 10.8998 ms | 32.138 ms |
|           RunNewtonsoftVersion |   7.864 ms |  0.3747 ms |  1.105 ms |

A sample of the extension methods for mapping is included below.

        public static async Task<IEnumerable<T>> QueryAsListMappedAsync<T>(this MySqlConnection connection, CommandDefinition commandDefinition)
        {
            // Retrieve sql data as objects and serialize it.
            var result = await connection.QueryAsync<object>(commandDefinition);
            var serializeObject = JsonConvert.SerializeObject(result);
            // Then deserialize it to prefered object with newtonsoft.
            return JsonConvert.DeserializeObject<IEnumerable<T>>(serializeObject,
                new OptionConverter()) ?? new List<T>();
        }
        
        public static async Task<T?> QueryFirstOrDefaultMappedAsync<T>(this MySqlConnection connection, CommandDefinition commandDefinition)
        {
            var result = await connection.QueryFirstOrDefaultAsync<object>(commandDefinition);
            var serializeObject = JsonConvert.SerializeObject(result);
            return JsonConvert.DeserializeObject<T>(serializeObject,
                new OptionConverter());
        }

## Conclusion
This article has only really touched upon issues and approaches towards refactoring C# to F#. However it should hopefully give an idea of the immense benefits that F# offers to existing dotnet projects. Everything from linq to C# 9.0's records borrow heavily from functional programming in general so the huge benefit that F# offers is you are also able to offer such functionality as immutability to legacy projects that don't necessarily support C# 9.0.

Interoperability can admittedly be unpredictable at times. Eg the immutability issue with CliMutable. However as demonstrated, it is reasonably straight forward to achieve it with some experimentation. With immutability, we gain  the advntage of pure code. Unexpected side effects such as our end results varying are bypassed. Plus it's incredibly concise and can easily be integrated into existing C# code.
