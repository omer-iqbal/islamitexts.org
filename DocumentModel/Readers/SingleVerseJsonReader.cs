using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace IslamiTexts.DocumentModel.Converters
{
    public class SingleVerseJsonReader : IVersesReader
    {
        public const string Name = @"single-verse-json";

        private static string YusufAliWeb = @"yusuf-ali-web";
        private static string YusufAliCommentary = @"yusuf-ali-commentary-1938";
        private static string Pickthall = @"pickthall-meaning-1930";
        private static string Shakir = @"shakir-web";
        private static string KhanHilali = @"khan-hilali-web";
        private static string Daryabadi = @"daryabadi-web";
        private static string DarwishQarib = @"darwish-qarib-web";
        private static string AyubKhan = @"ayub-khan-web";
        private static string SherAli = @"sher-ali-web";
        private static string MAsadWeb = @"m-asad-web";
        private static string MAsadMessage = @"m-asad-message-2003";
        private static string Arberry = @"arberry-web";
        private static string Maududi = @"maududi-meaning-quran";

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
            Stopwatch fileReaderWatch = new Stopwatch();
            Stopwatch deserializationWatch = new Stopwatch();

            Console.WriteLine($"Reading files from {this.versesPath}");
            List<string> versesFiles = new List<string>(Directory.GetFiles(versesPath));
            if (versesFiles.Count == 0)
            {
                throw new ArgumentException($"No verse files found at path {versesPath}");
            }

            List<string> missingFiles = new List<string>();
            int foundFilesCount = 0;
            Console.Write("Reading files,,");

            for (int surahNo = 1; surahNo <= 114; surahNo++)
            {
                for (int verseNo = 1; verseNo <= SurahMetadata.GetTotalVerses(surahNo); verseNo++)
                {
                    string filePath = Path.Combine(versesPath, $"{surahNo}-{verseNo}");
                    if (!versesFiles.Contains(filePath))
                    {
                        missingFiles.Add(Path.GetFileName(filePath));
                        continue;
                    }

                    fileReaderWatch.Start();
                    string jsonText = File.ReadAllText(filePath);
                    fileReaderWatch.Stop();

                    deserializationWatch.Start();
                    VerseOld oldVerse = VerseOld.DeserializeFromJson(jsonText);
                    deserializationWatch.Stop();

                    foundFilesCount++;
                    Console.Write($",{Path.GetFileName(filePath)}");
                    Convert(oldVerse, verses[surahNo, verseNo]);
                }
            }

            Console.WriteLine("");
            Console.WriteLine($"Total files read: {foundFilesCount}");
            Console.WriteLine($"Time to read files: {fileReaderWatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Time to deserialize files: {deserializationWatch.ElapsedMilliseconds} ms");

            if (missingFiles.Count > 0)
            {
                Console.WriteLine("");
                Console.WriteLine("The following verse files were not read: {0}", String.Join(',', missingFiles));
            }

            Console.WriteLine($"Read verses complete");
        }

        private static void Convert(VerseOld oldVerse, Verse newVerse)
        {
            newVerse.Translations.AddRange(new[]
            {
                new Verse.Translation { ReferenceId = YusufAliWeb, Text = oldVerse.en_tr_yusuf },
                new Verse.Translation { ReferenceId = Pickthall, Text = oldVerse.en_tr_pickth },
                new Verse.Translation { ReferenceId = Shakir, Text = oldVerse.en_tr_shakir },
                new Verse.Translation { ReferenceId = KhanHilali, Text = oldVerse.en_tr_hilali },
                new Verse.Translation { ReferenceId = Daryabadi, Text = oldVerse.en_tr_dbadi },
                new Verse.Translation { ReferenceId = DarwishQarib, Text = oldVerse.en_tr_qarib },
                new Verse.Translation { ReferenceId = AyubKhan, Text = oldVerse.en_tr_ayubk },
                new Verse.Translation { ReferenceId = SherAli, Text = oldVerse.en_tr_sher },
                new Verse.Translation { ReferenceId = MAsadWeb, Text = oldVerse.en_tr_asad },
                new Verse.Translation { ReferenceId = Arberry, Text = oldVerse.en_tr_arberry },
                new Verse.Translation { ReferenceId = Maududi, Text = oldVerse.en_tr_maududi }
            });

            Verse.Commentary yusufCmt = GetCommentary(YusufAliCommentary, oldVerse.en_ctr_yusuf, oldVerse.en_notes_yusuf);
            Verse.Commentary asadCmt = GetCommentary(MAsadMessage, oldVerse.en_ctr_asad, oldVerse.en_notes_asad);

            if (yusufCmt != null) newVerse.Commentaries.Add(yusufCmt);
            if (asadCmt != null) newVerse.Commentaries.Add(asadCmt);
        }

        private static Verse.Commentary GetCommentary(string workId, string oldTranslation, VerseOld.CommentaryNote[] oldNotes)
        {
            Verse.Commentary newCommentary = new Verse.Commentary
            { ReferenceId = workId, Translation = oldTranslation };

            List<Verse.CommentaryNote> newNotes = new List<Verse.CommentaryNote>();
            if (oldNotes != null)
            {
                foreach (VerseOld.CommentaryNote oldNote in oldNotes)
                {
                    Verse.CommentaryNote newNote = new Verse.CommentaryNote
                    {
                        Id = oldNote.note_no,
                        Text = oldNote.note
                    };

                    newNotes.Add(newNote);
                }
            }

            newCommentary.Notes = newNotes;
            return newCommentary;
        }
    }
}
