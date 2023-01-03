using IslamiTexts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IslamiTexts.Controllers
{
    public class MarkdownController : Controller
    {
        // GET: TestMarkdown
        public ActionResult Index()
        {
            return View("CheckMarkdown");
        }

        // GET: Markdown/Check
        public ActionResult Check()
        {
            return View("CheckMarkdown");
        }

        //// POST: TestMarkdown/Check
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Check([FromForm] Markdown md)
        {
            return View("CheckMarkdown", md);
        }   
    }
}
