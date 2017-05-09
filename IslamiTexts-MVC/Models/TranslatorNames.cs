using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IslamiTexts.Models
{
    public static class Translators
    {
        static IDictionary<Translator, string> translatorNames = new Dictionary<Translator, string>
        {
            { Translator.Arberry,    "Arthur Arberry" },
            { Translator.Asad,       "Muhammad Asad" },
            { Translator.AyubKhan,   "Ayub Khan" },
            { Translator.Daryabadi,  "Abdul-Majid Daryabadi" },
            { Translator.Hilali,     "Taqiuddin Hilali and M. Mohsin Khan" },
            { Translator.Maududi,    "Abu'l Ala Maududi" },
            { Translator.Pickthall,  "Marmaduke Pickthall" },
            { Translator.Qaribullah, "Hasan Qaribullah and Ahmed Darwish" },
            { Translator.Shakir,     "M. Habib Shakir" },
            { Translator.SherAli,    "Sher Ali" },
            { Translator.YusufAli,   "Abdullah Yusuf Ali" }
        };

        public static string GetName(Translator translator)
        {
            return translatorNames[translator];
        }
    }
}