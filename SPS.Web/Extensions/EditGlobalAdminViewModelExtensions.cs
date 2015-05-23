using System.Linq;
using SPS.BO;
using SPS.Model;
using SPS.Web.Models;

namespace SPS.Web.Extensions
{
    public static class EditGlobalAdminViewModelExtensions
    {
        public static EditGlobalAdminViewModel ToEditGlobalAdminViewModel(this GlobalManager globalManager)
        {
            var model = new EditGlobalAdminViewModel
            {
                CPF = globalManager.CPF,
                Email = globalManager.Email,
                FirstName = globalManager.FirstName,
                LastName = globalManager.LastName,
                PhoneNumber = globalManager.Telephone,
                RG = globalManager.RG,
                Number = globalManager.StreetNumber.ToString(),
                Complement = globalManager.Complement
            };

            SetAddress(globalManager.Address, model);

            return model;
        }
        public static GlobalManager ToGlobalManager(this EditGlobalAdminViewModel model, string passwordHash, string tokenHash)
        {
            return new GlobalManager
            {
                Address = GetAddress(model),
                CPF = model.CPF,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Telephone = model.PhoneNumber,
                RG = model.RG,
                Complement = model.Complement,
                StreetNumber = int.Parse(model.Number),
                Password = passwordHash,
                TokenHash = tokenHash
            };
        }

        private static void SetAddress(Address address, EditGlobalAdminViewModel model)
        {
            model.Street = address.Street;
            model.Square = address.Square;
            model.PostalCode = address.PostalCode;
            model.State = address.State;
            model.City = address.City;
        }

        private static Address GetAddress(EditGlobalAdminViewModel registerModel)
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