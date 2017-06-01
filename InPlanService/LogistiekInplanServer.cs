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
        private readonly I_CSVContext _csv;
        private readonly LogisticRepository _repo = new LogisticRepository();
        private List<BeheerTrack> _allTracks;
        private readonly List<InUitRijSchema> _schema;

        //private bool testing = true;
        //private int simulationSpeed = 600;

        public LogistiekInPlanServer()
        {
            //    if (testing == true)
            //    {
            //        simulationSpeed = 50;
            //    }
            _csv = new CSVContext();
            _schema = _csv.getSchema();
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
            _allTracks = new List<BeheerTrack>();
            foreach (Track track in _repo.GetTracksAndSectors())
            {
                _allTracks.Add(track == null ? null : BeheerTrack.ToBeheerTrack(track));
            }
        }

        private bool SortMovingTrams(TramLocation location)
        {
            List<Tram> movingTrams = _repo.GetAllTramsWithLocation(location);
            if (movingTrams.Count != 0)
            {
                UpdateTracks();
                SortingAlgoritm sorter = new SortingAlgoritm(_allTracks, _repo);
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
                        _repo.EditTram(movingTrams[i]);
                        _repo.WipeSectorByTramId(movingTrams[i].Number);
                        Console.WriteLine($"Tram {beheerTram.Number} left the remise.");
                    }
                }
                return true;
            }
            return false;
        }

        private DateTime? GetExitTime(BeheerTram tram)
        {
            foreach (InUitRijSchema entry in _schema.Where(entry => entry.Line == tram.Line && entry.TramNumber == null))
            {
                entry.TramNumber = tram.Number;
                return entry.ExitTime;
            }
            return null;
        }
    }
}