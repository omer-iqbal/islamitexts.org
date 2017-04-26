using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IslamiTexts.Models;
using IslamiTexts.Data.Models;

namespace IslamiTexts.Data
{
    public class DocumentRepository
    {
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
            return Surahs;
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

        private readonly static IList<Surah> Surahs = new List<Surah>
        {
            new Surah(1, new[] { "الفاتحة", "أم الكتاب", "أم القرآن", "السبع المثاني", "الحمد", "الشفاء", "سورة الصلاة", "الأساس" }, 
                new[] { "Al-Fātihah" }, 7),
            new Surah(2, new[] { "البقرة" }, new[] { "Al-Baqarah" }, 286),
            new Surah(3, new[] { "آل عمران" }, new[] { "Āl-i-Imrān" }, 200),
            new Surah(4, new[] { "النساء" }, new[] { "Al-Nisā" }, 176),
            new Surah(5, new[] { "المائدة" }, new[] { "Al-Māidah" }, 120),
            new Surah(6, new[] { "الأنعام" }, new[] { "Al-An'ām" }, 165),
            new Surah(7, new[] { "الأعراف" }, new[] { "Al-A'rāf" }, 206),
            new Surah(8, new[] { "الأنفال" }, new[] { "Al-Anfāl" }, 75),
            new Surah(9, new[] { "التوبة", "البراءة" }, new[] { "Al-Taubah" }, 129),
            new Surah(10, new[] { "يونس" }, new[] { "Yūnus" }, 109),
            new Surah(11, new[] { "هود" }, new[] { "Hūd" }, 123),
            new Surah(12, new[] { "يوسف" }, new[] { "Yūsuf" }, 111),
            new Surah(13, new[] { "الرعد" }, new[] { "Al-Ra'd" }, 43),
            new Surah(14, new[] { "إبراهيم" }, new[] { "Ibrāhīm" }, 52),
            new Surah(15, new[] { "الحجر" }, new[] { "Al-Hijr" }, 99),
            new Surah(16, new[] { "النحل", "النعم" }, new[] { "Al-Nahl" }, 128),
            new Surah(17, new[] { "الإسراء", "بني إسراءيل", "سبحان" }, new[] { "Al-Isrā" }, 111),
            new Surah(18, new[] { "الكهف" }, new[] { "Al-Kahf" }, 110),
            new Surah(19, new[] { "مريم" }, new[] { "Maryam" }, 98),
            new Surah(20, new[] { "طه" }, new[] { "Ta Ha" }, 135),
            new Surah(21, new[] { "الأنبياء" }, new[] { "Al-Anbiyā" }, 112),
            new Surah(22, new[] { "الحج" }, new[] { "Al-Hajj" }, 78),
            new Surah(23, new[] { "المؤمنون" }, new[] { "Al-Muminūn" }, 118),
            new Surah(24, new[] { "النور" }, new[] { "Al-Nūr" }, 64),
            new Surah(25, new[] { "الفرقان" }, new[] { "Al-Furqān" }, 77),
            new Surah(26, new[] { "الشعراء" }, new[] { "Al-Shu'arā" }, 227),
            new Surah(27, new[] { "النمل" }, new[] { "Al-Naml" }, 93),
            new Surah(28, new[] { "القصص" }, new[] { "Al-Qasas" }, 88),
            new Surah(29, new[] { "العنكبوت" }, new[] { "Al-'Ankabūt" }, 69),
            new Surah(30, new[] { "الروم" }, new[] { "Al-Rūm" }, 60),
            new Surah(31, new[] { "لقمان" }, new[] { "Luqmān" }, 34),
            new Surah(32, new[] { "السجدة", "المضاجع", "الم تنزيل" }, new[] { "Al-Sajadah" }, 30),
            new Surah(33, new[] { "الأحزاب" }, new[] { "Al-Ahzāb" }, 73),
            new Surah(34, new[] { "سبأ" }, new[] { "Sabā" }, 54),
            new Surah(35, new[] { "فاطر", "الملائكة" }, new[] { "Fātir" }, 45),
            new Surah(36, new[] { "يس" }, new[] { "Yā Sīn" }, 83),
            new Surah(37, new[] { "الصافات" }, new[] { "Al-Sāffāt" }, 182),
            new Surah(38, new[] { "ص", "داود" }, new[] { "Sād" }, 88),
            new Surah(39, new[] { "الزمر", "الغرف" }, new[] { "Al-Zumar" }, 75),
            new Surah(40, new[] { "غافر", "المؤمن", "الفضل" }, new[] { "Ghāfir" }, 85),
            new Surah(41, new[] { "فصلت", "حم سجدة", "المصابيح", "الأقوات" }, new[] { "Fussilat" }, 54),
            new Surah(42, new[] { "الشورى", "حم عسق" }, new[] { "Al-Shūra" }, 53),
            new Surah(43, new[] { "الزخرف" }, new[] { "Al-Zukhruf" }, 89),
            new Surah(44, new[] { "الدخان" }, new[] { "Al-Dukhān" }, 59),
            new Surah(45, new[] { "الجاثية", "الشريعة" }, new[] { "Al-Jāthiah" }, 37),
            new Surah(46, new[] { "الأحقاف" }, new[] { "Al-Ahqāf" }, 35),
            new Surah(47, new[] { "محمد", "القتال" }, new[] { "Muhammad" }, 38),
            new Surah(48, new[] { "الفتح" }, new[] { "Al-Fatah" }, 29),
            new Surah(49, new[] { "الحجرات" }, new[] { "Al-Hujurāt" }, 18),
            new Surah(50, new[] { "ق", "الباسقات" }, new[] { "Qāf" }, 45),
            new Surah(51, new[] { "الذاريات" }, new[] { "Al-Zāriyāt" }, 60),
            new Surah(52, new[] { "الطور" }, new[] { "Al-Tūr" }, 49),
            new Surah(53, new[] { "النجم" }, new[] { "Al-Najm" }, 62),
            new Surah(54, new[] { "القمر", "اقتربت الساعة" }, new[] { "Al-Qamar" }, 55),
            new Surah(55, new[] { "الرحمن" }, new[] { "Al-Rahmān" }, 78),
            new Surah(56, new[] { "الواقعة" }, new[] { "Al-Wāqi'ah" }, 96),
            new Surah(57, new[] { "الحديد" }, new[] { "Al-Hadīd" }, 29),
            new Surah(58, new[] { "المجادلة", "الظهار" }, new[] { "Al-Mujādilah" }, 22),
            new Surah(59, new[] { "الحشر", "بنو نضير" }, new[] { "Al-Hashr" }, 24),
            new Surah(60, new[] { "الممتحنة", "الامتحان", "المودة" }, new[] { "Al-Mumtahinah" }, 13),
            new Surah(61, new[] { "الصف", "الحواريون", "عيسى" }, new[] { "Al-Saff" }, 14),
            new Surah(62, new[] { "الجمعة" }, new[] { "Al-Jumu'ah" }, 11),
            new Surah(63, new[] { "المنافقون" }, new[] { "Al-Munāfiqūn" }, 11),
            new Surah(64, new[] { "التغابن" }, new[] { "Al-Taghābun" }, 18),
            new Surah(65, new[] { "الطلاق", "سورة النساء القصرى" }, new[] { "Al-Talāq" }, 12),
            new Surah(66, new[] { "التحريم", "لما تحرم", "المتحرم", "سورة النبي" }, new[] { "Al-Tahrīm" }, 12),
            new Surah(67, new[] { "الملك", "تبارك", "المانع", "المنجية" }, new[] { "Al-Mulk" }, 30),
            new Surah(68, new[] { "القلم" }, new[] { "Al-Qalam" }, 52),
            new Surah(69, new[] { "الحاقة" }, new[] { "Al-Hāqah" }, 52),
            new Surah(70, new[] { "المعارج", "المواقع", "سأل" }, new[] { "Al-Ma'ārij" }, 44),
            new Surah(71, new[] { "نوح" }, new[] { "Nūh" }, 28),
            new Surah(72, new[] { "الجن" }, new[] { "Al-Jinn" }, 28),
            new Surah(73, new[] { "المزمل" }, new[] { "Al-Muzammil" }, 20),
            new Surah(74, new[] { "المدثر" }, new[] { "Al-Mudathir" }, 56),
            new Surah(75, new[] { "القيامة", "لا أقسم" }, new[] { "Al-Qiyāmah" }, 40),
            new Surah(76, new[] { "الإنسان", "الدهر", "هل أتى", "الأبرار" }, new[] { "Al-Insān" }, 31),
            new Surah(77, new[] { "المرسلات" }, new[] { "Al-Mursilāt" }, 50),
            new Surah(78, new[] { "النبأ", "المعصرات", "التساءل" }, new[] { "Al-Nabā" }, 40),
            new Surah(79, new[] { "النازعات", "الساهرة", "الطامة" }, new[] { "Al-Nāzi'āt" }, 46),
            new Surah(80, new[] { "عبس", "الصاخة", "السفرة" }, new[] { "'Abasa" }, 42),
            new Surah(81, new[] { "التكوير" }, new[] { "Al-Takvīr" }, 29),
            new Surah(82, new[] { "الإنفطار" }, new[] { "Al-Infitār" }, 19),
            new Surah(83, new[] { "المطففين" }, new[] { "Al-Mutaffiffīn" }, 36),
            new Surah(84, new[] { "الإنشقاق" }, new[] { "Al-Inshiqāq" }, 25),
            new Surah(85, new[] { "البروج" }, new[] { "Al-Burūj" }, 22),
            new Surah(86, new[] { "الطارق" }, new[] { "Al-Tāriq" }, 17),
            new Surah(87, new[] { "الأعلى" }, new[] { "Al-A'lā" }, 19),
            new Surah(88, new[] { "الغاشية" }, new[] { "Al-Ghāshiyah" }, 26),
            new Surah(89, new[] { "الفجر" }, new[] { "Al-Fajr" }, 30),
            new Surah(90, new[] { "البلد" }, new[] { "Al-Balad" }, 20),
            new Surah(91, new[] { "الشمس" }, new[] { "Al-Shams" }, 15),
            new Surah(92, new[] { "الليل" }, new[] { "Al-Layl" }, 21),
            new Surah(93, new[] { "الضحى" }, new[] { "Al-Duhā" }, 11),
            new Surah(94, new[] { "الشرح", "الانشراح" }, new[] { "Al-Sharah" }, 8),
            new Surah(95, new[] { "التين" }, new[] { "Al-Tīn" }, 8),
            new Surah(96, new[] { "العلق", "اقرأ" }, new[] { "Al-'Alaq" }, 19),
            new Surah(97, new[] { "القدر" }, new[] { "Al-Qadr" }, 5),
            new Surah(98, new[] { "البينة", "لم يكن" }, new[] { "Al-Bayyinah" }, 8),
            new Surah(99, new[] { "الزلزلة" }, new[] { "Al-Zulzilah" }, 8),
            new Surah(100, new[] { "العاديات" }, new[] { "Al-'ādiyāt" }, 11),
            new Surah(101, new[] { "القارعة" }, new[] { "Al-Qāri'ah" }, 11),
            new Surah(102, new[] { "التكاثر" }, new[] { "Al-Takāthur" }, 8),
            new Surah(103, new[] { "العصر" }, new[] { "Al-'Asr" }, 3),
            new Surah(104, new[] { "الهمزة" }, new[] { "Al-Humazah" }, 9),
            new Surah(105, new[] { "الفيل" }, new[] { "Al-Fīl" }, 5),
            new Surah(106, new[] { "قريش" }, new[] { "Quraysh" }, 4),
            new Surah(107, new[] { "الماعون" }, new[] { "Al-Mā'ūn" }, 7),
            new Surah(108, new[] { "الكوثر" }, new[] { "Al-Kauthar" }, 3),
            new Surah(109, new[] { "الكافرون" }, new[] { "Al-Kāfirūn" }, 6),
            new Surah(110, new[] { "النصر" }, new[] { "Al-Nasr" }, 3),
            new Surah(111, new[] { "المسد", "اللهب" }, new[] { "Al-Masad" }, 5),
            new Surah(112, new[] { "الإخلاص", "المنفرة", "النجاة", "المعرفة", "المذكرة", "نور القرآن" }, 
                new[] { "Al-Ikhlās" }, 4),
            new Surah(113, new[] { "الفلق" }, new[] { "Al-Falaq" }, 5),
            new Surah(114, new[] { "الناس" }, new[] { "Al-Nās" }, 6)
        };
    }
}
