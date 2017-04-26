using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IslamiTexts.Models;

namespace IslamiTexts.ViewModels
{
    public class VersePageViewModel
    {
        public Verse Verse { get; set; }
        public VerseBlock VerseBlock { get; set; }
    }
}
