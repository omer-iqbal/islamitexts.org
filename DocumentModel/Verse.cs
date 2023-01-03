using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace IslamiTexts.DocumentModel
{
    public class Verse
    {
        public Verse() { }

        public int SurahNo { get; set; }

        public int VerseNo { get; set; }

        public string Language { get; set; }

        public string ArabicText { get; set; }

        public string ArabicTextClean { get; set; }

        public List<Translation> Translations { get; } = new();

        public List<Commentary> Commentaries { get; } = new();

        public override int GetHashCode()
        {
            StringBuilder hashString = new();
            hashString.Append('(');
            hashString.Append(SurahNo);
            hashString.Append(':');
            hashString.Append(VerseNo);
            hashString.Append(')');
            hashString.Append(Language ?? String.Empty);
            hashString.Append('|');
            hashString.Append(ArabicText ?? String.Empty);
            hashString.Append('|');
            hashString.Append(ArabicTextClean ?? String.Empty);
            hashString.Append('|');
            foreach (Translation t in this.Translations)
            {
                hashString.Append('|');
                hashString.Append(t.ReferenceId ?? String.Empty);
                hashString.Append('|');
                hashString.Append(t.Text ?? String.Empty);
                hashString.Append('|');
            }

            hashString.Append('|');
            foreach (Commentary c in this.Commentaries)
            {
                hashString.Append('|');
                hashString.Append(c.ReferenceId ?? String.Empty);
                hashString.Append('|');
                hashString.Append(c.Translation ?? String.Empty);
                hashString.Append('|');

                foreach (CommentaryNote n in c.Notes)
                {
                    hashString.Append('|');
                    hashString.Append(n.Id ?? String.Empty);
                    hashString.Append('|');
                    hashString.Append(n.Text ?? String.Empty);
                    hashString.Append('|');
                }
            }

            return hashString.ToString().GetHashCode();
        }
        public string SerializeToXml()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Verse));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, this);
                return textWriter.ToString();
            }
        }

        public class Translation
        {
            public string ReferenceId { get; set; }

            public string Text { get; set; }
        }

        public class Commentary
        {
            public string ReferenceId { get; set; }

            public string Translation { get; set; }

            public List<CommentaryNote> Notes { get; set; } = new();
        }

        public class CommentaryNote
        {
            public string Id { get; set; }

            public string Text { get; set; }
        }
    }
}