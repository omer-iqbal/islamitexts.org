using System.Collections.Generic;

namespace IslamiTexts.DocumentModel.Converters
{
    public interface IVersesReader
    {
        void Initialize(string path);

        void Read(VerseCollection verses);
    }
}
