using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using IslamiTexts.Models;
using IslamiTexts.Data.Models;
using System.Diagnostics;

namespace IslamiTexts.Data
{
    public class AzureSearchService : ISearcher
    {
        private static readonly string[] VerseSearchFields = new[]
        {
            PropertyNames.ArText,
            //PropertyNames.ArTextClean,
            PropertyNames.EnTrArberry,
            PropertyNames.EnTrAsad,
            PropertyNames.EnTrAyubKhan,
            PropertyNames.EnTrDaryabadi,
            //PropertyNames.EnTrHilali,
            PropertyNames.EnTrMaududi,
            PropertyNames.EnTrPickthall,
            PropertyNames.EnTrQaribullah,
            PropertyNames.EnTrShakir,
            PropertyNames.EnTrSherAli,
            PropertyNames.EnTrYusufAli
        };

        private static readonly string[] VerseCommentarySearchFields = new[]
        {
            PropertyNames.EnSearchNotesAsad,
            PropertyNames.EnSearchNotesYusufAli
        };

        public SearchResults GetSearchResults(
            SearchScope searchScope, 
            string searchString, 
            Translator preferredSnippetTranslator,
            int start,
            int resultsToFetch)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(searchString), "searchString is null or whitespace.");
            Debug.Assert(resultsToFetch > 0 && resultsToFetch < 100, "resultsToFetch must be between 0 and 100.");

            ISearchIndexClient searchClient = CreateSearchIndexClient();

            List<string> searchFields = new List<string>();

            if (searchScope.HasFlag(SearchScope.Verse)) searchFields.AddRange(VerseSearchFields);
            if (searchScope.HasFlag(SearchScope.VerseCommentary)) searchFields.AddRange(VerseCommentarySearchFields);

            List<string> fieldsToSelect = new List<string>(
                new string[] { PropertyNames.SurahNo, PropertyNames.VerseNo, PropertyNames.ArText });
            fieldsToSelect.AddRange(VerseSearchFields);
            //fieldsToSelect.AddRange(VerseCommentarySearchFields);

            SearchParameters parameters = new SearchParameters
            {
                Select = fieldsToSelect,
                SearchFields = searchFields,
                HighlightFields = searchFields,
                IncludeTotalResultCount = true,
                SearchMode = SearchMode.All,
                Top = resultsToFetch,
                Skip = start,
                QueryType = QueryType.Full
            };

            DocumentSearchResult<VerseDocument> results = 
                searchClient.Documents.Search<VerseDocument>(searchString, parameters);

            SearchResults searchResults = new SearchResults(start, resultsToFetch, (int)results.Count.Value);

            foreach (SearchResult<VerseDocument> result in results.Results)
            {
                searchResults.ResultItems.Add(GetSearchResultItem(result, preferredSnippetTranslator));
            }

            return searchResults;
        }

        private static ISearchResult GetSearchResultItem(
            SearchResult<VerseDocument> result,
            Translator preferredSnippetTranslator)
        {
            ContentType resultType = ContentType.VerseCommentary;

            foreach (var highlight in result.Highlights)
            {
                if (VerseSearchFields.Contains(highlight.Key))
                {
                    resultType = ContentType.Verse;
                }
            }

            switch (resultType)
            {
                case ContentType.Verse: return GetVerseResult(result, preferredSnippetTranslator);
                case ContentType.VerseCommentary: return GetVerseCommentaryResult(result, preferredSnippetTranslator);
                default: throw new NotSupportedException();
            }
        }

        private static ISearchResult GetVerseResult(
            SearchResult<VerseDocument> result,
            Translator preferredSnippetTranslator)
        {
            ISearchResult item = new VerseResult
            {
                ResultType = ContentType.Verse,
                SurahNo = result.Document.SurahNo,
                VerseNo = result.Document.VerseNo,
                ArabicText = result.Document.ArabicText,
                Translation = result.Document.GetTranslation(preferredSnippetTranslator)
            };



            return item;
        }

        private static ISearchResult GetVerseCommentaryResult(
            SearchResult<VerseDocument> result,
            Translator preferredSnippetTranslator)
        {
            VerseCommentaryResult item = new VerseCommentaryResult
            {
                ResultType = ContentType.VerseCommentary,
                SurahNo = result.Document.SurahNo,
                VerseNo = result.Document.VerseNo
            };

            IList<string> asadHighlights;
            result.Highlights.TryGetValue(PropertyNames.EnSearchNotesAsad, out asadHighlights);
            item.Snippets = asadHighlights;

            IList<string> yusufHighlights = null;
            result.Highlights.TryGetValue(PropertyNames.EnSearchNotesYusufAli, out yusufHighlights);

            if (asadHighlights == null
                || asadHighlights.Count == 0
                || (preferredSnippetTranslator == Translator.YusufAli && 
                    yusufHighlights != null &&
                    yusufHighlights.Count > 0))
            {
                item.Snippets = yusufHighlights;
            }

            return item;
        }

        private static ISearchIndexClient CreateSearchIndexClient()
        {
            string searchServiceName = ConfigurationManager.AppSettings["azureSearchServiceName"];
            string adminApiKey = ConfigurationManager.AppSettings["azureSearchApiKey"];
            string indexName = ConfigurationManager.AppSettings["azureSearchIndexName"];
            
            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
            return serviceClient.Indexes.GetClient(indexName);
        }
    }
}
