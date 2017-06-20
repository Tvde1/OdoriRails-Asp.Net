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

        private readonly List<Sector> _sectors;
        private readonly List<Track> _tracks;
        private readonly ITrackSectorContext _trackSectorContext = new TrackSectorContext();
        private readonly ITramContext _tramContext = new TramContext();
        private readonly Dictionary<int, Tram> _trams;

        public ApiRepository()
        {
            _sectors = ObjectCreator.GenerateListWithFunction(_trackSectorContext.GetAllSectors(),
                _objectCreator.CreateSector);
            _tracks = ObjectCreator.GenerateListWithFunction(_trackSectorContext.GetAllTracks(),
                ObjectCreator.CreateTrack);
            _trams = ObjectCreator.GenerateListWithFunction(_tramContext.GetAllTrams(),
                _objectCreator.CreateTram).ToDictionary(x => x.Number, x => x);
        }

        public List<Track> GetAllTracks()
        {
            foreach (var track in _tracks)
            foreach (var sector in _sectors.Where(x => x.TrackNumber == track.Number))
            {
                if (sector.TramId != null)
                    sector.SetTram(_trams[sector.TramId.Value]);
                track.AddSector(sector);
            }
            return _tracks;
        }
    }
}