using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPS.Web.Models
{
    public class RegisterParkingViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome*")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [Display(Name = "CNPJ*")]
        public string CNPJ { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [Display(Name = "Telefone*")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório")]
        [Display(Name = "CEP*")]
        [StringLength(9)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "A rua é obrigatória")]
        [Display(Name = "Rua")]
        public string Street { get; set; }

        [Required(ErrorMessage = "O número da residência é obrigatório")]
        [Display(Name = "Número*")]
        public string Number { get; set; }

        [Display(Name = "Complemento")]
        public string Complement { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório")]
        [Display(Name = "Bairro")]
        public string Square { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória")]
        [Display(Name = "Cidade")]
        public string City { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório")]
        [Display(Name = "Estado")]
        public string State { get; set; }

        [Required(ErrorMessage = "O estacionamento precisa de um administrador local")]
        [Display(Name = "Administrador Local")]
        public string LocalAdmin { get; set; }
    }
}