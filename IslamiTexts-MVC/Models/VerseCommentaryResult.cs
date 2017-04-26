using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IslamiTexts.Models
{
    public class VerseCommentaryResult : ISearchResult
    {
        public int SurahNo { get; set; }
        public int VerseNo { get; set; }
        public int NoteNo { get; set; }
        public IList<string> Snippets { get; set; }
        public string Commentator { get; set; }
        
        public ContentType ResultType { get; set; }

        public VerseCommentaryResult()
        {
            this.Snippets = new List<string>();
        }
    }
}
