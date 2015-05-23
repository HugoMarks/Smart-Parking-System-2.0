using SPS.Web.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPS.Web.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome*")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress]
        [Display(Name = "Email*")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [Display(Name = "Telefone*")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "O Assunto é obrigatório")]
        [Display(Name = "Assunto*")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "A Mensagem é obrigatória")]
        [Display(Name = "Mensagem*")]
        public string Message { get; set; }

    
    }
}