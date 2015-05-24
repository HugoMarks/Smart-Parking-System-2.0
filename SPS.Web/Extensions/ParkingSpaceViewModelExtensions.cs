using SPS.Model;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPS.Web.Extensions
{
    public static class ParkingSpaceViewModelExtensions
    {
        public static ParkingSpace ToParkingSpace(this ParkingSpaceViewModel model)
        {
            return new ParkingSpace
            {
                Id = model.Id,
                Number = model.Number,
                Status = model.Status
            };
        }

        public static ParkingSpaceViewModel ToParkingSpaceViewModel(this ParkingSpace parkingSpace)
        {
            return new ParkingSpaceViewModel
            {
                Id = parkingSpace.Id,
                Number = parkingSpace.Number,
                Status = parkingSpace.Status
            };
        }
    }
}