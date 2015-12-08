using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPS.Web.Models
{
    public class AuthorizationModel
    {
        public string TagId { get; set; }

        public string ParkingCNPJ { get; set; }

        public string CarPlate { get; set; }
    }
}