using System;
using static XmasDevGiftAssistantDemo.Shopping.Enums;

namespace XmasDevGiftAssistantDemo.Shopping
{
    public interface IShopping
    {
        SearchResult SearchItems(string keywords, SearchIndex searchIndex, long browseNode, bool amazonMerchant, int minPrice, int maxPrice, Int16 ItemPage, Enums.SearchSort searchSort, Enums.SearchSortOrder searchSortOrder);


        SearchNode GetSearchNodeBySearchIndex(SearchIndex searchIndex);

        SearchNode GetSearchNode(long searchNodeId);



    }
}
