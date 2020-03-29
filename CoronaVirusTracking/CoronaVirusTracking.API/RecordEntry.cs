using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaVirusTracking.API
{
    public class RecordEntry
    {
        public string ProvinceState { get; set; }
        public string CountryRegion { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public Dictionary<string, int> DateEntry { get; set; }

        public RecordEntry()
        {
            DateEntry = new Dictionary<string, int>();
        }
    }
}
