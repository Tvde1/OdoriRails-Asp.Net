using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.LogistiekBeheersysteem;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Tests
{
    [TestClass]
    public class LogistiekTests
    {
        private readonly LogisticRepository _logisticRepository = new LogisticRepository();

        [TestMethod]
        public void AddTram()
        {
            var tram = new Tram(1, TramStatus.Idle, 0, null, TramModel.Combino, TramLocation.In, null);

            if (_logisticRepository.DoesTramExist(tram.Number))
                RemoveTram();

            _logisticRepository.AddTram(tram);

            var fetchedTram = _logisticRepository.GetTram(1);
            Assert.AreEqual(JsonConvert.SerializeObject(tram), JsonConvert.SerializeObject(fetchedTram));
        }

        [TestMethod]
        public void EditTram()
        {
            var tram = _logisticRepository.GetTram(1);
            Assert.IsNotNull(tram);

            var newTram = BeheerTram.ToBeheerTram(tram);
            newTram.EditTramStatus(TramStatus.Cleaning);
            newTram.EditTramDepartureTime(new DateTime(2017, 6, 15, 08, 0, 0, DateTimeKind.Unspecified));
            newTram.EditTramLocation(TramLocation.Out);

            _logisticRepository.EditTram(newTram);

            var fetchTram = _logisticRepository.GetTram(1);
            Assert.IsNotNull(fetchTram);

            Assert.AreEqual(JsonConvert.SerializeObject(newTram), JsonConvert.SerializeObject(fetchTram));
        }

        [TestMethod]
        public void RemoveTram()
        {
            var tram = _logisticRepository.GetTram(1);

            _logisticRepository.RemoveTram(tram);

            var fetchedTram = _logisticRepository.GetTram(1);
            Assert.IsNull(fetchedTram);
        }

        [TestMethod]
        public void AddTrack()
        {
            var track = new Track(1000, 1, TrackType.Normal);

            var existTrack = _logisticRepository.GetTrack(track.Number);
            if (existTrack != null) RemoveTrack();

            _logisticRepository.AddTrack(track);

            var fetchTrack = _logisticRepository.GetTrack(track.Number);
            Assert.AreEqual(JsonConvert.SerializeObject(track), JsonConvert.SerializeObject(fetchTrack));
        }

        public void EditTrack()
        {
            var track = _logisticRepository.GetTrack(1000);

            var newTrack = BeheerTrack.ToBeheerTrack(track);
            newTrack.LockTrack();

            _logisticRepository.EditTrack(newTrack);

            var fetchTrack = _logisticRepository.GetTrack(1000);
            Assert.AreEqual(JsonConvert.SerializeObject(track), JsonConvert.SerializeObject(fetchTrack));
        }

        [TestMethod]
        public void RemoveTrack()
        {
            var track = _logisticRepository.GetTrack(1000);

            _logisticRepository.DeleteTrack(track);

            var fetchedTrack = _logisticRepository.GetTrack(1000);

            Assert.IsNull(fetchedTrack);
        }

        [TestMethod]
        public void GetAllTrams()
        {

            var tram = _logisticRepository.GetTram(999);
            bool found = false;

            foreach (Tram item in _logisticRepository.GetAllTrams())
            {

                if (JsonConvert.SerializeObject(tram) == JsonConvert.SerializeObject(item))
                {
                    found = true;
                }

            }
            Assert.IsTrue(found);
        }

    }
}