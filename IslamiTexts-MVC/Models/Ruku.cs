namespace IslamiTexts.Models
{
    public class Ruku
    {
        public int RukuNo { get; set; }
        public int StartVerse { get; set; }
        public int EndVerse { get; set; }

        public Ruku(int rukuNo, int startVerse, int endVerse)
        {
            this.RukuNo = rukuNo;
            this.StartVerse = startVerse;
            this.EndVerse = endVerse;
        }
    }
}
