namespace IslamiTexts.Models
{
    public class Surah
    {
        public int SurahNo { get; set; }
        public string[] ArabicNames { get; set; }
        public string[] EnglishNames { get; set; }
        public int VerseCount { get; set; }

        public Surah(int surahNo, string[] arabicNames, string[] englishNames, int verseCount)
        {
            this.SurahNo = surahNo;
            this.ArabicNames = arabicNames;
            this.EnglishNames = englishNames;
            this.VerseCount = verseCount;
        }
    }
}
