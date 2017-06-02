using System;
using System.Runtime.Serialization;

namespace InPlanService.CSV
{
    [Serializable]
    internal class CouldNotReadCSVFileExeption : Exception
    {
        public CouldNotReadCSVFileExeption()
        {
        }
    }
}