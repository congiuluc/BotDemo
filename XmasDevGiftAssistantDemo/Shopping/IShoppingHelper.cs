using System;
using System.Collections.Generic;
using static XmasDevGiftAssistantDemo.Shopping.Enums;

namespace XmasDevGiftAssistantDemo.Shopping
{
    public interface IShoppingHelper
    {
        Dictionary<int, string> GetSearchIndexDictionary();

        Dictionary<int, string> GetSearchIndexDictionary(int age);
        
        Dictionary<int, string> GetSearchIndexDictionary(int age, Gender gender);

        string GetSearchIndexDesc(int searchIndex);

        long GetRootIdBySearchIndex(int searchIndex);



    }
}
