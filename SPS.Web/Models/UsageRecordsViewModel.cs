using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPS.Web.Models
{
    public class UsageRecordsViewModel
    {
        public string TagNumber { get; set; }

        public string ParkingName { get; set; }

        public string ParkingCNPJ { get; set; }

        public string EnterDateTime { get; set; }

        public string ExitDateTime { get; set; }

        public string TotalHours { get; set; }

        public string TotalCash { get; set; }
    }
}