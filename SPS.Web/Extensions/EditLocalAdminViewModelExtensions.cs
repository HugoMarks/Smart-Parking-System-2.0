using SPS.BO;
using SPS.Model;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPS.Web.Extensions
{
    public static class EditLocalAdminViewModelExtensions
    {
        public static ApplicationUser ToApplicationUser(this EditLocalAdminViewModel model)
        {
            return new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserType = UserType.LocalAdmin
            };
        }

        public static LocalManager ToLocalAdmin(this EditLocalAdminViewModel model, string passwordHash)
        {
            return new LocalManager
            {
                Address = GetAddress(model),
                StreetNumber = int.Parse(model.Number),
                CPF = model.CPF,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = passwordHash,
                RG = model.RG,
                Telephone = model.PhoneNumber
            };
        }

        public static LocalManager ToLocalAdmin(this FullEditLocalAdminViewModel model, string passwordHash)
        {
            return new LocalManager
            {
                Address = GetAddress(model),
                StreetNumber = int.Parse(model.Number),
                CPF = model.CPF,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = passwordHash,
                RG = model.RG,
                Telephone = model.PhoneNumber
            };
        }

        private static Address GetAddress(EditLocalAdminViewModel registerModel)
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