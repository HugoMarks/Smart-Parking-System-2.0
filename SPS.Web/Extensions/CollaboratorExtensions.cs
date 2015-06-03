using SPS.Model;
using SPS.Web.Models;

namespace SPS.Web.Extensions
{
    public static class CollaboratorExtensions
    {
        public static RegisterCollaboratorViewModel ToRegisterCollaboratorViewModel(this Collaborator collaborator)
        {
            var collaboratorViewModel = new RegisterCollaboratorViewModel
            {
                CPF = collaborator.CPF,
                Email = collaborator.Email,
                FirstName = collaborator.FirstName,
                LastName = collaborator.LastName,
                Number = collaborator.StreetNumber.ToString(),
                PhoneNumber = collaborator.Telephone,
                RG = collaborator.RG,
                Salary = collaborator.Salary.ToString()
            };

            SetAddress(collaborator.Address, collaboratorViewModel);

            return collaboratorViewModel;
        }

        public static EditCollaboratorViewModel ToEditCollaboratorViewModel(this Collaborator collaborator)
        {
            var collaboratorViewModel = new EditCollaboratorViewModel
            {
                CPF = collaborator.CPF,
                Email = collaborator.Email,
                FirstName = collaborator.FirstName,
                LastName = collaborator.LastName,
                Number = collaborator.StreetNumber.ToString(),
                PhoneNumber = collaborator.Telephone,
                RG = collaborator.RG,
                Salary = collaborator.Salary.ToString()
            };

            SetAddress(collaborator.Address, collaboratorViewModel);

            return collaboratorViewModel;
        }

        private static void SetAddress(Address address, RegisterCollaboratorViewModel collaboratorViewModel)
        {
            collaboratorViewModel.Street = address.Street;
            collaboratorViewModel.Square = address.Square;
            collaboratorViewModel.PostalCode = address.PostalCode;
            collaboratorViewModel.State = address.State;
            collaboratorViewModel.City = address.City;
        }

        private static void SetAddress(Address address, EditCollaboratorViewModel collaboratorViewModel)
        {
            collaboratorViewModel.Street = address.Street;
            collaboratorViewModel.Square = address.Square;
            collaboratorViewModel.PostalCode = address.PostalCode;
            collaboratorViewModel.State = address.State;
            collaboratorViewModel.City = address.City;
        }
    }
}