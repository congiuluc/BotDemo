using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XmasDevGiftAssistantDemo.Shopping
{
    public class Item
    {

        public string ItemId { get; set; }
        public string DetailPageUrl { get; set; }
        public string Brand { get; set; }
        public string Title{ get; set; }
        public decimal Price { get; set; }
        public int SalesRank { get; set; }
        public string ImageUrl { get; set; }
        
    }
}
