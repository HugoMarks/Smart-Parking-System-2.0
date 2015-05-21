using SPS.Web.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPS.Web.Models
{
    public class EditGlobalAdminViewModel
    {
        [Display(Name = "Primeiro nome")]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome")]
        public string LastName { get; set; }

        [Display(Name = "Celular")]
        public string PhoneNumber { get; set; }

        [Display(Name = "CEP")]
        [StringLength(9)]
        public string PostalCode { get; set; }

        [Display(Name = "Rua")]
        public string Street { get; set; }

        [Display(Name = "Número")]
        public string Number { get; set; }

        [Display(Name = "Complemento")]
        public string Complement { get; set; }

        [Display(Name = "Bairro")]
        public string Square { get; set; }

        [Display(Name = "Cidade")]
        public string City { get; set; }

        [Display(Name = "Estado")]
        public string State { get; set; }

        [Display(Name = "RG")]
        public string RG { get; set; }

        [Display(Name = "CPF")]
        public string CPF { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string Password { get; set; }

        [StringLength(100, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nova Senha")]
        [Compare("NewPassword", ErrorMessage = "A senha e a senha de confirmação não correspondem.")]
        public string ConfirmNewPassword { get; set; }

        [StringLength(6, ErrorMessage = "O token deve ter 6 dígitos", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Novo Token")]
        public string Token { get; set; }
    }
}