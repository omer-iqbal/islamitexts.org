using System.Text.Json;

namespace IslamiTexts.DocumentModel.Converters
{
    public class VerseOld
    {
        public const string Name = @"";

        public class CommentaryNote
        {
            public string note_no;
            public string note;
        }

        public string type;
        public int surah_no;
        public int verse_no;
        public string ar_text;
        public string ar_text_clean;
        public string en_tr_yusuf;
        public string en_tr_pickth;
        public string en_tr_shakir;
        public string en_tr_hilali;
        public string en_tr_dbadi;
        public string en_tr_qarib;
        public string en_tr_ayubk;
        public string en_tr_sher;
        public string en_tr_asad;
        public string en_tr_arberry;
        public string en_tr_maududi;
        public string en_ctr_yusuf;
        public CommentaryNote[] en_notes_yusuf;
        public string en_ctr_asad;
        public CommentaryNote[] en_notes_asad;

        public static VerseOld DeserializeFromJson(string text)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.IncludeFields = true;
            return JsonSerializer.Deserialize<VerseOld>(text, options);
        }
    }
}
