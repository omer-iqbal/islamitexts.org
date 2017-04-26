using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IslamiTexts.Models;

namespace IslamiTexts.Common.Web
{
    public class Pagination
    {
        public Pagination(SearchResults results, int noOfPagesToShow)
        {
            this.FirstItemIndex = results.FirstItemIndex;
            this.TotalMatches = results.TotalMatches;
            this.PageSize = results.PageSize;
            this.NoOfPagesToShow = noOfPagesToShow;

            this.CurrentPageNo = this.FirstItemIndex / this.PageSize + 1;

            this.FirstPageNoToShow = this.CurrentPageNo - (this.NoOfPagesToShow - 1) / 2;
            this.LastPageNoToShow = this.CurrentPageNo + this.NoOfPagesToShow / 2;

            if (this.FirstPageNoToShow < 1)
            {
                this.LastPageNoToShow += 1 - this.FirstPageNoToShow;
            }

            int lastPossiblePage = this.TotalMatches / this.PageSize + 1;
            if (this.LastPageNoToShow > lastPossiblePage)
            {
                this.FirstPageNoToShow -= this.LastPageNoToShow - lastPossiblePage;
            }

            this.FirstPageNoToShow = this.FirstPageNoToShow < 1 ? 1 : this.FirstPageNoToShow;
            this.LastPageNoToShow = this.LastPageNoToShow > lastPossiblePage ? lastPossiblePage : this.LastPageNoToShow;
        }

        public int TotalMatches { get; private set; }

        public int FirstItemIndex { get; private set; }

        public int PageSize { get; private set; }

        public int NoOfPagesToShow { get; private set; }

        public int CurrentPageNo { get; private set; }

        public int FirstPageNoToShow { get; private set; }

        public int LastPageNoToShow { get; private set; }

    }
}
