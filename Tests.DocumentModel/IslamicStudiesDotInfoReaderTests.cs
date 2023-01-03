using IslamiTexts.DocumentModel;
using IslamiTexts.DocumentModel.Converters;
using System.Collections.Generic;
using System.IO;

namespace Tests.DocumentModel
{
    [TestClass]
    public class IslamicStudiesDotInfoReaderTests
    {
        [TestMethod]
        public void Read_Just1Verse()
        {
            string noteText = @"One of the many practices taught by Islam is that its followers should begin their activities in the name of God. This principle, if consciously and earnestly followed, will necessarily yield three beneficial results. First, one will be able to restrain oneself from many misdeed, since the habit of pronouncing the name of God is bound to make one wonder when about to commit some offence how such an act can be reconciled with the saying of God's holy name. Second, if a man pronounces the name of God before starting good and legitimate tasks, this act will ensue that both his starting point and his mental orientation are sound. Third - and this is the most important benefit - when a man begins something by pronouncing God's name, he will enjoy God's support and succour; God will bless his efforts and protect him from the machinations and temptation of Satan. For whenever man turns to God, God turns to him as well.";
            string verseText = $@"بِسۡمِ اللهِ الرَّحۡمٰنِ الرَّحِيۡمِ 
(1:1) In the name of Allah, the Merciful, the Compassionate1

1. {noteText}";
            VerseCollection expectedVerses = new();
            Verse verse = expectedVerses[1, 1];
            Verse.Commentary commentary = new()
            {
                Translation = "In the name of Allah, the Merciful, the Compassionate[^1]",
                ReferenceId = IslamicStudiesDotInfoReader.ReferenceId,
                Notes = new List<Verse.CommentaryNote>()
                {
                    new Verse.CommentaryNote()
                    {
                        Id = "1",
                        Text = noteText
                    }
                }
            };

            verse.SurahNo = 1;
            verse.VerseNo = 1;
            verse.Commentaries.Add(commentary);
            Read_ValidVerses(verseText, expectedVerses);
        }

        public void Read_ValidVerses(string text, VerseCollection expectedVerses)
        {
            IslamicStudiesDotInfoReader reader = new();
            reader.Initialize(@"c:\");

            using (StringReader stringReader = new StringReader(text))
            {
                VerseCollection actualVerses = new();
                reader.Read(stringReader, 1, actualVerses);
                Assert.AreEqual(expectedVerses, actualVerses);
            }
        }

        [TestMethod]
        public void IsVerse_ValidTranslationWithNotes()
        {
            IslamicStudiesDotInfoReader reader = new();
            reader.Initialize(@"c:\");

            int expectedSurahNo = 24;
            int expectedVerseNo = 30;
            string expectedTranslation = "(O Prophet), enjoin believing men to cast down their looks29 and guard their private parts.30 That is purer for them. Surely Allah is well aware of all what they do.";
            string line = $"({expectedSurahNo}:{expectedVerseNo}) {expectedTranslation}";
            int actualSurahNo;
            int actualVerseNo;
            string actualTranslation;
            bool isVerse = reader.IsTranslation(line, out actualSurahNo, out actualVerseNo, out actualTranslation);
            Assert.IsTrue(isVerse, "isVerse should be true");
            Assert.AreEqual(expectedSurahNo, actualSurahNo, $"expectedSurahNo doesn't match actualSurahNo: {actualSurahNo}");
            Assert.AreEqual(expectedVerseNo, actualVerseNo, $"expectedVerseNo doesn't match actualVerseNo: {actualVerseNo}");
            Assert.AreEqual(expectedTranslation, actualTranslation, $"expectedTranslation doesn't match actualTranslation: {actualTranslation}");
        }

        [TestMethod]
        public void AddNoteNos_ValidTranslation_2Notes()
        {
            ValidateAddNoteNos("(O Prophet), looks29 private parts.30 That all what they do.",
                "(O Prophet), looks[^29] private parts.[^30] That all what they do.",
                new[] { "29", "30" });
        }

        [TestMethod]
        public void AddNoteNos_ValidTranslation_BOLNote()
        {
            ValidateAddNoteNos("22(O Prophet), looks private parts.30 That all what they do.",
                "[^22](O Prophet), looks private parts.[^30] That all what they do.",
                new[] {"22", "30" });
        }

        [TestMethod]
        public void AddNoteNos_ValidTranslation_EOLNote()
        {
            ValidateAddNoteNos("(O Prophet), looks private parts.30 That all what they do. 40",
                "(O Prophet), looks private parts.[^30] That all what they do. [^40]",
                new[] {"30", "40" });
        }

        [TestMethod]
        public void IsArabic_True()
        {
            IslamicStudiesDotInfoReader reader = new();
            reader.Initialize(@"c:\");
            bool isArabic = reader.IsArabic(@"لَـيۡسَ عَلَيۡكُمۡ جُنَاحٌ اَنۡ تَدۡخُلُوۡا بُيُوۡتًا غَيۡرَ مَسۡكُوۡنَةٍ فِيۡهَا مَتَاعٌ لَّـكُمۡ​ ؕ وَاللّٰهُ يَعۡلَمُ مَا تُبۡدُوۡنَ وَمَا تَكۡتُمُوۡنَ‏ ﻿﻿ ", 3, 1, 2);
            Assert.AreEqual(true, isArabic);
        }

        [TestMethod]
        public void IsArabic_False()
        {
            IslamicStudiesDotInfoReader reader = new();
            reader.Initialize(@"c:\");
            bool isArabic = reader.IsArabic(@"(24:31) And enjoin believing women to cast down their looks31 and guard their private32 parts33 " + 
                "and not reveal their adornment34 except that which is revealed of", 1, 2, 3);
            Assert.AreEqual(false, isArabic);
        }

        [TestMethod]
        public void IsNote_True()
        {
            ValidateIsNote("3.Hello world", true, 3, "Hello world");
        }

        public void ValidateAddNoteNos(string line, string expectedNewLine, string[] noteIds)
        {
            IslamicStudiesDotInfoReader reader = new();
            reader.Initialize(@"c:\");

            Queue<string> noteQueue = new();
            string actualNewLine = reader.AddNoteNumbers(line, noteQueue);
            foreach (string noteId in noteIds)
            {
                Assert.AreEqual(noteId, noteQueue.Dequeue());
            }

            Assert.AreEqual(0, noteQueue.Count, "noteQueue.Count");
            Assert.AreEqual(expectedNewLine, actualNewLine);
        }

        public void ValidateIsNote(string line, bool expectedIsNote, int expectedNoteNo, string expectedText)
        {
            IslamicStudiesDotInfoReader reader = new();
            reader.Initialize(@"c:\");
            string actualNoteNo;
            string actualText;
            bool actualIsNote = reader.IsNote(line, out actualNoteNo, out actualText);
            Assert.AreEqual(expectedIsNote, actualIsNote, "actualIsNote");
            Assert.AreEqual(expectedNoteNo, actualNoteNo, "actualNoteNo");
            Assert.AreEqual(expectedText, actualText, "actualText");
        }
    }
}
