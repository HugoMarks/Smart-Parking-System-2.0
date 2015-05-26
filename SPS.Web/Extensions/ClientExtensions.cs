using SPS.Model;
using SPS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPS.Web.Extensions
{
    public static class ClientExtensions
    {
        public static EditClientViewModel ToEditClientViewModel(this Client client)
        {
            var model = new EditClientViewModel
            {
                CPF = client.CPF,
                Complement = client.Complement,
                Email = client.Email,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Number = client.StreetNumber.ToString(),
                PhoneNumber = client.Telephone,
                RG = client.RG
            };

            SetAddress(client.Address, model);

            return model;
        }

        private static void SetAddress(Address address, EditClientViewModel model)
        {
            model.Street = address.Street;
            model.Square = address.Square;
            model.PostalCode = address.PostalCode;
            model.State = address.State;
            model.City = address.City;
        }
    }
}