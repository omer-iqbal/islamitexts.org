using System;
using System.IO;
using System.Linq;

namespace IslamiTexts.DocumentModel.Transformations
{
    public class ReplaceBrWithBackslashTransformation : ITransformation
    {
        public const string Name = @"replace-br-with-backslash";

        public void Apply(VerseCollection verses, string translationReferenceId)
        {
            foreach (Verse verse in verses)
            {
                Verse.Translation translation = verse.Translations.Where
                    (t => t.ReferenceId.Equals(translationReferenceId)).SingleOrDefault();
                if (translation == null)
                {
                    throw new InvalidDataException($"A translation with Reference Id {translationReferenceId} not found.");
                }

                translation.Text = translation.Text.Replace("<br/>", Environment.NewLine + Environment.NewLine);
            }
        }
    }
}
