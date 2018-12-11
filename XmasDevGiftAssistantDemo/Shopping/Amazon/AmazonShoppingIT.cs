using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nager.AmazonProductAdvertising;
using Nager.AmazonProductAdvertising.Model;
using System;
using System.Collections.Generic;
using static XmasDevGiftAssistantDemo.Shopping.Enums;

namespace XmasDevGiftAssistantDemo.Shopping.Amazon
{
    public class AmazonShoppingIT : IShopping
    {
        private readonly string _amazonAccessKey;
        private readonly string _amazonSecretKey;
        private readonly string _amazonPartnerId;
        private readonly ILogger _logger;
        private readonly ShoppingHelperIT _shoppingHelper;

        public AmazonShoppingIT(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AmazonShoppingIT>();

            _amazonAccessKey = configuration.GetSection("AmazonAccessKey")?.Value;

            _amazonSecretKey = configuration.GetSection("AmazonSecretKey")?.Value;

            _amazonPartnerId = configuration.GetSection("AmazonPartnerId")?.Value;

            _shoppingHelper = new ShoppingHelperIT(loggerFactory);

        }


        public SearchNode GetSearchNodeBySearchIndex(SearchIndex searchIndex)
        {
            long searchNodeId = _shoppingHelper.GetRootIdBySearchIndex((int)searchIndex);
            return GetSearchNode(searchNodeId);
        }

        public SearchNode GetSearchNode(long searchNodeId)
        {
            SearchNode result = new SearchNode() { Id = searchNodeId };

            var authentication = new AmazonAuthentication();
            authentication.AccessKey = this._amazonAccessKey;
            authentication.SecretKey = this._amazonSecretKey;

            var wrapper = new AmazonWrapper(authentication, AmazonEndpoint.IT, this._amazonPartnerId);
            wrapper.ErrorReceived += (errorResonse) => { _logger.LogError(errorResonse.Error.Message); };

            BrowseNodeLookupResponse nodelookup = wrapper.BrowseNodeLookup(searchNodeId, AmazonResponseGroup.BrowseNodeInfo);
            if (nodelookup.BrowseNodes.Request.IsValid == "True")
            {
                result.Name = nodelookup.BrowseNodes.BrowseNode.Name;

                foreach (var item in nodelookup.BrowseNodes.BrowseNode.Children)
                {
                    result.SearchSubNodes.Add(GetSearchNode(Convert.ToInt64(item.BrowseNodeId)));
                }
            }
            return result;
        }


        public void SearchTopSellerItems(string keywords, long searchNode, int minPrice, int maxPrice, bool searchSubNodes, Enums.SearchSort searchSort, Enums.SearchSortOrder searchSortOrder)
        {
            var authentication = new AmazonAuthentication();
            authentication.AccessKey = this._amazonAccessKey;
            authentication.SecretKey = this._amazonSecretKey;

            var wrapper = new AmazonWrapper(authentication, AmazonEndpoint.IT, this._amazonPartnerId);
            wrapper.ErrorReceived += (errorResonse) => { _logger.LogError(errorResonse.Error.Message); };

            var searchNodeTopSeller = wrapper.BrowseNodeLookup(searchNode, AmazonResponseGroup.BrowseNodeInfo | AmazonResponseGroup.TopSellers);

            var test33 = wrapper.ItemLookupOperation(new List<string> { "B00PAZYAT2", "B007SGHF3Y" }, AmazonResponseGroup.Small | AmazonResponseGroup.BrowseNodes | AmazonResponseGroup.SalesRank | AmazonResponseGroup.ItemAttributes);
            test33.ParameterDictionary.Add("MerchantId", "Amazon");
            var xmlResponseTT = wrapper.Request(test33);
            var parsedTT = XmlHelper.ParseXml<ItemLookupResponse>(xmlResponseTT.Content);

        }

        public SearchResult SearchItems(string keywords, SearchIndex searchIndex, long browseNode, bool amazonMerchant, int minPrice, int maxPrice, Int16 ItemPage, Enums.SearchSort searchSort, Enums.SearchSortOrder searchSortOrder)
        {

            SearchResult result = new SearchResult();

            var authentication = new AmazonAuthentication();
            authentication.AccessKey = this._amazonAccessKey;
            authentication.SecretKey = this._amazonSecretKey;

            var wrapper = new AmazonWrapper(authentication, AmazonEndpoint.IT, this._amazonPartnerId);
            wrapper.ErrorReceived += (errorResonse) => { _logger.LogError(errorResonse.Error.Message); };



            var searchOperation = wrapper.ItemSearchOperation(keywords, (AmazonSearchIndex)searchIndex, AmazonResponseGroup.Medium | AmazonResponseGroup.BrowseNodes | AmazonResponseGroup.SalesRank | AmazonResponseGroup.ItemAttributes);

            searchOperation.Available();
            searchOperation.Condition(ItemCondition.New);
            searchOperation.ParameterDictionary.Add("ItemPage", ItemPage.ToString());

            if (browseNode == 0)
            {
                browseNode = _shoppingHelper.GetRootIdBySearchIndex((int)searchIndex);
            }
            searchOperation.ParameterDictionary.Add("BrowseNode", browseNode.ToString());

            if (amazonMerchant)
            {
                searchOperation.ParameterDictionary.Add("MerchantId", "Amazon");
            }


            if (minPrice >= 0 && maxPrice > 0)
            {
                searchOperation.PriceBetween(maxPrice*100, minPrice*100);
            }
            else if (minPrice > 0)
            {
                searchOperation.MinPrice(minPrice*100);
            }
            else
            {
                searchOperation.MaxPrice(maxPrice*100);
            }
            if ((int)searchIndex != (int)AmazonSearchIndex.All)
            {
                searchOperation.Sort((AmazonSearchSort)searchSort, (AmazonSearchSortOrder)searchSortOrder);
            }
            // searchOperation.Skip((ItemPage - 1) * 10);
            // searchOperation.Skip(2);
            var xmlResponse = wrapper.Request(searchOperation);

            ItemSearchResponse searchResponse = XmlHelper.ParseXml<ItemSearchResponse>(xmlResponse.Content);

            if (searchResponse.Items.Request.IsValid == "True")
            {
                result.TotalItems = Convert.ToInt32(searchResponse.Items.TotalResults);
                result.TotalPages = Convert.ToInt32(searchResponse.Items.TotalPages);

                foreach (var item in searchResponse.Items.Item)
                {

                    try
                    {
                        Item itemResult = new Item()
                        {
                            ItemId = item.ASIN,
                            Brand = item.ItemAttributes.Brand ?? "",
                            DetailPageUrl = item.DetailPageURL ?? "",
                            ImageUrl = item.LargeImage.URL,
                            Price = (Convert.ToDecimal(item.ItemAttributes.ListPrice.Amount) / 100),
                            SalesRank = Convert.ToInt32(item.SalesRank),
                            Title = item.ItemAttributes.Title ?? ""
                        };
                        result.Items.Add(itemResult);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                 

                }
            }
            return result;
        }

    }
}
