using SPS.Web.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPS.Web.Models
{
    public class TokenGeneratorViewModel
    {
        [Required(ErrorMessage="O CPF é obrigatório")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}