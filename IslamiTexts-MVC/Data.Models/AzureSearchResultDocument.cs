using Newtonsoft.Json;

namespace IslamiTexts.Data.Models
{
    public class AzureSearchResultDocument
    {
        [JsonProperty(PropertyName = PropertyNames.Type)]
        public string Type { get; set; }

        [JsonProperty(PropertyName = PropertyNames.SurahNo)]
        public int SurahNo { get; set; }

        [JsonProperty(PropertyName = PropertyNames.VerseNo)]
        public int VerseNo { get; set; }

        [JsonProperty(PropertyName = PropertyNames.ArText)]
        public string ArabicText { get; set; }
    }
}
