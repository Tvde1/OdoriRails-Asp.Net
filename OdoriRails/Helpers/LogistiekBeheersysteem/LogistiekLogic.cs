using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OdoriRails.Helpers.LogistiekBeheersysteem
{
    public class LogistiekLogic
    {
        public List<BeheerTrack> AllTracks { get; private set; }
        public List<BeheerTram> AllTrams { get; private set; }

        private readonly LogisticRepository _repo = new LogisticRepository();

        public void Update()
        {
            AllTracks = new List<BeheerTrack>();
            foreach (Track track in _repo.GetTracksAndSectors())
            {
                AllTracks.Add(track == null ? null : BeheerTrack.ToBeheerTrack(track));
            }
            AllTrams = new List<BeheerTram>();
            foreach (Tram tram in _repo.GetAllTrams())
            {
                AllTrams.Add(tram == null ? null : BeheerTram.ToBeheerTram(tram));
            }
        }

        public bool AddSector(int trackNumber)
        {
            foreach (Track track in AllTracks.Where(x => x.Number == trackNumber))
            {
                track.AddSector(new Sector(track.Sectors.Count + 1));
                _repo.AddSector(track.Sectors[track.Sectors.Count - 1], track);
                Update();
                return true;
            }
            return false;
        }

        public bool DeleteSector(int trackNumber)
        {
            foreach (Track track in AllTracks.Where(x => x.Number == trackNumber && x.Sectors[x.Sectors.Count - 1].OccupyingTram == null))
            {
                track.DeleteSector();
                _repo.DeleteSectorFromTrack(track, track.Sectors[track.Sectors.Count - 1]);
                Update();
                return true;
            }
            return false;
        }

        public bool DeleteTram(int tramNumber)
        {
            foreach (Tram tram in AllTrams.Where(x => x.Number == tramNumber))
            {
                _repo.RemoveTram(tram);
                Update();
                return true;
            }
            return false;
        }

        public bool DeleteTrack(int trackNumber)
        {
            foreach (Track track in AllTracks.Where(x => x.Number == trackNumber))
            {
                if (track.Sectors.Where(x => x.OccupyingTram != null).Count() > 0)
                {
                    return false;
                }
                _repo.DeleteTrack(track);
                Update();
                return true;
            }
            return false;
        }
    
        public bool Lock(string tracks, string sectors)
        {
            int[] lockSectors = { -1 };
            int[] lockTracks;

            try
            {
                if (sectors != "")
                {
                    lockSectors = Parse(sectors);
                    for (var i = 0; i < lockSectors.Length; i++)
                    {
                        lockSectors[i] -= 1;
                    }
                }

                lockTracks = Parse(tracks);
            }
            catch
            {
                return false;
            }
            if (lockTracks[0] == -1) return true;

            if (sectors == "")
            {
                foreach (var track in AllTracks)
                {
                    if (Array.IndexOf(lockTracks, track.Number) <= -1) continue;
                    var beheerTrack = BeheerTrack.ToBeheerTrack(track);
                    beheerTrack.LockTrack();
                    _repo.EditTrack(beheerTrack);
                }
            }
            else
            {
                foreach (var track in AllTracks)
                {
                    if (Array.IndexOf(lockTracks, track.Number) <= -1) continue;
                    for (var i = 0; i < track.Sectors.Count - 1; i++)
                    {
                        if (Array.IndexOf(lockSectors, i) <= -1) continue;
                        var beheerSector = track.Sectors[i] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[i]);
                        if (beheerSector == null) continue;
                        beheerSector.Lock();
                        _repo.EditSector(beheerSector);
                    }
                }
            }
            Update();
            return true;
        }

        public bool Unlock(string tracks, string sectors)
        {
            int[] unlockSectors = { -1 };
            int[] unlockTracks;

            try
            {
                if (sectors != "")
                {
                    unlockSectors = Parse(sectors);
                    for (var i = 0; i < unlockSectors.Length; i++)
                    {
                        unlockSectors[i] -= 1;
                    }
                }
                unlockTracks = Parse(tracks);
            }
            catch
            {
                return false;
            }

            if (unlockTracks[0] == -1) return true;
            if (sectors == "")
            {
                foreach (var track in AllTracks)
                {
                    if (Array.IndexOf(unlockTracks, track.Number) <= -1) continue;
                    BeheerTrack beheerTrack = BeheerTrack.ToBeheerTrack(track);
                    beheerTrack.UnlockTrack();
                    _repo.EditTrack(beheerTrack);
                }
            }
            else
            {
                foreach (BeheerTrack track in AllTracks)
                {
                    if (Array.IndexOf(unlockTracks, track.Number) <= -1) continue;
                    for (var i = 0; i < track.Sectors.Count - 1; i++)
                    {
                        if (Array.IndexOf(unlockSectors, i) <= -1) continue;
                        var beheerSector =
                            track.Sectors[i] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[i]);
                        if (beheerSector == null) continue;
                        beheerSector.UnLock();
                        _repo.EditSector(beheerSector);
                    }
                }
            }
            Update();
            return true;
        }

        public void ToggleDisabled(string trams)
        {
            var iTrams = Parse(trams);
            foreach (var tram in AllTrams)
            {
                var pos = Array.IndexOf(iTrams, tram.Number);
                if (pos <= -1) continue;
                if (tram.Status == TramStatus.Defect)
                {
                    tram.EditTramStatus(TramStatus.Idle);
                    _repo.EditTram(tram);
                }
                else
                {
                    tram.EditTramStatus(TramStatus.Defect);
                    _repo.EditTram(tram);
                }
            }
            Update();
        }

        public bool MoveTram(int moveTram, int moveTrack, int moveSector)
        {
            moveSector -= 1;

            foreach (var track in AllTracks.Where(x => x.Number == moveTrack && x.Sectors.Count > moveSector))
            {
                foreach (var tram in AllTrams.Where(x => x.Number == moveTram))
                {
                    var beheerSector = track.Sectors[moveSector] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[moveSector]);
                    if (beheerSector == null || !beheerSector.SetOccupyingTram(tram)) continue;
                    _repo.WipeSectorByTramId(tram.Number);
                    _repo.EditSector(beheerSector);
                    Update();
                    return true;
                }
            }
            return false;
        }

        public void AddTram(int tramNumber, int? defaultLine, string model)
        {
            if (tramNumber == -1 || defaultLine == -1 || defaultLine == null) return;
            TramModel newModel;
            Enum.TryParse(model, out newModel);

            _repo.AddTram(new Tram(tramNumber, ToInt(defaultLine), newModel));
            Update();
        }

        public void AddTrack(int trackNumber, int sectorAmount, string trackType, int? defaultLine)
        {
            int? newDefaultLine = defaultLine ?? 0;

            if (trackNumber == -1 || sectorAmount == -1) return;
            TrackType newTrackType;
            Enum.TryParse(trackType, out newTrackType);

            var newSectors = new List<Sector>();
            for (int i = 0; i < sectorAmount; i++)
            {
                newSectors.Add(new Sector(i + 1));
            }

            _repo.AddTrack(new Track(trackNumber, newDefaultLine, newTrackType, newSectors));
            Update();
        }

        private static int[] Parse(string _string)
        {
            int[] array = { };
            try
            {
                array = Array.ConvertAll(_string.Split(','), int.Parse);
            }
            catch
            {
                //TODO: Hier een fatsoenlijke trycatch van maken dat een waarschuwing in ASP.NET geeft.
            }

            return array;
        }

        private static int ToInt(int? _int)
        {
            var integer = -1;

            try
            {
                integer = Convert.ToInt32(_int);
            }
            catch
            {
                //TODO: Hier een fatsoenlijke trycatch van maken dat een waarschuwing in ASP.NET geeft.
            }

            return integer;
        }
    }
}