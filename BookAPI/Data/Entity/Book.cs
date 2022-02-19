namespace BookAPI.Data.Entity
{
    public class Book:BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public int PageCount { get; set; }
        public string Language { get; set; }
        public decimal SalePrice { get; set; }

        public decimal CostPrice { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
