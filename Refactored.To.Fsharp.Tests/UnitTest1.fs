module Refactored.To.Fsharp.Tests

open System
open NUnit.Framework
open Refactored_To_FSharp

[<SetUp>]
let Setup () =
    ()

[<Test>]
let ModelIsValidAndReturnsSuccesfully () =
    let expectedValues = {| ExpectedName = "Harry Potter"; ExpectedAuthor = "Rowling"; ExpectedPageCount = 24; ExpectedPublisher = "Puffin";
                         ExpectedGenres = [GenreV2.Biography]; ExpectedIsbn = "1-4028-9462-7"|}
    let res = BookMapper.mapBook(expectedValues.ExpectedName, expectedValues.ExpectedAuthor, expectedValues.ExpectedIsbn,
                                 expectedValues.ExpectedPageCount, expectedValues.ExpectedPublisher, [GenreV2.Biography], None, None)
    Assert.AreEqual(expectedValues.ExpectedName, res.Name)
    Assert.AreEqual(expectedValues.ExpectedAuthor, res.Author)
    Assert.AreEqual(expectedValues.ExpectedPageCount, res.PageCount)
    Assert.AreEqual(expectedValues.ExpectedPublisher, res.Publisher)
    Assert.AreEqual(expectedValues.ExpectedIsbn, res.Isbn)
    Assert.Null(res.Rating)
    Assert.Null(res.NextInSeries)

[<Test>]
let ThrowsExceptionIfRequiredStringsAreNull () =
    Assert.Throws<ArgumentException>(fun ()
                                      -> BookMapper.mapBook("", "Rowling", "13-2-21", 24, "B", [GenreV2.Biography], None, None) |> ignore)
    |> ignore
    
[<Test>]
let ThrowsExceptionIfPageCountLessThanZero () =
    Assert.Throws<ArgumentException>(fun ()
                                      -> BookMapper.mapBook("f", "Rowling", "13-2-21", -1, "B", [GenreV2.Biography], None, None) |> ignore)
    |> ignore
    
[<Test>]
let ThrowsExceptionIfPageCountEqualZero () =
    Assert.Throws<ArgumentException>(fun ()
                                      -> BookMapper.mapBook("f", "Rowling", "13-2-21", 0, "B", [GenreV2.Biography], None, None) |> ignore)
    |> ignore
    
[<Test>]
let ThrowsExceptionIfIsbnInvalidLength () =
    Assert.Throws<ArgumentException>(fun ()
                                      -> StringModule.validateIsbn("1") |> ignore)
    |> ignore
    
[<Test>]
let IsbnIsLengthTenThenValidates () =
    let isbn = "1-4028-9462-7"
    let validatedIsbn =StringModule.validateIsbn(isbn)
    
    Assert.AreEqual(isbn, validatedIsbn)

[<Test>]
let IsbnIsLengthThirteenThenValidates () =
    let isbn = "1-4028-9462-7222"
    let validatedIsbn =StringModule.validateIsbn(isbn)
    
    Assert.AreEqual(isbn, validatedIsbn)
    
[<Test>]
let IsbnHasNoDashesButValidThenValidatesSuccessfully () =
    let isbn = "1402894627222"
    let validatedIsbn =StringModule.validateIsbn(isbn)
    
    Assert.AreEqual(isbn, validatedIsbn)
    
[<Test>]
let UpdateNextInSeriesSuccesfullyUpdates () =
    let expectedValues = {| ExpectedName = "Harry Potter"; ExpectedAuthor = "Rowling"; ExpectedPageCount = 24; ExpectedPublisher = "Puffin";
                         ExpectedGenres = [GenreV2.Biography]; ExpectedIsbn = "1-4028-9462-7"; ExpectedNextInSeries = "Kill Bill" |}
    let res = BookMapper.mapBook(expectedValues.ExpectedName, expectedValues.ExpectedAuthor, expectedValues.ExpectedIsbn,
                                 expectedValues.ExpectedPageCount, expectedValues.ExpectedPublisher, [GenreV2.Biography], None, None)
    let updatedModel = BookFunctions.updateNextInSeries res "Kill Bill"
    Assert.AreEqual(expectedValues.ExpectedNextInSeries, updatedModel.NextInSeries.Value)
    
    
[<Test>]
let ShouldBeAbleToFindBookByGenreAndAuthor () =
    let expectedAuthor = "Stephen King"
    let expectedGenre = GenreV2.Horror
    let bookCollection = [BookMapper.mapBook("The Shining", "Stephen King", "978-3-16-148410-0",
                                 1200, "DoubleDay", [GenreV2.Horror], None, None);
    BookMapper.mapBook("Pet Semetary", "Stephen King", "978-3-16-148410-0",
                                 1000, "DoubleDay", [GenreV2.Horror], None, None);
    BookMapper.mapBook("Stand By Me", "Stephen King", "978-3-16-148410-0",
                                 50, "DoubleDay", [GenreV2.Drama], None, None);
    BookMapper.mapBook("Ulysses", "James Joyce", "978-3-16-148410-0",
                                 400, "DoubleDay", [GenreV2.Drama], None, None)
    ]
    let result = BookFunctions.findBookByGenreAndAuthor bookCollection expectedAuthor expectedGenre
    
    Assert.AreEqual(2, result.Length)