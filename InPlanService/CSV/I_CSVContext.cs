﻿using OdoriRails.Helpers.LogistiekBeheersysteem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlanService
{
    public interface I_CSVContext
    {
        List<InUitRijSchema> getSchema();
    }
}
