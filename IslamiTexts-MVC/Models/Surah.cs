using System;

namespace IslamiTexts.Models
{
    public class Surah
    {
        public int SurahNo { get; set; }
        public string[] ArabicNames { get; set; }
        public string[] EnglishNames { get; set; }
        public int VerseCount { get; set; }

        public Ruku[] Rukus { get; set; }

        public Surah(int surahNo, string[] arabicNames, string[] englishNames, int verseCount)
        {
            this.SurahNo = surahNo;
            this.ArabicNames = arabicNames;
            this.EnglishNames = englishNames;
            this.VerseCount = verseCount;
        }

        public Ruku GetRukuForVerse(int verseNo)
        {
            if (verseNo < 1 || verseNo > this.VerseCount)
                throw new ArgumentOutOfRangeException(
                    $"The verse no must be between 0 and {this.VerseCount}.");

            foreach (Ruku ruku in this.Rukus)
            {
                if (verseNo >= ruku.StartVerse && verseNo <= ruku.EndVerse)
                {
                    return ruku;
                }
            }

            throw new InvalidOperationException(
                $"A ruku was not found for verse {verseNo} in surah {this.SurahNo}");
        }
    }
}
