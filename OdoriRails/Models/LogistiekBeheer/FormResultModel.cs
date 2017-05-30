﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdoriRails.Models
{
    public class FormResultModel
    {
        public int TramNumber { get; set; }
        public string TramModel { get; set; }
        public int TrackNumber { get; set; }
        public string TrackType { get; set; }
        public int SectorNumber { get; set; }
        public int SectorAmount { get; set; }
        public int DefaultLine { get; set; }
        public int RadioButton { get; set; }
    }
}