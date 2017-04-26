using Newtonsoft.Json;

namespace IslamiTexts.Data
{
    public class VerseNote
    {
        [JsonProperty(PropertyName = "note_no")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "note")]
        public string Text { get; set; }
    }
}
