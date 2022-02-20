using System.Collections.Generic;

namespace BookMVC.DTOs
{
    public class ListDto<TItem>
    {
        public List<TItem> Items { get; set; }
    }
}
