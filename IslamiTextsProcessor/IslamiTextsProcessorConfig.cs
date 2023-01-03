using System.Xml.Serialization;

namespace IslamiTextsProcessor
{
    public class IslamiTextsProcessorConfig
    {
        public static IslamiTextsProcessorConfig Deserialize(string path)
        {
            XmlSerializer serializer = new(typeof(IslamiTextsProcessorConfig));
            IslamiTextsProcessorConfig config;

            using (StreamReader reader = new(path))
            {
                config = (IslamiTextsProcessorConfig)serializer.Deserialize(reader);
            }

            return config;
        }

        [XmlElement]
        public string? Operation;

        public Source? Input;

        public List<Transformation>? Transformations;

        public Source? Output;
    }

    public class Source
    {
        [XmlElement]
        public string? Format;

        [XmlElement]
        public string? Path;
    }

    public class Transformation
    {
        [XmlAttribute]
        public string? Id;

        [XmlAttribute]
        public string? ApplyTo;
    }
}
