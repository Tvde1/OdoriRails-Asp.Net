using OdoriRails.Helpers.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdoriRails.Models
{
    public class GoogleMapsModel
    {
        List<Tram> Trams { get; set; }
        List<Track> Tracks { get; set; }
    }
}