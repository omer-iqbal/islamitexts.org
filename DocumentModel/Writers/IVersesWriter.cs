using System.Collections.Generic;

namespace IslamiTexts.DocumentModel.Writers
{
    public interface IVersesWriter
    {
        void Initialize(string path);

        void Write(VerseCollection verses);
    }
}
