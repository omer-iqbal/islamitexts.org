using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IslamiTexts.Models
{
    public class VerseResult : ISearchResult
    {
        public ContentType ResultType { get; set; }
        public int SurahNo { get; set; }
        public int VerseNo { get; set; }
        public string ArabicText { get; set; }
        public string Translation { get; set; }

    }
}
