namespace OdoriRails.Models.LogistiekBeheer
{
    public class FormResultModel
    {
        //Tram
        public int TramNumber { get; set; }
        public string TramNumbers { get; set; }
        public string TramModel { get; set; }
        //Track
        public int TrackNumber { get; set; }
        public string TrackNumbers { get; set; }
        public string TrackType { get; set; }
        //Sector
        public int SectorNumber { get; set; }
        public string SectorNumbers { get; set; }
        public int SectorAmount { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        //Other
        public int? DefaultLine { get; set; }
        public int RadioButton { get; set; }
    }
}