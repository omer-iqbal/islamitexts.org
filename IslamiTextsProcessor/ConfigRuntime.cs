using IslamiTexts.DocumentModel;
using IslamiTexts.DocumentModel.Converters;
using IslamiTexts.DocumentModel.Transformations;
using IslamiTexts.DocumentModel.Writers;

namespace IslamiTextsProcessor
{
    internal class ConfigRuntime
    {
        static readonly IDictionary<string, IVersesReader> Readers = 
            new Dictionary<string, IVersesReader>(StringComparer.OrdinalIgnoreCase)
        {
            { SingleVerseJsonReader.Name, new SingleVerseJsonReader() },
            { IslamicStudiesDotInfoReader.Name, new IslamicStudiesDotInfoReader() },
            { OneTranslationPerFileReader.Name, new OneTranslationPerFileReader() }
        };

        static readonly IDictionary<string, IVersesWriter> Writers = 
            new Dictionary<string, IVersesWriter>(StringComparer.OrdinalIgnoreCase)
        {
            { OneTranslationPerFileWriter.Name, new OneTranslationPerFileWriter() }
        };

        static readonly IDictionary<string, ITransformation> Transformations = 
            new Dictionary<string, ITransformation>(StringComparer.OrdinalIgnoreCase)
        {
            { ReplaceBrWithBackslashTransformation.Name, new ReplaceBrWithBackslashTransformation() }
        };

        public static void ConvertVerses(IslamiTextsProcessorConfig config)
        {
            IVersesReader versesReader;
            if (!Readers.TryGetValue(config.Input.Format, out versesReader))
            {
                throw new InvalidOperationException($"InputSource Format {config.Input.Format} is invalid.");
            }

            Console.WriteLine($"Invoking convert verses: {config.Input.Format}");
            VerseCollection verses = new VerseCollection();
            versesReader.Initialize(config.Input.Path);
            versesReader.Read(verses);

            foreach (Transformation transformConfig in config.Transformations)
            {
                ITransformation transformation;
                if (!Transformations.TryGetValue(transformConfig.Id, out transformation))
                {
                    throw new InvalidOperationException($"Transformation with Id {transformConfig.Id} not found.");
                }

                transformation.Apply(verses, transformConfig.ApplyTo);
            }

            IVersesWriter versesWriter;
            if (!Writers.TryGetValue(config.Output.Format, out versesWriter))
            {
                throw new InvalidOperationException($"OutputPath Format {config.Output.Format} is invalid.");
            }

            versesWriter.Initialize(config.Output.Path);
            versesWriter.Write(verses);

            Console.WriteLine("Done.");
        }
    }
}
