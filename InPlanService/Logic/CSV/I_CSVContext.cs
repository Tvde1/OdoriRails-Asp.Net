using System.Collections.Generic;
using OdoriRails.Helpers.LogistiekBeheersysteem;

namespace InPlanService.CSV
{
    public interface I_CSVContext
    {
        List<InUitRijSchema> getSchema();
    }
}