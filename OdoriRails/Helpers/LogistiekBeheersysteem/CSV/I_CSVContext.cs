using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdoriRails.Helpers.LogistiekBeheersysteem.CSV
{
    interface I_CSVContext
    {
        List<InUitRijSchema> getSchema();
    }
}
