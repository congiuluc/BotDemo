using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XmasDevGiftAssistantDemo.Shopping
{
    public class SearchNode
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<SearchNode> SearchSubNodes { get; set; }
    }
}
