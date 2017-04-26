using IslamiTexts.Models;

namespace IslamiTexts.Data.Models
{
    public class VerseTranslationDocument
    {
        public int SurahNo { get; set; }

        public int VerseNo { get; set; }

        public string ArabicText { get; set; }

        public string TranslatedText { get; set; }

        public Translator Translator { get; set; }
    }
}
