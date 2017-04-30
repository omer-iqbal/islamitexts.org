using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IslamiTexts.Models;
using IslamiTexts.Data.Models;
using System.Web.Hosting;

namespace IslamiTexts.Data
{
    public class DocumentRepository
    {
        private const string SurahListPath = @"~/App_Data/Surahs.json";
        private readonly static Surah[] surahs = new Surah[114];

        public DocumentRepository()
        {
            string mappedPath = HostingEnvironment.MapPath(SurahListPath);
            string surahsJson = File.ReadAllText(mappedPath);

            SurahDocument[] surahDocuments = JsonConvert.DeserializeObject<SurahDocument[]>(surahsJson);

            foreach (SurahDocument document in surahDocuments)
            {
                Surah surah = new Surah(
                    document.SurahNo, 
                    document.ArabicNames, 
                    document.EnglishNames, 
                    document.VerseCount);
                surahs[surah.SurahNo - 1] = surah;
            }
        }

        public async Task<Verse> GetVerseAsync(int surahNo, int verseNo)
        {
            IEnumerable<VerseDocument> verses = await DocumentDBRepository<VerseDocument>.GetItemsAsync(
                doc => doc.SurahNo == surahNo && doc.VerseNo == verseNo);
            VerseDocument verseDocument = verses.Single();

            Verse verse = GetVerse(verseDocument);

            return verse;
        }

        public async Task<VerseBlock> GetOrderedVersesAsync(
            int surahNo, int startVerseNo, int endVerseNo, Translator translator)
        {
            IList<VerseTranslationDocument> verseDocuments = await DocumentDBVerses.GetVersesAsync(
                surahNo, startVerseNo, endVerseNo);

            if (verseDocuments.Count == 0)
            {
                return null;
            }

            VerseTranslationDocument firstVerseDocument = verseDocuments[0];
            VerseTranslationDocument lastVerseDocument = verseDocuments[verseDocuments.Count - 1];

            VerseBlock verseBlock = new VerseBlock
            {
                SurahNo = firstVerseDocument.SurahNo,
                Translator = translator,
                VerseTranslations = GetOrderedVerseTranslations(verseDocuments),
                Count = verseDocuments.Count
            };

            // Set the first and last verse nos after ordered translations have been received
            verseBlock.FirstVerseNo = verseBlock.VerseTranslations[0].VerseNo;
            verseBlock.LastVerseNo = verseBlock.VerseTranslations[verseBlock.Count - 1].VerseNo;

            return verseBlock;
        }

        public IList<Surah> GetSurahs()
        {
            return surahs;
        }

        private IList<VerseTranslation> GetOrderedVerseTranslations(IList<VerseTranslationDocument> documents)
        {
            return documents.Select(
                doc => new VerseTranslation
                {
                    SurahNo = doc.SurahNo,
                    VerseNo = doc.VerseNo,
                    ArabicText = doc.ArabicText,
                    TranslatedText = doc.TranslatedText
                })
                .OrderBy(doc => doc.VerseNo)
                .ToList();
        }

        private Verse GetVerse(VerseDocument verseDocument)
        {
            Verse verse = new Verse
            {
                SurahNo = verseDocument.SurahNo,
                VerseNo = verseDocument.VerseNo,
                ArabicText = verseDocument.ArabicText,
            };

            List<TranslatedText> translatedTexts = new List<TranslatedText>();
            translatedTexts.Add(new TranslatedText { Text = verseDocument.EnTrYusufAli, Translator = Translator.YusufAli });
            translatedTexts.Add(new TranslatedText { Text = verseDocument.EnTrPickthall, Translator = Translator.Pickthall });
            translatedTexts.Add(new TranslatedText { Text = verseDocument.EnTrShakir, Translator = Translator.Shakir });
            translatedTexts.Add(new TranslatedText { Text = verseDocument.EnTrHilali, Translator = Translator.Hilali });
            translatedTexts.Add(new TranslatedText { Text = verseDocument.EnTrDaryabadi, Translator = Translator.Daryabadi });
            translatedTexts.Add(new TranslatedText { Text = verseDocument.EnTrQaribullah, Translator = Translator.Qaribullah });
            translatedTexts.Add(new TranslatedText { Text = verseDocument.EnTrAyubKhan, Translator = Translator.AyubKhan });
            translatedTexts.Add(new TranslatedText { Text = verseDocument.EnTrSherAli, Translator = Translator.SherAli });
            translatedTexts.Add(new TranslatedText { Text = verseDocument.EnTrAsad, Translator = Translator.Asad });
            translatedTexts.Add(new TranslatedText { Text = verseDocument.EnTrArberry, Translator = Translator.Arberry });
            translatedTexts.Add(new TranslatedText { Text = verseDocument.EnTrMaududi, Translator = Translator.Maududi });
            verse.TranslatedTexts = translatedTexts;

            verse.VerseCommentaries.Add(CreateVerseCommentary(
                Translator.YusufAli,
                verseDocument.EnCtrYusufAli,
                verseDocument.EnNotesYusufAli));

            verse.VerseCommentaries.Add(CreateVerseCommentary(
                Translator.Asad,
                verseDocument.EnCtrAsad,
                verseDocument.EnNotesAsad));

            return verse;
        }

        private VerseCommentary CreateVerseCommentary(
            Translator commentator, 
            string commentaryText, 
            IEnumerable<VerseNote> documentNotes)
        {
            VerseCommentary verseCommentary = new VerseCommentary();
            verseCommentary.Text = commentaryText;
            verseCommentary.Commentator = commentator;

            if (documentNotes != null)
            {
                foreach (VerseNote documentNote in documentNotes)
                {
                    CommentaryNote commentaryNote = new CommentaryNote
                    {
                        Id = documentNote.Id,
                        Text = documentNote.Text
                    };

                    verseCommentary.Notes.Add(commentaryNote);
                }
            }

            return verseCommentary;
        }
    }
}
