using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IslamiTexts.Data;
using IslamiTexts.Models;
using IslamiTexts.ViewModels;

namespace IslamiTexts.Controllers
{
    public class QuranController : Controller
    {
        const int NoOfVersesInContext = 7;

        [ActionName("Verse")]
        public async Task<ActionResult> GetVerseAsync(int surahNo, int verseNo)
        {
            if (surahNo == 0 && verseNo == 0)
                return new HttpNotFoundResult();

            this.ViewData[Constants.QueryKey] = String.Format(@"{0}:{1}", surahNo, verseNo);

            DocumentRepository repository = new DocumentRepository();
            Verse verse = await repository.GetVerseAsync(surahNo, verseNo);

            Surah surah = repository.GetSurah(surahNo);
            Ruku ruku = surah.GetRukuForVerse(verseNo);

            int blockStartVerseNo = ruku.StartVerse;
            int blockEndVerseNo = ruku.EndVerse;

            VerseBlock verseBlock = await repository.GetOrderedVersesAsync(
                surahNo, blockStartVerseNo, blockEndVerseNo, Translator.Asad);

            VersePageViewModel viewModel = new VersePageViewModel
            {
                Surah = surah,
                VerseBlock = verseBlock,
                Verse = verse
            };

            return View("Verse", viewModel);
        }
    }
}
