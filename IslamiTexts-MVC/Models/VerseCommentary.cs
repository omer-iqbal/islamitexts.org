using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IslamiTexts.Models
{
    public class VerseCommentary
    {
        public string Text { get; set; }
        public Translator Commentator { get; set; }

        public IList<CommentaryNote> Notes { get; set; }

        public VerseCommentary()
        {
            Notes = new List<CommentaryNote>();
        }
    }
}
