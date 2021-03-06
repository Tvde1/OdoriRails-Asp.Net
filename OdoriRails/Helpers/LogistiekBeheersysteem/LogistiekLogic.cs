﻿using System;
using System.Collections.Generic;
using System.Linq;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.LogistiekBeheersysteem.ObjectClasses;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Helpers.LogistiekBeheersysteem
{
    public class LogistiekLogic
    {
        private readonly LogisticRepository _repo = new LogisticRepository();

        public LogistiekLogic()
        {
            Update();
        }

        public Dictionary<int, BeheerTrack> AllTracks { get; private set; }
        public Dictionary<int, BeheerTram> AllTrams { get; private set; }

        private void Update()
        {
            AllTracks = new Dictionary<int, BeheerTrack>();
            foreach (var track in _repo.GetTracksAndSectors())
            {
                if (track == null) continue;
                AllTracks.Add(track.Number, BeheerTrack.ToBeheerTrack(track));
            }
            AllTrams = new Dictionary<int, BeheerTram>();
            foreach (var tram in _repo.GetAllTrams())
            {
                if (tram == null) continue;
                AllTrams.Add(tram.Number, BeheerTram.ToBeheerTram(tram));
            }
        }

        public string AddSector(int trackNumber, decimal latitude, decimal longitude)
        {
            if (!AllTracks.ContainsKey(trackNumber)) return "Dit spoor bestaat niet.";
            var track = AllTracks[trackNumber];
            track.AddSector(new Sector(track.Sectors.Count + 1, track.Number, SectorStatus.Open, null, latitude,
                longitude));
            _repo.AddSector(track.Sectors[track.Sectors.Count - 1], track);
            Update();
            return null;
        }

        public string DeleteSector(int trackNumber)
        {
            if (!AllTracks.ContainsKey(trackNumber)) return "Dit spoor bestaat niet.";
            var track = AllTracks[trackNumber];
            var sector = track.Sectors.Last();

            if (sector == null) return "Dit spoor heeft geen sectoren.";
            if (sector.OccupyingTram != null) return "Er staat een tram op deze sector. Haal deze eerst weg.";

            track.DeleteSector();
            _repo.DeleteSectorFromTrack(track, sector);
            Update();
            return null;
        }

        public string DeleteTram(int tramNumber)
        {
            if (!AllTrams.ContainsKey(tramNumber)) return "Deze tram bestaat niet.";
            var tram = AllTrams[tramNumber];

            _repo.RemoveTram(tram);
            Update();
            return null;
        }

        public string DeleteTrack(int trackNumber)
        {
            if (!AllTracks.ContainsKey(trackNumber)) return "Dit spoor bestaat niet.";
            var track = AllTracks[trackNumber];

            if (track.Sectors.Any(x => x.OccupyingTram != null))
                return "Er staat een tram op dit spoor. haal het eerst weg.";

            _repo.DeleteTrack(track);
            Update();
            return null;
        }

        public string Lock(string tracks, string sectors)
        {
            int[] lockSectors = {-1};
            int[] lockTracks;

            try
            {
                if (!string.IsNullOrEmpty(sectors))
                {
                    lockSectors = Parse(sectors);
                    for (var i = 0; i < lockSectors.Length; i++)
                        lockSectors[i] -= 1;
                }

                lockTracks = Parse(tracks);
            }
            catch
            {
                return "De input klopt niet.";
            }
            if (lockTracks[0] == -1) return null;

            var newLockTracks = AllTracks.Where(x => lockTracks.Contains(x.Key));

            foreach (var track in newLockTracks)
            {
                var beheerTrack = BeheerTrack.ToBeheerTrack(track.Value);
                beheerTrack.LockTrack();
                _repo.EditTrack(beheerTrack);
            }


            foreach (var track in AllTracks)
            {
                if (Array.IndexOf(lockTracks, track.Key) <= -1) continue;
                for (var i = 0; i < track.Value.Sectors.Count - 1; i++)
                {
                    if (Array.IndexOf(lockSectors, i) <= -1) continue;
                    var beheerSector = track.Value.Sectors[i] == null
                        ? null
                        : BeheerSector.ToBeheerSector(track.Value.Sectors[i]);
                    if (beheerSector == null) continue;
                    beheerSector.Lock();
                    _repo.EditSector(beheerSector);
                }
            }

            Update();
            return null;
        }

        public string Unlock(string tracks, string sectors)
        {
            int[] unlockSectors = {-1};
            int[] unlockTracks;

            try
            {
                if (!string.IsNullOrEmpty(sectors))
                {
                    unlockSectors = Parse(sectors);
                    for (var i = 0; i < unlockSectors.Length; i++)
                        unlockSectors[i] -= 1;
                }
                unlockTracks = Parse(tracks);
            }
            catch
            {
                return "De input is incorrect.";
            }

            foreach (var track in AllTracks)
            {
                if (Array.IndexOf(unlockTracks, track.Key) <= -1) continue;
                var beheerTrack = BeheerTrack.ToBeheerTrack(track.Value);
                beheerTrack.UnlockTrack();
                _repo.EditTrack(beheerTrack);
            }

            foreach (var track in AllTracks)
            {
                if (Array.IndexOf(unlockTracks, track.Key) <= -1) continue;
                for (var i = 0; i < track.Value.Sectors.Count - 1; i++)
                {
                    if (Array.IndexOf(unlockSectors, i) <= -1) continue;
                    var beheerSector =
                        track.Value.Sectors[i] == null ? null : BeheerSector.ToBeheerSector(track.Value.Sectors[i]);
                    if (beheerSector == null) continue;
                    beheerSector.UnLock();
                    _repo.EditSector(beheerSector);
                }
            }
            Update();
            return null;
        }

        public void ToggleDisabled(string trams)
        {
            var iTrams = Parse(trams);
            foreach (var tram in AllTrams)
            {
                var pos = Array.IndexOf(iTrams, tram.Key);
                if (pos != -1)
                    if (tram.Value.Status == TramStatus.Defect)
                    {
                        tram.Value.EditTramStatus(TramStatus.Idle);
                        _repo.EditTram(tram.Value);
                    }
                    else
                    {
                        tram.Value.EditTramStatus(TramStatus.Defect);
                        _repo.EditTram(tram.Value);
                    }
            }
            Update();
        }

        public string MoveTram(int moveTram, int moveTrack, int moveSector, bool hasAccepted = false)
        {
            moveSector -= 1;

            if (!AllTrams.ContainsKey(moveTram)) return "Deze tram bestaat niet.";
            var tram = AllTrams[moveTram];
            if (!hasAccepted && tram.Status != TramStatus.Idle) return "NOT IDLE";
            if (!AllTracks.ContainsKey(moveTrack)) return "Dit spoor betaat niet.";
            var track = AllTracks[moveTrack];
            if (track.Sectors.Count < moveSector + 1) return $"Spoor {track.Number} heeft zo veel secoren niet";
            var sector = track.Sectors[moveSector];

            switch (sector.Status)
            {
                case SectorStatus.Locked:
                    return "Deze sector is afgesloten.";
                case SectorStatus.Occupied:
                    return "Deze sector is bezet.";
                case SectorStatus.Open:
                {
                    var beheerSector = BeheerSector.ToBeheerSector(sector);
                    if (beheerSector != null && beheerSector.SetOccupyingTram(tram))
                        _repo.WipeSectorByTramId(tram.Number);
                    _repo.EditSector(beheerSector);
                    Update();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
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
            for (var i = 0; i < sectorAmount; i++)
                newSectors.Add(new Sector(i + 1));

            _repo.AddTrack(new Track(trackNumber, newDefaultLine, newTrackType, newSectors));
            Update();
        }

        private static int[] Parse(string _string)
        {
            return Array.ConvertAll(_string.Split(','), int.Parse);
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

        public void StartSimulation()
        {
            _repo.StartSimulation();
        }
    }
}