using System.Collections.Generic;

namespace IslamiTexts.Models
{
    public class VerseBlock
    {
        public int SurahNo { get; set; }

        public int FirstVerseNo { get; set; }

        public int LastVerseNo { get; set; }

        public Translator Translator { get; set; }

        public IList<VerseTranslation> VerseTranslations { get; set; }

        public int Count { get; set; }

        public int? PreviousBlockStartVerse { get; set; }

        public int? NextBlockStartVerse { get; set; }
    }
}
