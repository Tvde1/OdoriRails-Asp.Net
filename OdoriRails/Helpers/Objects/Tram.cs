using System;
using System.Runtime.Serialization;

namespace OdoriRails.Helpers.Objects
{
    public enum TramStatus
    {
        Idle,
        Cleaning,
        Maintenance,
        CleaningMaintenance,
        Defect
    }

    public enum TramModel
    {
        Other,
        Combino,
        ElfG,
        DubbelKopCombino,
        TwaalfG,
        Opleidingstram
    }

    public enum TramLocation
    {
        In,
        ComingIn,
        Out,
        GoingOut,
        NotAssigned
    }

    [DataContract]
    public class Tram
    {
        /// <summary>
        ///     Aanmaken nieuwe tram met bestuurder
        /// </summary>
        /// <param name="number"></param>
        /// <param name="status"></param>
        /// <param name="line"></param>
        /// <param name="driver"></param>
        /// <param name="model"></param>
        /// <param name="location"></param>
        /// <param name="departureTime"></param>
        public Tram(int number, TramStatus status, int line, User driver, TramModel model, TramLocation location,
            DateTime? departureTime)
        {
            Number = number;
            Status = status;
            Line = line;
            Driver = driver;
            Model = model;
            Location = location;
            DepartureTime = departureTime;
        }

        /// <summary>
        ///     Minimale constructor tram zonder driver en line
        /// </summary>
        /// <param name="number"></param>
        /// <param name="line"></param>
        /// <param name="model"></param>
        public Tram(int number, int line, TramModel model)
        {
            Number = number;
            Line = line;
            Model = model;
            Status = TramStatus.Idle;
            Location = TramLocation.ComingIn;
        }

        /// <summary>
        ///     Ophalen tramnummer
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        ///     Ophalen Tramstatus
        /// </summary>
        [DataMember]
        public TramStatus Status { get; set; }

        /// <summary>
        ///     Get/Set lijn waar de tram opstaat
        /// </summary>
        [DataMember]
        public int Line { get; set; }

        /// <summary>
        ///     Get/Set bestuurder van de tram
        /// </summary>
        public User Driver { get; protected set; }

        /// <summary>
        ///     Ophalen model van de tram
        /// </summary>
        [DataMember]
        public TramModel Model { get; set; }

        /// <summary>
        ///     De departure time.
        /// </summary>
        public DateTime? DepartureTime { get; protected set; }

        /// <summary>
        ///     De locatie van de tram.
        /// </summary>
        [DataMember]
        public TramLocation Location { get; set; }
    }
}