using SPS.BO;
using SPS.Model;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SPS.Web.Extensions
{
    public static class RegisterCollaboratorViewModelExtensions
    {
        public static ApplicationUser ToApplicationUser(this RegisterCollaboratorViewModel model, UserType userType)
        {
            return new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserType = userType
            };
        }

        public static Collaborator ToCollaborator(this RegisterCollaboratorViewModel model, string passwordHash)
        {
            var salary = decimal.Parse(model.Salary, NumberStyles.Currency, new CultureInfo("pt-br"));

            return new Collaborator
            {
                Address = GetAddress(model),
                StreetNumber = int.Parse(model.Number),
                CPF = model.CPF,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = passwordHash,
                RG = model.RG,
                Salary = salary,
                Telephone = model.PhoneNumber,
                Complement = model.Complement
            };
        }

        private static Address GetAddress(RegisterCollaboratorViewModel registerModel)
        {
            return new Address
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