using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPS.Web.Models
{
    public class AttachPlateViewModel
    {
        [Display(Name = "Placa*")]
        [Required(ErrorMessage = "A placa é obrigatória")]
        public string PlateId { get; set; }

        [Display(Name = "Email*")]
        [Required(ErrorMessage = "O email do usuário é obrigatório")]
        public string UserEmail { get; set; }
    }
}