using OdoriRails.Helpers.Objects;
using OdoriRails.Helpers.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OdoriRails.Helpers.LogistiekBeheersysteem;
using OdoriRails.Helpers.LogistiekBeheersysteem.ObjectClasses;

namespace InPlanService
{
    public class SortingAlgoritm
    {
        private LogisticRepository repo;
        private List<BeheerTrack> allTracks;

        public SortingAlgoritm(List<BeheerTrack> allTracks, LogisticRepository repo)
        {
            this.repo = repo;
            this.allTracks = allTracks;
        }

        public string AssignTramLocation(BeheerTram tram)
        {
            tram.EditTramLocation(TramLocation.In);

            //With a service needed, put on the first free slot
            if (tram.Status == TramStatus.Cleaning || tram.Status == TramStatus.Maintenance || tram.Status == TramStatus.CleaningMaintenance)
            {
                foreach (Track track in allTracks.Where(track => track.Type == TrackType.Service))
                {
                    for (int i = 0; i < track.Sectors.Count; i++)
                    {
                        if (track.Sectors[i].OccupyingTram == null && track.Sectors[i].Status == SectorStatus.Open)
                        {
                            BeheerSector beheerSector = track.Sectors[i] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[i]);
                            track.Sectors[i] = Assign(beheerSector, tram);
                            return String.Format("Moved tram {0} to track: {1}, sector: {2} ({3})", tram.Number, beheerSector.TrackNumber, beheerSector.Number, tram.Status.ToString());
                        }
                    }
                }
            }

            //Put tram on track thats connected to the line the tram is on
            foreach (BeheerTrack track in allTracks.Where(track => track.Line == tram.Line && track.Type == TrackType.Normal))
            {
                for (int i = 0; i < track.Sectors.Count - 1; i++)
                {
                    if (track.Sectors[i].OccupyingTram == null && track.Sectors[i].Status == SectorStatus.Open)
                    {
                        BeheerSector beheerSector = track.Sectors[i] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[i]);
                        track.Sectors[i] = Assign(beheerSector, tram);
                        return String.Format("Moved tram {0} to track: {1}, sector: {2}", tram.Number, beheerSector.TrackNumber, beheerSector.Number);
                    }

                    if (track.Sectors[i].Status == SectorStatus.Occupied && track.Sectors[i].OccupyingTram.DepartureTime < tram.DepartureTime)
                    {
                        if (track.Sectors[i + 1].Status == SectorStatus.Open)
                        {
                            BeheerSector beheerSector = track.Sectors[i + 1] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[i + 1]);
                            track.Sectors[i + 1] = Assign(beheerSector, tram);
                            return String.Format("Moved tram {0} to track: {1}, sector: {2}", tram.Number, beheerSector.TrackNumber, beheerSector.Number);
                        }
                    }
                }
            }

            //If not successful put tram on any other normal track (that doesn't have another line connected to it)
            foreach (BeheerTrack track in allTracks.Where(track => track.Type == TrackType.Normal))
            {
                for (int i = 0; i < track.Sectors.Count - 1; i++)
                {
                    if (track.Sectors[0].OccupyingTram == null && track.Sectors[0].Status == SectorStatus.Open)
                    {
                        BeheerSector beheerSector = track.Sectors[i] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[i]);
                        track.Sectors[i] = Assign(beheerSector, tram);
                        return String.Format("Moved tram {0} to track: {1}, sector: {2}", tram.Number, beheerSector.TrackNumber, beheerSector.Number);
                    }
                    else if (track.Sectors[i].Status == SectorStatus.Occupied && track.Sectors[i].OccupyingTram.DepartureTime < tram.DepartureTime)
                    {
                        if (track.Sectors[i + 1].Status == SectorStatus.Open)
                        {
                            BeheerSector beheerSector = track.Sectors[i + 1] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[i + 1]);
                            track.Sectors[i + 1] = Assign(beheerSector, tram);
                            return String.Format("Moved tram {0} to track: {1}, sector: {2}", tram.Number, beheerSector.TrackNumber, beheerSector.Number);
                        }
                    }
                }
            }

            //If not successful put on an exit line
            foreach (BeheerTrack track in allTracks.Where(track => track.Type == TrackType.Exit))
            {
                for (int i = 0; i < track.Sectors.Count - 1; i++)
                {
                    if (track.Sectors[0].OccupyingTram == null && track.Sectors[0].Status == SectorStatus.Open)
                    {
                        BeheerSector beheerSector = track.Sectors[i] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[i]);
                        track.Sectors[i] = Assign(beheerSector, tram);
                        return String.Format("Moved tram {0} to track: {1}, sector: {2}", tram.Number, beheerSector.TrackNumber, beheerSector.Number);
                    }
                    else if (track.Sectors[i].Status == SectorStatus.Occupied && track.Sectors[i].OccupyingTram.DepartureTime < tram.DepartureTime)
                    {
                        if (track.Sectors[i + 1].Status == SectorStatus.Open)
                        {
                            BeheerSector beheerSector = track.Sectors[i + 1] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[i + 1]);
                            track.Sectors[i + 1] = Assign(beheerSector, tram);
                            return String.Format("Moved tram {0} to track: {1}, sector: {2}", tram.Number, beheerSector.TrackNumber, beheerSector.Number);
                        }
                    }
                    else if (track.Sectors[i].Status == SectorStatus.Occupied && track.Sectors[0].OccupyingTram.DepartureTime == null)
                    {
                        if (track.Sectors[i + 1].Status == SectorStatus.Open)
                        {
                            BeheerSector beheerSector = track.Sectors[i + 1] == null ? null : BeheerSector.ToBeheerSector(track.Sectors[i + 1]);
                            track.Sectors[i + 1] = Assign(beheerSector, tram);
                            return String.Format("Moved tram {0} to track: {1}, sector: {2}", tram.Number, beheerSector.TrackNumber, beheerSector.Number);
                        }
                    }
                }
            }

            //If not successful let user place tram
            return String.Format("Could not move tram {0}, please move manually.", tram.Number);
        }

        public BeheerSector Assign(BeheerSector sector, BeheerTram tram)
        {
            Console.WriteLine("test");
            sector.SetOccupyingTram(tram);
            repo.EditTram(tram);
            repo.EditSector(sector);
            return sector;
        }
    }
}