using System;
using IslamiTexts.Models;

namespace IslamiTexts.Data.Models
{
    public static class TranslatorConverter
    {
        public static string GetTranslatorPropertyName(Translator translator)
        {
            switch (translator)
            {
                case Translator.Arberry: return PropertyNames.EnTrArberry;
                case Translator.Asad: return PropertyNames.EnTrAsad;
                case Translator.AyubKhan: return PropertyNames.EnTrAyubKhan;
                case Translator.Daryabadi: return PropertyNames.EnTrDaryabadi;
                case Translator.Hilali: return PropertyNames.EnTrHilali;
                case Translator.Maududi: return PropertyNames.EnTrMaududi;
                case Translator.Pickthall: return PropertyNames.EnTrPickthall;
                case Translator.Qaribullah: return PropertyNames.EnTrQaribullah;
                case Translator.Shakir: return PropertyNames.EnTrShakir;
                case Translator.SherAli: return PropertyNames.EnTrSherAli;
                case Translator.YusufAli: return PropertyNames.EnTrYusufAli;
                default: throw new NotSupportedException();
            }
        }
    }
}