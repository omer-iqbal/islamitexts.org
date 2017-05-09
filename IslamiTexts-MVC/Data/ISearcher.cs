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
    public interface ISearcher
    {
        SearchResults GetSearchResults(
            SearchScope searchScope,
            string searchString,
            Translator preferredSnippetTranslator,
            int start,
            int resultsToFetch);
    }
}
