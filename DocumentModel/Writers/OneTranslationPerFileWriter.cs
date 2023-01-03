using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static IslamiTexts.DocumentModel.Verse;

namespace IslamiTexts.DocumentModel.Writers
{
    public class OneTranslationPerFileWriter : IVersesWriter
    {
        public const string Name = @"one-translation-per-file";

        private string versesPath;

        public void Initialize(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            versesPath = path;
        }

        public void Write(VerseCollection verses)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            // Create one file per reference work
            foreach (string translationWork in verses.GetTranslationReferenceIds())
            {
                string filePath = Path.Combine(versesPath, translationWork);
                Console.WriteLine($"Writing translation file: {translationWork}");
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    for (int surahNo = 1; surahNo <= 114; surahNo++)
                    {
                        int verseCount = SurahMetadata.GetTotalVerses(surahNo);
                        for (int verseNo = 1; verseNo <= verseCount; verseNo++)
                        {
                            writer.WriteLine($"|{surahNo}|{verseNo}|");
                            Translation translation = verses[surahNo, verseNo].Translations.Where(
                                t => t.ReferenceId.Equals(translationWork)).Single();
                            writer.Write(translation.Text);
                        }
                    }
                }
            }

            // Create one file per reference work
            foreach (string commentaryWork in verses.GetCommentaryReferenceIds())
            {
                string filePath = Path.Combine(versesPath, commentaryWork);
                Console.WriteLine($"Writing translation file: {commentaryWork}");
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    for (int surahNo = 1; surahNo <= 114; surahNo++)
                    {
                        int verseCount = SurahMetadata.GetTotalVerses(surahNo);
                        for (int verseNo = 1; verseNo <= verseCount; verseNo++)
                        {
                            writer.WriteLine($"|{surahNo}|{verseNo}|");
                            Commentary commentary = verses[surahNo, verseNo].Commentaries.Where(
                                t => t.ReferenceId.Equals(commentaryWork)).Single();
                            string translation = commentary.Translation;
                            translation = translation.Replace("{{", "[^").Replace("}}", "]");
                            writer.WriteLine(translation);
                            writer.WriteLine();

                            if (commentary.Notes.Count > 0)
                            {
                                foreach (CommentaryNote note in commentary.Notes) 
                                {
//                                    string text = note.Text.Replace("<br/>", Environment.NewLine + Environment.NewLine);
                                    writer.WriteLine($"[^{note.Id}] {note.Text}");
                                    writer.WriteLine();
                                }
                            }
                        }
                    }
                }
            }

            watch.Stop();
            Console.WriteLine($"Time taken to write files: {watch.ElapsedMilliseconds} ms");
        }
    }
}
