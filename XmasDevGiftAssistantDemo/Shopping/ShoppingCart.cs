using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XmasDevGiftAssistantDemo.Shopping
{
    public class ShoppingItem
    {
        public string ItemId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
