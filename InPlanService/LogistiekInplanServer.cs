﻿using OdoriRails.Helpers.DAL.Repository;
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

        public string FetchTramsComingIn()
        {
            return SortMovingTrams(TramLocation.ComingIn);
        }

        public string FetchTramsGoingOut()
        {
            return SortMovingTrams(TramLocation.GoingOut);
        }

        private void UpdateTracks()
        {
            _allTracks = new List<BeheerTrack>();
            foreach (Track track in _repo.GetTracksAndSectors())
            {
                _allTracks.Add(track == null ? null : BeheerTrack.ToBeheerTrack(track));
            }
        }

        private string SortMovingTrams(TramLocation location)
        {
            List<Tram> movingTrams = _repo.GetAllTramsWithLocation(location);
            if (movingTrams.Count == 0) return null;

            UpdateTracks();
            SortingAlgoritm sorter = new SortingAlgoritm(_allTracks, _repo);
            for (int i = 0; i < movingTrams.Count; i++)
            {
                BeheerTram beheerTram = BeheerTram.ToBeheerTram(movingTrams[i]);
                switch (location)
                {
                    case TramLocation.ComingIn:
                    {
                        if (movingTrams[i].DepartureTime == null)
                        {
                            GetExitTime(beheerTram);
                        }
                        return sorter.AssignTramLocation(beheerTram);
                    }
                    case TramLocation.GoingOut:
                    {
                        beheerTram.EditTramLocation(TramLocation.Out);
                        movingTrams[i] = beheerTram;
                        _repo.EditTram(movingTrams[i]);
                        _repo.WipeSectorByTramId(movingTrams[i].Number);
                        return $"Tram {beheerTram.Number} left the remise.";
                    }
                }
            }
            return null;
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

        //TODO: Simulation toevoegen.

        //    public void WipePreSimulation()
        //    {
        //        repo.WipeAllDepartureTimes();
        //        repo.WipeAllTramsFromSectors();

        //        UpdateTracks();
        //AllTrams = new List<BeheerTram>();
        //        foreach (Tram tram in repo.GetAllTrams())
        //        {
        //            AllTrams.Add(tram == null ? null : BeheerTram.ToBeheerTram(tram));
        //        }
        //    }

        //    public void Simulation()
        //    {
        //        SortingAlgoritm sorter = new SortingAlgoritm(AllTracks, repo);

        //        //De schema moet op volgorde van eerst binnenkomende worden gesorteerd
        //        schema.Sort((x, y) => x.EntryTime.CompareTo(y.EntryTime));

        //        //Voor iedere inrijtijd een tram eraan koppellen
        //        foreach (InUitRijSchema entry in schema.Where(x => x.TramNumber == null))
        //        {
        //            foreach (BeheerTram tram in AllTrams.Where(x => x.DepartureTime == null && x.Line == entry.Line))
        //            {
        //                entry.TramNumber = tram.Number;
        //                tram.EditTramDepartureTime(entry.ExitTime);
        //                break;
        //            }
        //        }

        //        //Too little linebound trams to fill each entry so overflow to other types of trams
        //        foreach (InUitRijSchema entry in schema.Where(x => x.TramNumber == null))
        //        {
        //            foreach (BeheerTram tram in AllTrams.Where(x => x.DepartureTime == null))
        //            {
        //                if ((entry.Line == 5 || entry.Line == 1624) && (tram.Model == TramModel.Dubbel_Kop_Combino || tram.Model == TramModel.TwaalfG)) //No driver lines
        //                {
        //                    entry.TramNumber = tram.Number;
        //                    tram.EditTramDepartureTime(entry.ExitTime);
        //                    break;
        //                }
        //                else if ((entry.Line != 5 || entry.Line != 1624) && tram.Model == TramModel.Combino) //Driver lines
        //                {
        //                    entry.TramNumber = tram.Number;
        //                    tram.EditTramDepartureTime(entry.ExitTime);
        //                    break;
        //                }
        //            }
        //        }

        //        //Het schema afgaan voor de simulatie
        //        foreach (InUitRijSchema entry in schema)
        //        {
        //            BeheerTram tram = AllTrams.Find(x => x.Number == entry.TramNumber);
        //            SortTram(sorter, tram);
        //            //form.Invalidate();
        //            Thread.Sleep(simulationSpeed);
        //        }

        //        foreach (BeheerTram tram in AllTrams.Where(x => x.DepartureTime == null))
        //        {
        //            SortTram(sorter, tram);
        //            //form.Invalidate();
        //            Thread.Sleep(simulationSpeed);
        //        }

        //        schema = csv.getSchema();
        //        FetchUpdates();
        //    }

        //    public int[] Parse(string _string)
        //    {
        //        int[] array = { -1 };

        //        try
        //        {
        //            array = Array.ConvertAll(_string.Split(','), int.Parse);
        //        }
        //        catch (Exception e)
        //        {
        //            //TODO: Hier een fatsoenlijke trycatch van maken dat een waarschuwing in ASP.NET geeft.
        //        }

        //        return array;
        //    }

        //    public int ToInt(string _string)
        //    {
        //        int integer = -1;

        //        try
        //        {
        //            integer = Convert.ToInt32(_string);
        //        }
        //        catch (Exception e)
        //        {
        //            //TODO: Hier een fatsoenlijke trycatch van maken dat een waarschuwing in ASP.NET geeft.
        //        }

        //        return integer;
        //    }

        //    public int ToInt(int? _int)
        //    {
        //        int integer = -1;

        //        try
        //        {
        //            integer = Convert.ToInt32(_int);
        //        }
        //        catch (Exception e)
        //        {
        //            //TODO: Hier een fatsoenlijke trycatch van maken dat een waarschuwing in ASP.NET geeft.
        //        }

        //        return integer;
        //    }
    }
}