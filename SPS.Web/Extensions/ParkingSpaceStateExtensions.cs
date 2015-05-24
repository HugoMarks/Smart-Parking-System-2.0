using SPS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPS.Web.Extensions
{
    public static class ParkingSpaceStateExtensions
    {
        public static string GetName(this ParkingSpaceState parkingSpaceState)
        {
            switch (parkingSpaceState)
            {
                case ParkingSpaceState.Free:
                    return "Livre";
                case ParkingSpaceState.Busy:
                    return "Ocupado";
                case ParkingSpaceState.Maintenance:
                    return "Em manutenção";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}