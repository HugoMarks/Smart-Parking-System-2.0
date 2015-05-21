using SPS.Model;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPS.Web.Extensions
{
    public static class ParkingExtensions
    {
        public static RegisterParkingViewModel ToRegisterParkingViewModel(this Parking parking)
        {
            var model = new RegisterParkingViewModel
            {
                CNPJ = parking.CNPJ,
                LocalAdmin = parking.LocalManager.CPF,
                Name = parking.Name,
                Number = parking.StreetNumber.ToString(),
                PhoneNumber = parking.PhoneNumber
            };

            SetAddress(parking.Address, model);

            return model;
        }

        private static void SetAddress(Address address, RegisterParkingViewModel model)
        {
            model.Street = address.Street;
            model.Square = address.Square;
            model.PostalCode = address.PostalCode;
            model.State = address.State;
            model.City = address.City;
        }
    }
}