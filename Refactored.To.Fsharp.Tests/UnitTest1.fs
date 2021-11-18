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
let IsbnHasNoDashesButValidThenValidatesSuccesfully () =
    let isbn = "1402894627222"
    let validatedIsbn =StringModule.validateIsbn(isbn)
    
    Assert.AreEqual(isbn, validatedIsbn)