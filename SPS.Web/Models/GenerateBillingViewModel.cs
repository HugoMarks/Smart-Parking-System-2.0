using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPS.Web.Models
{
    public class GenerateBillingViewModel
    {
        [Required(ErrorMessage = "A data de início é obrigatória")]
        [Display(Name = "Data de ínicio")]
        public string StartDateTime { get; set; }

        [Required(ErrorMessage = "A data de término é obrigatória")]
        [Display(Name = "Data de término")]
        public string EndDateTime { get; set; }
    }
}