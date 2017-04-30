using Newtonsoft.Json;

namespace IslamiTexts.Data.Models
{
    public class SurahRuku
    {
        [JsonProperty(PropertyName = PropertyNames.RukuNo)]
        public int RukuNo { get; set; }

        [JsonProperty(PropertyName = PropertyNames.VerseStart)]
        public int VerseStart { get; set; }

        [JsonProperty(PropertyName = PropertyNames.VerseEnd)]
        public int VerseEnd { get; set; }
    }
}
