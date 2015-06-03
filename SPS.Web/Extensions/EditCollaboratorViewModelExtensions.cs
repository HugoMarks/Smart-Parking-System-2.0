using SPS.BO;
using SPS.Model;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPS.Web.Extensions
{
    public static class EditCollaboratorViewModelExtensions
    {
        public static ApplicationUser ToApplicationUser(this EditCollaboratorViewModel model)
        {
            return new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserType = UserType.Collaborator
            };
        }

        public static Collaborator ToCollaborator(this EditCollaboratorViewModel model, string passwordHash)
        {
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
                Telephone = model.PhoneNumber,
                Salary = decimal.Parse(model.Salary)
            };
        }

        private static Address GetAddress(EditCollaboratorViewModel registerModel)
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