﻿using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.LogistiekBeheersysteem;
using OdoriRails.Helpers.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using InPlanService.CSV;
using InPlanService.Logic;

namespace InPlanService.Logic
{
    public class LogistiekInPlan
    {
        private I_CSVContext _csv;
        private LogisticRepository _repo = new LogisticRepository();
        private List<BeheerTrack> _allTracks;
        private List<InUitRijSchema> _schema;

        public LogistiekInPlan()
        {
            _csv = new CSVContext();
            try
            {
                _schema = _csv.getSchema();
            }
            catch (CouldNotReadCSVFileExeption)
            {
                Console.WriteLine("ERROR: Could not find Uitnummerlijst.CSV");
            }
        }

        public void FetchMovingTrams()
        {
            SortMovingTrams(TramLocation.GoingOut);
            SortMovingTrams(TramLocation.ComingIn);
        }

        private bool SortMovingTrams(TramLocation location)
        {
            List<Tram> movingTrams = _repo.GetAllTramsWithLocation(location);
            if (movingTrams.Count != 0)
            {
                UpdateTracks();
                TramSortingAlgoritm sorter = new TramSortingAlgoritm(_allTracks, _repo);
                for (int i = 0; i < movingTrams.Count; i++)
                {
                    BeheerTram beheerTram = BeheerTram.ToBeheerTram(movingTrams[i]);
                    if (location == TramLocation.ComingIn)
                    {
                        if (beheerTram.DepartureTime == null)
                        {
                            beheerTram.EditTramDepartureTime(GetExitTime(beheerTram));                            
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

        private void UpdateTracks()
        {
            _allTracks = new List<BeheerTrack>();
            foreach (Track track in _repo.GetTracksAndSectors())
            {
                _allTracks.Add(track == null ? null : BeheerTrack.ToBeheerTrack(track));
            }
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