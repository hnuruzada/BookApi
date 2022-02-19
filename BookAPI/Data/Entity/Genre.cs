using System.Collections.Generic;

namespace BookAPI.Data.Entity
{
    public class Genre:BaseEntity
    {
        public string Name { get; set; }
        public List<Book> Books { get; set; }

    }
}
