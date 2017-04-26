using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IslamiTexts.Models
{
    public class TranslatedText
    {
        public string Text { get; set; }

        public Translator Translator { get; set; }
    }
}