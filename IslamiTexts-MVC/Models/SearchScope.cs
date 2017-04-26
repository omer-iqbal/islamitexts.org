using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IslamiTexts.Models
{
    [Flags]
    public enum SearchScope
    {
        Verse = 1,

        VerseCommentary = 2
    }
}
