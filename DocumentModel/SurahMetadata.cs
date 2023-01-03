using System;

namespace IslamiTexts.DocumentModel
{
    public class SurahMetadata
    {
        public const int TotalSurahs = 114;

        private static int[] surahTotalVerses = new[]
        {
            0,
            /*  1*/   7, 286, 200, 176, 120, 165, 206,  75, 129, 109,
            /* 11*/ 123, 111,  43,  52,  99, 128, 111, 110,  98, 135,
            /* 21*/ 112,  78, 118,  64,  77, 227,  93,  88,  69,  60,
            /* 31*/  34,  30,  73,  54,  45,  83, 182,  88,  75,  85,
            /* 41*/  54,  53,  89,  59,  37,  35,  38,  29,  18,  45,
            /* 51*/  60,  49,  62,  55,  78,  96,  29,  22,  24,  13,
            /* 61*/  14,  11,  11,  18,  12,  12,  30,  52,  52,  44,
            /* 71*/  28,  28,  20,  56,  40,  31,  50,  40,  46,  42,
            /* 81*/  29,  19,  36,  25,  22,  17,  19,  26,  30,  20,
            /* 91*/  15,  21,  11,   8,   8,  19,   5,   8,   8,  11,
            /*101*/  11,   8,   3,   9,   5,   4,   7,   3,   6,   3,
            /*111*/   5,   4,   5,   6
        };

        public static int GetTotalVerses(int surahNo)
        {
            if (surahNo < 1 || surahNo > 114)
            {
                throw new ArgumentException($"Invalid surahNo provided: {surahNo}");
            }

            return surahTotalVerses[surahNo];
        }
    }
}
