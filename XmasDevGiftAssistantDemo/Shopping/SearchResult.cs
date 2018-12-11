using System.Collections.Generic;

namespace XmasDevGiftAssistantDemo.Shopping
{
    public class SearchResult
    {
        public SearchResult()
        {
            Items = new List<Item>();
        }
        public List<Item> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

    }
}
