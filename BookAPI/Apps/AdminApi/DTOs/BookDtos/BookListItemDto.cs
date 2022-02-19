namespace BookAPI.Apps.AdminApi.DTOs.BookDtos
{
    public class BookListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Language { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public int PageCount { get; set; }
        public AuthorInBookListItem Author { get; set; }
        public GenreInBookListItemDto Genre { get; set; }
    }
    public class AuthorInBookListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
    public class GenreInBookListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
