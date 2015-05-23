using SPS.Model;
using SPS.Web.Models;

namespace SPS.Web.Extensions
{
    public static class LocalAdminExtensions
    {
        public static RegisterLocalAdminViewModel ToRegisterLocalAdminViewModel(this LocalManager localAdmin)
        {
            var localAdminViewModel = new RegisterLocalAdminViewModel
            {
                CPF = localAdmin.CPF,
                Email = localAdmin.Email,
                FirstName = localAdmin.FirstName,
                LastName = localAdmin.LastName,
                Number = localAdmin.StreetNumber.ToString(),
                PhoneNumber = localAdmin.Telephone,
                RG = localAdmin.RG,
                Complement = localAdmin.Complement
            };

            SetAddress(localAdmin.Address, localAdminViewModel);

            return localAdminViewModel;
        }

        public static EditLocalAdminViewModel ToEditLocalAdminViewModel(this LocalManager localAdmin)
        {
            var localAdminViewModel = new EditLocalAdminViewModel
            {
                CPF = localAdmin.CPF,
                Email = localAdmin.Email,
                FirstName = localAdmin.FirstName,
                LastName = localAdmin.LastName,
                Number = localAdmin.StreetNumber.ToString(),
                PhoneNumber = localAdmin.Telephone,
                RG = localAdmin.RG,
                Complement = localAdmin.Complement
            };

            SetAddress(localAdmin.Address, localAdminViewModel);

            return localAdminViewModel;
        }

        public static FullEditLocalAdminViewModel ToFullEditLocalAdminViewModel(this LocalManager localAdmin)
        {
            var localAdminViewModel = new FullEditLocalAdminViewModel
            {
                CPF = localAdmin.CPF,
                Email = localAdmin.Email,
                FirstName = localAdmin.FirstName,
                LastName = localAdmin.LastName,
                Number = localAdmin.StreetNumber.ToString(),
                PhoneNumber = localAdmin.Telephone,
                RG = localAdmin.RG,
                Complement = localAdmin.Complement
            };

            SetAddress(localAdmin.Address, localAdminViewModel);

            return localAdminViewModel;
        }

        private static void SetAddress(Address address, RegisterLocalAdminViewModel localAdminViewModel)
        {
            localAdminViewModel.Street = address.Street;
            localAdminViewModel.Square = address.Square;
            localAdminViewModel.PostalCode = address.PostalCode;
            localAdminViewModel.State = address.State;
            localAdminViewModel.City = address.City;
        }

        private static void SetAddress(Address address, EditLocalAdminViewModel localAdminViewModel)
        {
            localAdminViewModel.Street = address.Street;
            localAdminViewModel.Square = address.Square;
            localAdminViewModel.PostalCode = address.PostalCode;
            localAdminViewModel.State = address.State;
            localAdminViewModel.City = address.City;
        }
    }
}