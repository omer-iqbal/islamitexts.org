using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IslamiTexts.Models
{
    public class SearchResults
    {
        public int TotalMatches { get; private set; }

        public int FirstItemIndex { get; private set; }

        public int PageSize { get; private set; }

        public int NoOfPagesToShow { get; private set; }

        public IList<ISearchResult> ResultItems { get; set; }

        public SearchResults(int firstItemIndex, int pageSize, int totalCountOfMatches)
        {
            this.ResultItems = new List<ISearchResult>();
            this.FirstItemIndex = firstItemIndex;
            this.PageSize = pageSize;
            this.TotalMatches = totalCountOfMatches;
        }
    }
}
