using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.LogistiekBeheersysteem;
using OdoriRails.Helpers.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using InPlanService.CSV;

namespace InPlanService
{
    public class LogistiekInPlanServer
    {
        private I_CSVContext csv;
        private LogisticRepository repo = new LogisticRepository();
        private List<BeheerTrack> allTracks;
        private List<InUitRijSchema> schema;
        
        public LogistiekInPlanServer()
        {
            csv = new CSVContext();
            schema = csv.getSchema();
        }

        public bool FetchTramsGoingOut()
        {
            return SortMovingTrams(TramLocation.GoingOut);
        }

        public bool FetchTramsComingIn()
        {
            return SortMovingTrams(TramLocation.ComingIn);
        }

        private void UpdateTracks()
        {
            allTracks = new List<BeheerTrack>();
            foreach (Track track in repo.GetTracksAndSectors())
            {
                allTracks.Add(track == null ? null : BeheerTrack.ToBeheerTrack(track));
            }
        }

        public bool SortMovingTrams(TramLocation location)
        {
            List<Tram> movingTrams = repo.GetAllTramsWithLocation(location);
            if (movingTrams.Count != 0)
            {
                UpdateTracks();
                SortingAlgoritm sorter = new SortingAlgoritm(allTracks, repo);
                for (int i = 0; i < movingTrams.Count; i++)
                {
                    BeheerTram beheerTram = BeheerTram.ToBeheerTram(movingTrams[i]);
                    if (location == TramLocation.ComingIn)
                    {
                        if (movingTrams[i].DepartureTime == null)
                        {
                            GetExitTime(beheerTram);
                        }
                        sorter.AssignTramLocation(beheerTram);
                    }
                    else if (location == TramLocation.GoingOut)
                    {
                        beheerTram.EditTramLocation(TramLocation.Out);
                        movingTrams[i] = beheerTram;
                        repo.EditTram(movingTrams[i]);
                        repo.WipeSectorByTramId(movingTrams[i].Number);
                        Console.WriteLine("Tram {0} left the remise.", beheerTram.Number);
                    }
                }
                return true;
            }
            return false;
        }

        private DateTime? GetExitTime(BeheerTram tram)
        {
            foreach (InUitRijSchema entry in schema.Where(entry => entry.Line == tram.Line && entry.TramNumber == null))
            {
                entry.TramNumber = tram.Number;
                return entry.ExitTime;
            }
            return null;
        }
    }
}