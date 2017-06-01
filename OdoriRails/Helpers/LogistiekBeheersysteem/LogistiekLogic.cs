using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.LogistiekBeheersysteem.CSV;
using OdoriRails.Helpers.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace OdoriRails.Helpers.LogistiekBeheersysteem
{
    public class LogistiekLogic
    {
        public List<BeheerTrack> BackupAllTracks;
        public List<BeheerTrack> AllTracks { get; private set; }
        public List<BeheerTram> AllTrams { get; private set; }
        bool testing = true;
        int simulationSpeed = 600;

        I_CSVContext csv;
        private LogisticRepository repo = new LogisticRepository();
        private List<InUitRijSchema> schema;

        /// <summary>
        /// Constructor: Voert alles uit dat bij de launch uitgevoerd moet worden.
        /// </summary>
        public LogistiekLogic()
        {
            if (testing == true)
            {
                simulationSpeed = 50;
            }

            FetchUpdates();
            csv = new CSVContext();
            schema = csv.getSchema();
        }

        public void Update()
        {
            AllTracks = new List<BeheerTrack>();
            foreach (Track track in repo.GetTracksAndSectors())
            {
                AllTracks.Add(track == null ? null : BeheerTrack.ToBeheerTrack(track));
            }
            AllTrams = new List<BeheerTram>();
            foreach (Tram tram in repo.GetAllTrams())
            {
                AllTrams.Add(tram == null ? null : BeheerTram.ToBeheerTram(tram));
            }
        }

        public bool AddSector(int trackNumber)
        {
            foreach (Track track in AllTracks.Where(x => x.Number == trackNumber))
            {
                track.AddSector(new Sector(track.Sectors.Count + 1));
                repo.AddSector(track.Sectors[track.Sectors.Count - 1], track);
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
                repo.DeleteSectorFromTrack(track, track.Sectors[track.Sectors.Count - 1]);
                Update();
                return true;
            }
            return false;
        }

        public bool DeleteTram(int tramNumber)
        {
            foreach (Tram tram in AllTrams.Where(x => x.Number == tramNumber))
            {
                repo.RemoveTram(tram);
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
                repo.DeleteTrack(track);
                Update();
                return true;
            }
            return false;
        }

        public void Lock(string tracks, string sectors)
        {
            int[] lockSectors = { -1 };
            int[] lockTracks = { -1 };

            if (sectors != "")
            {
                lockSectors = Parse(sectors);
                for (int i = 0; i < lockSectors.Length; i++)
                {
                    lockSectors[i] -= 1;
                }
            }

            lockTracks = Parse(tracks);

            if (lockTracks[0] != -1)
            {
                if (sectors == "")
                {
                    foreach (Track track in AllTracks)
                    {
                        int pos = Array.IndexOf(lockTracks, track.Number);
                        if (pos > -1)
                        {
                            BeheerTrack beheerTrack = track == null ? null : BeheerTrack.ToBeheerTrack(track);
                            beheerTrack.LockTrack();
                            repo.EditTrack(beheerTrack);
                        }
                    }
                }
                else
                {
                    foreach (BeheerTrack track in AllTracks)
                    {
                        int pos = Array.IndexOf(lockTracks, track.Number);
                        if (pos > -1)
                        {
                            for (int i = 0; i < track.Sectors.Count - 1; i++)
                            {
                                pos = Array.IndexOf(lockSectors, i);
                                if (pos > -1)
                                {
                                    BeheerSector beheerSector = track.Sectors[i] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[i]);
                                    beheerSector.Lock();
                                    repo.EditSector(beheerSector);
                                }
                            }
                        }
                    }
                }
                Update();
            }
        }

        public void Unlock(string tracks, string sectors)
        {
            int[] unlockSectors = { -1 };
            int[] unlockTracks = { -1 };

            if (sectors != "")
            {
                unlockSectors = Parse(sectors);
                for (int i = 0; i < unlockSectors.Length; i++)
                {
                    unlockSectors[i] -= 1;
                }
            }
            unlockTracks = Parse(tracks);

            if (unlockTracks[0] != -1)
            {
                if (sectors == "")
                {
                    foreach (Track track in AllTracks)
                    {
                        int pos = Array.IndexOf(unlockTracks, track.Number);
                        if (pos > -1)
                        {
                            BeheerTrack beheerTrack = track == null ? null : BeheerTrack.ToBeheerTrack(track);
                            beheerTrack.UnlockTrack();
                            repo.EditTrack(beheerTrack);
                        }
                    }
                }
                else
                {
                    foreach (BeheerTrack track in AllTracks)
                    {
                        int pos = Array.IndexOf(unlockTracks, track.Number);
                        if (pos > -1)
                        {
                            for (int i = 0; i < track.Sectors.Count - 1; i++)
                            {
                                pos = Array.IndexOf(unlockSectors, i);
                                if (pos > -1)
                                {
                                    BeheerSector beheerSector = track.Sectors[i] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[i]);
                                    beheerSector.UnLock();
                                    repo.EditSector(beheerSector);
                                }
                            }
                        }
                    }
                }
                Update();
            }
        }

        public void ToggleDisabled(string trams)
        {
            int[] iTrams = Parse(trams);
            foreach (BeheerTram tram in AllTrams)
            {
                int pos = Array.IndexOf(iTrams, tram.Number);
                if (pos > -1)
                {
                    if (tram.Status == TramStatus.Defect)
                    {
                        tram.EditTramStatus(TramStatus.Idle);
                        repo.EditTram(tram);
                    }
                    else
                    {
                        tram.EditTramStatus(TramStatus.Defect);
                        repo.EditTram(tram);
                    }
                }
            }
            Update();
        }

        public bool MoveTram(int moveTram, int moveTrack, int moveSector)
        {
            moveSector -= 1;

            foreach (Track track in AllTracks.Where(x => x.Number == moveTrack && x.Sectors.Count > moveSector))
            {
                foreach (Tram tram in AllTrams.Where(x => x.Number == moveTram))
                {
                    BeheerSector beheerSector = track.Sectors[moveSector] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[moveSector]);
                    if (beheerSector.SetOccupyingTram(tram))
                    {
                        repo.WipeSectorByTramId(tram.Number);
                        repo.EditSector(beheerSector);
                        Update();
                        return true;
                    }
                }
            }
            return false;
        }

        public void AddTram(int tramNumber, int? defaultLine, string _model)
        {
            if (tramNumber != -1 && defaultLine != -1 && defaultLine != null)
            {
                TramModel model;
                Enum.TryParse<TramModel>(_model, out model);

                repo.AddTram(new Tram(tramNumber, ToInt(defaultLine), model));
                Update();
            }
        }

        public void AddTrack(int trackNumber, int sectorAmount, string _trackType, int? _defaultLine)
        {
            int? defaultLine;
            if (_defaultLine == null)
            {
                defaultLine = 0;
            }
            else
            {
                defaultLine = _defaultLine;
            }

            if (trackNumber != -1 && sectorAmount != -1)
            {
                TrackType trackType;
                Enum.TryParse<TrackType>(_trackType, out trackType);

                List<Sector> newSectors = new List<Sector>();
                for (int i = 0; i < sectorAmount; i++)
                {
                    newSectors.Add(new Sector(i + 1));
                }

                repo.AddTrack(new Track(trackNumber, defaultLine, trackType, newSectors));
                Update();
            }
        }

        public int[] Parse(string _string)
        {
            int[] array = { -1 };

            try
            {
                array = Array.ConvertAll(_string.Split(','), int.Parse);
            }
            catch (Exception e)
            {
                //TODO: Hier een fatsoenlijke trycatch van maken dat een waarschuwing in ASP.NET geeft.
            }

            return array;
        }

        public int ToInt(int? _int)
        {
            int integer = -1;

            try
            {
                integer = Convert.ToInt32(_int);
            }
            catch (Exception e)
            {
                //TODO: Hier een fatsoenlijke trycatch van maken dat een waarschuwing in ASP.NET geeft.
            }

            return integer;
        }

        public void Simulation()
        {
            //RunSimulation
            Update();
        }
    }
}