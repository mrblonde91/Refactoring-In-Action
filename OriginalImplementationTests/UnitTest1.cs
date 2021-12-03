using System.Collections.Generic;
using Microsoft.FSharp.Collections;
using NUnit.Framework;
using OriginalImplementation;
using Refactored_To_FSharp;
using blah = Refactored.To.FSharp;

namespace OriginalImplementationTests
{
    public class Tests
    {

        [Test]
        public void CSharpVerusFSharpImplementation()
        {
            var csharp = new Book
            {
            };
            //FSharp Implementation Won't build if missing parameters
            //var fsharp = new BookV2 { };
            var publisherV2 = new blah.PublisherV2()
            {

            };
            publisherV2.Founder = "2";
        }
    }

    public class Publishe
    {
    }
}