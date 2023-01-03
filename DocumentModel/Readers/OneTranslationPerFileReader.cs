using System;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace IslamiTexts.DocumentModel.Converters
{
    public class OneTranslationPerFileReader : IVersesReader
    {
        public const string Name = @"one-translation-per-file";

        private string versesPath;

        public void Initialize(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            versesPath = path;
        }

        public void Read(VerseCollection verses)
        {
            Stopwatch endToEndWatch = Stopwatch.StartNew();
            foreach (string filePath in Directory.GetFiles(versesPath))
            {
                string referenceId = Path.GetFileName(filePath);
                using (TextReader reader = File.OpenText(filePath))
                {
                    Read(reader, referenceId, verses);
                }
            }

            endToEndWatch.Stop();
            Console.WriteLine($"Total time taken for all files: {endToEndWatch.ElapsedMilliseconds}ms");
        }

        internal void Read(TextReader reader, string referenceId, VerseCollection verses)
        {
            Stopwatch fileWatch = Stopwatch.StartNew();
            Console.Write($"Starting processing {referenceId}...");
            int surahNo = 1;
            int verseNo = 0;
            int lineNo = 0;
            string line;

            StringBuilder currentTranslationText = new();

            line = reader.ReadLine();
            while (line != null)
            {
                if (line.Length > 0 && line[0] == '|')
                {
                    if (verseNo != 0)
                    {
                        currentTranslationText = AssignTranslation(referenceId, verses, surahNo, verseNo, currentTranslationText);
                    }

                    string[] splits = line.Split('|');
                    Trace.Assert(splits.Length == 4, $"Must have 4 components, but found {splits.Length} at line no. {lineNo}. Line:\n{line}");
                    int newSurahNo = Convert.ToInt32(splits[1]);
                    int newVerseNo = Convert.ToInt32(splits[2]);

                    Trace.Assert((newSurahNo == surahNo && newVerseNo == verseNo + 1) || (newSurahNo == surahNo + 1 && newVerseNo == 1),
                        $"Mismatched surah and verse no. read at line no. {lineNo}, new surah no: {newSurahNo}, " +
                        $"surah no: {surahNo}, new verse no: {newVerseNo}, verse no: {verseNo}. Line:\n{line}");

                    if (newSurahNo == surahNo + 1) { Console.Write($",{newSurahNo}"); }

                    surahNo = newSurahNo;
                    verseNo = newVerseNo;
                }
                else
                {
                    currentTranslationText.AppendLine(line);
                }

                line = reader.ReadLine();
                lineNo++;
            }

            if (currentTranslationText.Length > 0)
            {
                 AssignTranslation(referenceId, verses, surahNo, verseNo, currentTranslationText);
            }

            fileWatch.Stop();
            Console.WriteLine($", time taken: {fileWatch.ElapsedMilliseconds}ms");
        }

        private static StringBuilder AssignTranslation(string referenceId, VerseCollection verses, int surahNo, int verseNo, StringBuilder currentTranslationText)
        {
            // Assign previous line to a verse
            verses[surahNo, verseNo].Translations.Add(
                new Verse.Translation()
                {
                    ReferenceId = referenceId,
                    Text = currentTranslationText.ToString()
                });

            currentTranslationText = new();
            return currentTranslationText;
        }
    }
}