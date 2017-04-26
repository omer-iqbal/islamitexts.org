using System.Collections.Generic;

namespace IslamiTexts.Models
{
    public class Verse
    {
        public int SurahNo { get; set; }

        public int VerseNo { get; set; }

        public string ArabicText { get; set; }

        public IList<TranslatedText> TranslatedTexts { get; set; }

        public IList<VerseCommentary> VerseCommentaries { get; set; }

        public Verse()
        {
            TranslatedTexts = new List<TranslatedText>();
            VerseCommentaries = new List<VerseCommentary>();
        }
    }
}
