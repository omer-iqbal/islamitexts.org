using Newtonsoft.Json;

namespace IslamiTexts.Data.Models
{
    public class SurahDocument
    {
        [JsonProperty(PropertyName = PropertyNames.SurahNo)]
        public int SurahNo { get; set; }

        [JsonProperty(PropertyName = PropertyNames.ArNames)]
        public string[] ArabicNames { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EnNames)]
        public string[] EnglishNames { get; set; }

        [JsonProperty(PropertyName = PropertyNames.VerseCount)]
        public int VerseCount { get; set; }

        [JsonProperty(PropertyName = PropertyNames.Rukus)]
        public SurahRuku[] Rukus { get; set; }
    }
}
