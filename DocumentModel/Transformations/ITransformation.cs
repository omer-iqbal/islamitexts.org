using System.Collections.Generic;

namespace IslamiTexts.DocumentModel.Transformations
{
    public interface ITransformation
    {
        void Apply(VerseCollection verses, string translationReferenceId);
    }
}
