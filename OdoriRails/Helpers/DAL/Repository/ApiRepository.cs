using System.Collections.Generic;
using System.Linq;
using OdoriRails.Helpers.DAL.ContextInterfaces;
using OdoriRails.Helpers.DAL.Contexts;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.DAL.Repository
{
    public class ApiRepository
    {
        private readonly ObjectCreator _objectCreator = new ObjectCreator();
        private readonly ITramContext _tramContext = new TramContext();
        private readonly ITrackSectorContext _trackSectorContext = new TrackSectorContext();

        private readonly List<Sector> _sectors;
        private readonly Dictionary<int, Track> _tracks;

        public ApiRepository()
        {
            _sectors = ObjectCreator.GenerateListWithFunction(_trackSectorContext.GetAllSectors(),
                _objectCreator.CreateSector);
            _tracks = ObjectCreator.GenerateListWithFunction(_trackSectorContext.GetAllTracks(),
                ObjectCreator.CreateTrack).ToDictionary(x => x.Number, x => x);

            foreach (var sector in _sectors)
            {
                _tracks[sector.TrackNumber].AddSector(sector);
            }
        }

        public IEnumerable<Tram> GetAllTrams()
        {
            return ObjectCreator.GenerateListWithFunction(_tramContext.GetAllTrams(), _objectCreator.CreateTram);
        }

        public IEnumerable<Sector> GetSectorsFromTram(Tram tram)
        {
            return _sectors.Where(x => x.TramId == tram.Number);
        }

        public KeyValuePair<Track, Sector> GetTrackFromTram(Tram tram)
        {
            var track = _tracks.FirstOrDefault(x => x.Value.Sectors.Exists(y => y.TramId == tram.Number));
            if (track.Value == null) return new KeyValuePair<Track, Sector>(null, null);
            var sectors = _sectors.FirstOrDefault(x => x.TrackNumber == track.Key);
            return new KeyValuePair<Track, Sector>(track.Value, sectors);
        }
    }
}