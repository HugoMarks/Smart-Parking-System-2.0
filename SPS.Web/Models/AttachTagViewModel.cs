using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPS.Web.Models
{
    public class AttachTagViewModel
    {
        [Display(Name = "Tag*")]
        [Required(ErrorMessage = "O ID da tag é obrigatório")]
        public string TagId { get; set; }

        [Display(Name = "Email*")]
        [Required(ErrorMessage = "O email do usuário é obrigatório")]
        public string UserEmail { get; set; }
    }
}