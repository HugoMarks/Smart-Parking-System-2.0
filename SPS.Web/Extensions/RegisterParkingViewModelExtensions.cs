using SPS.BO;
using SPS.Model;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPS.Web.Extensions
{
    public static class RegisterParkingViewModelExtensions
    {
        public static Parking ToParking(this RegisterParkingViewModel model)
        {
            return new Parking
            {
                CNPJ = model.CNPJ,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = GetAddress(model),
                StreetNumber = int.Parse(model.Number),
                LocalManager = BusinessManager.Instance.LocalManagers.Find(model.LocalAdmin)
            };
        }

        private static Address GetAddress(RegisterParkingViewModel registerModel)
        {
            return BusinessManager.Instance.Addresses.FindAll().
                          Where(a => a.PostalCode == registerModel.PostalCode)
                          .FirstOrDefault() ??
                          new Address
                          {
                              City = registerModel.City,
                              PostalCode = registerModel.PostalCode,
                              Square = registerModel.Square,
                              State = registerModel.State,
                              Street = registerModel.Street
                          };

        }
    }
}