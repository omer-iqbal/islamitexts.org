namespace IslamiTexts.Common.Web
{
    public static class Links
    {
        public static string GetVerseLink(int surahNo, int verseNo, bool anchorLink = true)
        {
            if (anchorLink)
                return $"/quran/{surahNo}/{verseNo}#{surahNo}:{verseNo}";
            else
                return $"/quran/{surahNo}/{verseNo}";
        }
    }
}
