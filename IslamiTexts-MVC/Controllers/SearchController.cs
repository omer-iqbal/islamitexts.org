using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IslamiTexts.Data;
using IslamiTexts.Models;

namespace IslamiTexts.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index(string q, int t = 9, int start = 0)
        {
            if (String.IsNullOrWhiteSpace(q))
                return RedirectToAction("Index", "Home");

            if (q.Contains(":"))
            {
                string[] parts = q.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    int snoFromQ;
                    int verseNofromQ;

                    if (Int32.TryParse(parts[0], out snoFromQ) && Int32.TryParse(parts[1], out verseNofromQ))
                        return RedirectToAction("Verse", "Quran", new { surahNo = snoFromQ, verseNo = verseNofromQ });
                }
            }

            this.ViewData[Constants.QueryKey] = q;

            ISearcher searchService = new LuceneSearch();
            SearchResults searchResults = searchService.GetSearchResults(
                SearchScope.Verse, 
                q, 
                (Translator)t,
                start,
                10);    // no of results to fetch
            return View(searchResults);
        }
    }
}
