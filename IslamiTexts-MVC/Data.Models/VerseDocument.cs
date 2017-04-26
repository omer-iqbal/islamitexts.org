using System;
using IslamiTexts.Models;
using Newtonsoft.Json;

namespace IslamiTexts.Data.Models
{
    public class VerseDocument
    {
        [JsonProperty(PropertyName = PropertyNames.SurahNo)]
        public int SurahNo { get; set; }

        [JsonProperty(PropertyName = PropertyNames.VerseNo)]
        public int VerseNo { get; set; }

        [JsonProperty(PropertyName = PropertyNames.ArText)]
        public string ArabicText { get; set; }

        [JsonProperty(PropertyName = PropertyNames.ArTextClean)]
        public string ArabicTextClean { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnTrYusufAli)]
        public string EnTrYusufAli { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnTrPickthall)]
        public string EnTrPickthall { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnTrShakir)]
        public string EnTrShakir { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnTrHilali)]
        public string EnTrHilali { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnTrDaryabadi)]
        public string EnTrDaryabadi { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnTrQaribullah)]
        public string EnTrQaribullah { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnTrAyubKhan)]
        public string EnTrAyubKhan { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnTrSherAli)]
        public string EnTrSherAli { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnTrAsad)]
        public string EnTrAsad { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnTrArberry)]
        public string EnTrArberry { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnTrMaududi)]
        public string EnTrMaududi { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnCtrYusufAli)]
        public string EnCtrYusufAli { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnCtrAsad)]
        public string EnCtrAsad { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnNotesYusufAli)]
        public VerseNote[] EnNotesYusufAli { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnNotesAsad)]
        public VerseNote[] EnNotesAsad { get; set; }

        public string GetTranslation(Translator translator)
        {
            switch (translator)
            {
                case Translator.Arberry: return this.EnTrArberry;
                case Translator.Asad: return this.EnTrAsad;
                case Translator.AyubKhan: return this.EnTrAyubKhan;
                case Translator.Daryabadi: return this.EnTrDaryabadi;
                case Translator.Hilali: return this.EnTrHilali;
                case Translator.Maududi: return this.EnTrMaududi;
                case Translator.Pickthall: return this.EnTrPickthall;
                case Translator.Qaribullah: return this.EnTrQaribullah;
                case Translator.Shakir: return this.EnTrShakir;
                case Translator.SherAli: return this.EnTrSherAli;
                case Translator.YusufAli: return this.EnTrYusufAli;
                default: throw new NotSupportedException();
            }
        }
    }
}
