using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using IslamiTexts.Data;

namespace IslamiTextsTests
{
    [TestClass]
    public class DocumentTests
    {
        [TestMethod]
        public void DeserializeSearchResults()
        {
            string testFile = @"TestFiles\AzureSearchResponse.json";
            string testContent = File.ReadAllText(testFile);
            SearchResultMock notes = JsonConvert.DeserializeObject<SearchResultMock>(testContent);
            Assert.AreEqual(50, notes.Value.Count, "notes.Count");
        }

        public class SearchResultMock
        {
            [JsonProperty(PropertyName = "value")]
            public ICollection<Document> Value;
        }
    }
}
