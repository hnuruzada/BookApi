using System.Collections.Generic;

namespace BookAPI.Data.Entity
{
    public class Author:BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }
        public List<Book> Books { get; set; }
    }
}
