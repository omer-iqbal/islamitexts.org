using Newtonsoft.Json;

namespace IslamiTexts.Data.Models
{
    public class SurahRuku
    {
        [JsonProperty(PropertyName = PropertyNames.RukuNo)]
        public int RukuNo { get; set; }

        [JsonProperty(PropertyName = PropertyNames.StartVerse)]
        public int StartVerse { get; set; }

        [JsonProperty(PropertyName = PropertyNames.EndVerse)]
        public int EndVerse { get; set; }
    }
}
