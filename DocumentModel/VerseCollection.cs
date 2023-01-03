using System;
using static IslamiTexts.DocumentModel.Verse;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IslamiTexts.DocumentModel
{
    public class VerseCollection : IEquatable<VerseCollection>, IEnumerable<Verse>
    {
        private Verse[][] verseMap = new Verse[115][];

        public VerseCollection()
        {
            for (int surahNo = 1; surahNo <= SurahMetadata.TotalSurahs; surahNo++)
            {
                int verseCount = SurahMetadata.GetTotalVerses(surahNo);
                verseMap[surahNo - 1] = new Verse[verseCount];

                for (int verseNo = 1; verseNo <= verseMap[surahNo - 1].Length; verseNo++)
                {
                    verseMap[surahNo - 1][verseNo - 1] = new Verse();
                    verseMap[surahNo - 1][verseNo - 1].SurahNo = surahNo;
                    verseMap[surahNo - 1][verseNo - 1].VerseNo = verseNo;
                }
            }
        }

        public Verse this[int surahNo, int verseNo]
        {
            get
            {
                ValidateSurahAndVerseNos(surahNo, verseNo);
                return verseMap[surahNo - 1][verseNo - 1];
            }
        }

        public void CheckTranslationIntegrity(string translationReferenceId)
        {
        }

        public void CheckCommentaryIntegrity(string commentaryReferenceId)
        {
        }

        public string[] GetTranslationReferenceIds()
        {
            HashSet<string> translationWorks = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (Translation translation in verseMap[0][0].Translations)
            {
                translationWorks.Add(translation.ReferenceId);
            }

            return translationWorks.ToArray();
        }

        public string[] GetCommentaryReferenceIds()
        {
            HashSet<string> commentaryWorks = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (Commentary commentary in verseMap[0][0].Commentaries)
            {
                commentaryWorks.Add(commentary.ReferenceId);
            }

            return commentaryWorks.ToArray();
        }

        private void ValidateSurahAndVerseNos(int surahNo, int verseNo)
        {
            if (surahNo < 1 || surahNo > 114) throw new ArgumentException($"Invalid surahNo provided: {surahNo}");
            if (verseNo < 1 || verseNo > verseMap[surahNo - 1].Length) throw new ArgumentException(
                $"Invalid verseNo {verseNo} provided for surahNo {surahNo}.");

        }

        public override bool Equals(object obj)
        {
            VerseCollection verses = obj as VerseCollection;
            if (verses == null) return false;
            return this.Equals(verses);
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            for (int surahNo = 1; surahNo <= SurahMetadata.TotalSurahs; surahNo++)
            {
                int verseCount = SurahMetadata.GetTotalVerses(surahNo);
                for (int verseNo = 1; verseNo < verseCount; verseNo++)
                {
                    hashCode ^= this[surahNo, verseNo].GetHashCode();
                }
            }

            return hashCode;
        }

        public bool Equals(VerseCollection other)
        {
            if (other == null) return false;

            // First, match the translation, commentary counts and simple fields.
            // If those differ, then we save string comparisons.
            for (int surahNo = 1; surahNo <= SurahMetadata.TotalSurahs; surahNo++)
            {
                int verseCount = SurahMetadata.GetTotalVerses(surahNo);
                for (int verseNo = 1; verseNo < verseCount; verseNo++)
                {
                    if (other[surahNo, verseNo].SurahNo != this[surahNo, verseNo].SurahNo) return false;
                    if (other[surahNo, verseNo].VerseNo != this[surahNo, verseNo].VerseNo) return false;
                    if (!String.Equals(other[surahNo, verseNo].Language, this[surahNo, verseNo].Language)) return false;
                    if (other[surahNo, verseNo].Translations.Count != this[surahNo, verseNo].Translations.Count) return false;
                    if (other[surahNo, verseNo].Commentaries.Count != this[surahNo, verseNo].Commentaries.Count) return false;
                }
            }

            // First, match the translation and commentary counts. If those differ, then we save string comparisons.
            for (int surahNo = 1; surahNo <= SurahMetadata.TotalSurahs; surahNo++)
            {
                int verseCount = SurahMetadata.GetTotalVerses(surahNo);
                for (int verseNo = 1; verseNo < verseCount; verseNo++)
                {
                    Verse thisVerse = this[surahNo, verseNo];
                    Verse otherVerse = other[surahNo, verseNo];
                    for (int translationNo = 0; translationNo < thisVerse.Translations.Count; translationNo++)
                    {
                        if (!String.Equals(thisVerse.Translations[translationNo].ReferenceId, otherVerse.Translations[translationNo].ReferenceId)) return false;
                        if (!String.Equals(thisVerse.Translations[translationNo].Text, otherVerse.Translations[translationNo].Text)) return false;
                    }

                    for (int commentaryNo = 0; commentaryNo < thisVerse.Commentaries.Count; commentaryNo++)
                    {
                        Verse.Commentary thisCommentary = thisVerse.Commentaries[commentaryNo];
                        Verse.Commentary otherCommentary = otherVerse.Commentaries[commentaryNo];
                        if (!String.Equals(thisCommentary.ReferenceId, otherCommentary.ReferenceId)) return false;
                        if (!String.Equals(thisCommentary.Translation, otherCommentary.Translation)) return false;

                        for (int noteNo = 0; noteNo < thisCommentary.Notes.Count; noteNo++)
                        {
                            Verse.CommentaryNote thisNote = thisCommentary.Notes[noteNo];
                            Verse.CommentaryNote otherNote = otherCommentary.Notes[noteNo];
                            if (!String.Equals(thisNote.Id, otherNote.Id)) return false;
                            if (!String.Equals(thisNote.Text, otherNote.Text)) return false;
                        }
                    }
                }
            }

            return true;
        }

        public IEnumerator<Verse> GetEnumerator()
        {
            for (int surahNo = 1; surahNo <= SurahMetadata.TotalSurahs; surahNo++)
            {
                for (int verseNo = 1; verseNo < SurahMetadata.GetTotalVerses(surahNo); verseNo++)
                {
                    yield return verseMap[surahNo - 1][verseNo - 1];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
