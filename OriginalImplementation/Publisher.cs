using System.Collections.Generic;

namespace OriginalImplementation
{
    public class Publisher
    {
        public string Name { get; init; }
        public int Established { get; init; }
        public string Founder { get; init; }
        public List<Book> Books { get; init; }
        public string Location { get; init; }
        public string ParentCompany { get; init; }
    }
}