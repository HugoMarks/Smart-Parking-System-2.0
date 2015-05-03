using SPS.Model;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPS.Web.Extensions
{
    public static class RegisterViewModelExtensions
    {
        public static ApplicationUser ToApplicationUser(this RegisterViewModel registerModel)
        {
            return new ApplicationUser
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                UserName = registerModel.Email,
                Email = registerModel.Email,
                PhoneNumber = registerModel.PhoneNumber
            };
        }

        public static MonthlyClient ToMonthlyClient(this RegisterViewModel registerModel, string passwordHash)
        {
            return new MonthlyClient
            {
                Address = new Address
                {
                    City = registerModel.City,
                    Number = uint.Parse(registerModel.Number),
                    PostalCode = registerModel.PostalCode,
                    Square = registerModel.Square,
                    State = registerModel.State,
                    Street = registerModel.Street
                },
                CPF = registerModel.CPF,
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Password = passwordHash,
                RG = registerModel.RG,
                Telephone = registerModel.PhoneNumber
            };
        }
    }
}