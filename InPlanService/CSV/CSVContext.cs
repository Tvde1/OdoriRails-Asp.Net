using OdoriRails.Helpers.LogistiekBeheersysteem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlanService
{
    public class CSVContext : I_CSVContext
    {
        //string path = @"..\..\DAL\InUitRijSchema.csv"; //Select for testing file in project
        string path = @"Uitnummerlijst.csv"; //Select for testing file in executable folder

        public List<InUitRijSchema> getSchema()
        {
            List<InUitRijSchema> schema = new List<InUitRijSchema>();

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {

                    string[] schemaArray;
                    string headerLine = reader.ReadLine();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        schemaArray = line.Split(';');
                        if (schemaArray[1] == "16" || schemaArray[1] == "24")
                        {
                            schema.Add(new InUitRijSchema(schemaArray[0], schemaArray[6], 1624));
                        }
                        else
                        {
                            schema.Add(new InUitRijSchema(schemaArray[0], schemaArray[6], Convert.ToInt32(schemaArray[1])));
                        }
                    }
                }
                return schema;
            }
            catch
            {
                //MessageBox.Show("Bestand uitlezen mislukt");
            }
            return null;
        }
    }
}
