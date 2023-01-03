using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace IslamiTexts.DocumentModel.Converters
{
    public class IslamicStudiesDotInfoReader : IVersesReader
    {
        public const string Name = @"islamicissues.info-text";
        public const string ReferenceId = @"maududi-towards-understanding-quran";

        private readonly Regex VerseRegex = new(@"^\((?<surahNo>\d{1,3}):(?<verseNo>\d{1,3})\)(?<translation>.+)$", RegexOptions.Compiled);

        private const string NoteNoInTranslationPattern = @"(?<noteNo>\d{1,3}a?)";
        private readonly Regex NoteNoInTranslationRegex = new(NoteNoInTranslationPattern, RegexOptions.Compiled);

        private readonly Regex NoteRegex = new(@"^(?<noteNo>\d{1,3}a?)\.(?<noteText>.+)$", RegexOptions.Compiled);

        private string versesPath;

        private enum State
        {
            Start,
            Verse,
            Note
        }

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
            for (int surahNo = 1; surahNo <= 114; surahNo++)
            {
                string filePath = Path.Combine(versesPath, surahNo.ToString());
                using (TextReader reader = File.OpenText(filePath))
                {
                    Read(reader, surahNo, verses);
                }
            }
        }

        internal void Read(TextReader reader, int surahNo, VerseCollection verses)
        {
            int verseNo = 0;
            int lineNo = 0;
            string line = ReadUntilVerse(reader, ref lineNo, surahNo, 1);
            if (line == null)
            {
                throw new InvalidDataException($"No verses found.");
            }

            Queue<string> pendingNoteIds = new();
            State currentState = State.Start;
            Verse.Commentary currentCommentary = new();
            StringBuilder currentNoteText = new();
            StringBuilder currentTranslationText = new();
            Verse.CommentaryNote currentCommentaryNote = new();

            while (line != null)
            {
                int foundSurahNo;
                int foundVerseNo;
                string foundNoteId;
                string text;

                Trace.Assert(!line.Trim().StartsWith("Sura", StringComparison.OrdinalIgnoreCase), $"Line no {lineNo} in " +
                    $"surah {surahNo} starts with the word Surah, line:\n{line}");

                if (IsTranslation(line, out foundSurahNo, out foundVerseNo, out text))
                {
                    Trace.Assert(pendingNoteIds.Count == 0, $"Reading new translation line at {surahNo}:{verseNo}, " +
                        $"should have 0 pending notes but have {pendingNoteIds.Count} at line {lineNo}.");
                    ProcessTranslationOrNote(verses, surahNo, verseNo, currentState, currentTranslationText,
                        currentNoteText, ref currentCommentaryNote, ref currentCommentary);

                    // Validate and start processing the new verse
                    verseNo++;
                    Trace.Assert(surahNo == foundSurahNo, $"Expected surah no. {surahNo}, found {foundSurahNo} at " +
                        $"line {lineNo}.");
                    Trace.Assert(verseNo == foundVerseNo, $"Expected verse no. {verseNo}, found {foundVerseNo} in " +
                        $"{surahNo} at line {lineNo}.");

                    string newText = AddNoteNumbers(text, pendingNoteIds);

                    currentTranslationText = new StringBuilder();
                    currentTranslationText.AppendLine(newText);
                    currentCommentary = new Verse.Commentary();
                    currentState = State.Verse;
                }
                else if (IsNote(line, out foundNoteId, out text))
                {
                    Trace.Assert(currentState == State.Verse || currentState == State.Note, $"Note found " +
                        $"unexpectedly at {surahNo}:{verseNo} at line {lineNo}, current state: {currentState}.");
                    Trace.Assert(pendingNoteIds.Count > 0, $"Did not expect notes, but found {foundNoteId} " +
                        $"at {surahNo}:{verseNo} at line {lineNo}.");

                    string expectedNoteId = pendingNoteIds.Dequeue();
                    Trace.Assert(String.Equals(expectedNoteId, foundNoteId, StringComparison.Ordinal),
                        $"Expected note no. {expectedNoteId}, found {foundNoteId} at {surahNo}:{verseNo} at line {lineNo}.");

                    if (currentState == State.Note)
                    {
                        currentCommentaryNote.Text = currentNoteText.ToString().Trim();
                        currentCommentary.Notes.Add(currentCommentaryNote);
                    }
                    else if (currentState == State.Verse)
                    {
                        currentCommentary.Translation = currentTranslationText.ToString().Trim();
                    }

                    currentCommentaryNote = new Verse.CommentaryNote();
                    currentNoteText = new StringBuilder();
                    currentNoteText.AppendLine(text);
                    currentCommentaryNote.Id = foundNoteId.ToString();
                    currentState = State.Note;
                }
                else if (IsArabic(line, lineNo, surahNo, verseNo))
                {
                    // Totally ignore this line
                }
                else
                {
                    if (currentState == State.Note)
                    {
                        currentNoteText.AppendLine(line);
                    }
                    else if (currentState == State.Verse)
                    {
                        string newLine = AddNoteNumbers(line, pendingNoteIds);
                        currentTranslationText.AppendLine(newLine);
                    }
                    else
                    {
                        throw new InvalidDataException($"Unexpected line found at {surahNo}:{verseNo} when " +
                        $"processing {currentState} at line {lineNo}.");
                    }
                }

                line = reader.ReadLine();
                lineNo++;
            }

            Trace.Assert(pendingNoteIds.Count == 0, $"End of file reached at line {lineNo} for {surahNo}:{verseNo}, " +
                $"should have 0 pending notes but have {pendingNoteIds.Count}.");
            ProcessTranslationOrNote(verses, surahNo, verseNo, currentState, currentTranslationText,
                currentNoteText, ref currentCommentaryNote, ref currentCommentary);
        }

        private void ProcessTranslationOrNote(VerseCollection verses, int surahNo, int verseNo, State currentState, 
            StringBuilder currentTranslationText, StringBuilder currentNoteText,
            ref Verse.CommentaryNote currentCommentaryNote, ref Verse.Commentary currentCommentary)
        {
            currentCommentary.ReferenceId = ReferenceId;
            if (currentState == State.Note)
            {
                currentCommentaryNote.Text = currentNoteText.ToString().Trim();
                currentCommentary.Notes.Add(currentCommentaryNote);
                verses[surahNo, verseNo].Commentaries.Add(currentCommentary);
            }
            else if (currentState == State.Verse)
            {
                currentCommentary.Translation = currentTranslationText.ToString().Trim();
                verses[surahNo, verseNo].Commentaries.Add(currentCommentary);
            }
        }

        internal void ValidateAndAddNote(Verse verse, Queue<int> pendingNoteNos, string note)
        {
            if (pendingNoteNos.Count > 0)
            {
                throw new InvalidDataException($"");
            }
        }

        internal string AddNoteNumbers(string translation, Queue<string> noteNos)
        {
            MatchCollection matches = NoteNoInTranslationRegex.Matches(translation);

            foreach (Match m in matches)
            {
                noteNos.Enqueue(m.Groups[@"noteNo"].Value);
            }

            return Regex.Replace(translation, NoteNoInTranslationPattern, "[^$1]");
        }

        internal bool IsTranslation(string line, out int surahNo, out int verseNo, out string translation)
        {
            Match m = VerseRegex.Match(line);

            if (m.Success)
            {
                surahNo = Convert.ToInt32(m.Groups["surahNo"].Value);
                verseNo = Convert.ToInt32(m.Groups["verseNo"].Value);
                translation = m.Groups["translation"].Value.Trim();
                return true;
            }

            surahNo = 0;
            verseNo = 0;
            translation = null;
            return false;
        }

        internal bool IsArabic(string line, int lineNo, int surahNo, int verseNo)
        {
            bool foundAscii = false;
            bool foundOther = false;
            int foundAsciiAt = 0;
            char asciiChar = '\0';
            int foundOtherAt = 0;
            char otherChar = '\0';
            for (int i=0; i<line.Length; i++)
            {
                char c = line[i];
                if (!foundAscii && c > 64 && c <= 256)
                {
                    foundAscii = true;
                    foundAsciiAt = i;
                    asciiChar = c;
                }
                else if (c == 339 || (c > 0x2000 && c < 0x2200))
                {
                    // TODO: Temporary to ignore currency symbol, smart quotes, TM, etc.
                    continue;
                }
                else if (!foundOther && c > 256)
                { 
                    foundOther = true;
                    foundOtherAt = i;
                    otherChar = c;
                }
            }

            if (foundAscii && foundOther)
            {
                throw new InvalidDataException($"Found Ascii '{asciiChar}'{(int)asciiChar} at {foundAsciiAt} and non-Ascii '{otherChar}'{(int)otherChar} " +
                    $"at {foundOtherAt} on line {lineNo} in surah {surahNo}, verse {verseNo}, Text:\n{line}");
            }

            return foundOther;
        }

        internal bool IsNote(string line, out string noteId, out string text)
        {
            Match m = NoteRegex.Match(line);

            if (m.Success)
            {
                noteId = m.Groups["noteNo"].Value;
                text = m.Groups["noteText"].Value.TrimStart();
                return true;
            }

            noteId = null;
            text = null;
            return false;
        }

        internal string ReadUntilVerse(TextReader reader, ref int lineNo, int surahNo, int verseNo)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lineNo++;
                if (line.StartsWith($"({surahNo}:{verseNo})"))
                {
                    return line;
                }
            }

            return null;
        }
    }
}