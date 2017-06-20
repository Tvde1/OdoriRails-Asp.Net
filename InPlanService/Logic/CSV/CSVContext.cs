using System;
using System.Collections.Generic;
using System.IO;
using OdoriRails.Helpers.LogistiekBeheersysteem;

namespace InPlanService.CSV
{
    public class CSVContext : I_CSVContext
    {
        private readonly string path = @"Uitnummerlijst.csv"; //Select for testing file in executable folder

        public List<InUitRijSchema> getSchema()
        {
            var schema = new List<InUitRijSchema>();

            try
            {
                using (var reader = new StreamReader(path))
                {
                    string[] schemaArray;
                    var headerLine = reader.ReadLine();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        schemaArray = line.Split(';');
                        if (schemaArray[1] == "16" || schemaArray[1] == "24")
                            schema.Add(new InUitRijSchema(schemaArray[0], schemaArray[6], 1624));
                        else
                            schema.Add(new InUitRijSchema(schemaArray[0], schemaArray[6],
                                Convert.ToInt32(schemaArray[1])));
                    }
                }
                return schema;
            }
            catch
            {
                throw new CouldNotReadCSVFileExeption();
            }
        }
    }
}