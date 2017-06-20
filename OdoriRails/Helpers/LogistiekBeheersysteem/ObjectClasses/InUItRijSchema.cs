using System;

namespace OdoriRails.Helpers.LogistiekBeheersysteem
{
    public class InUitRijSchema
    {
        public InUitRijSchema(string _UitRijTijd, string _InRijTijd, int _line)
        {
            ExitTime = Convert.ToDateTime(_UitRijTijd);
            EntryTime = Convert.ToDateTime(_InRijTijd);
            Line = _line;
        }

        public DateTime ExitTime { get; }
        public DateTime EntryTime { get; }
        public int Line { get; }
        public int dw { get; private set; }
        public int? TramNumber { get; set; }
        public int TrackNumber { get; private set; }
        public string bijzonderheden { get; private set; }

        public override string ToString()
        {
            return ExitTime + " " + EntryTime + " " + Line + " " + TramNumber;
        }
    }
}