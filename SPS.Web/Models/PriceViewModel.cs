using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPS.Web.Models
{
    public class PriceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A hora de início é obrigatória")]
        [Display(Name = "Hora de início")]
        public string StartTime { get; set; }

        [Required(ErrorMessage = "A hora de término é obrigatória")]
        [Display(Name = "Hora de término")]
        public string EndTime { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Display(Name = "Preço")]
        [DataType(DataType.Currency)]
        public string Price { get; set; }

        [Display(Name = "Preço padrão")]
        public bool IsDefault { get; set; }
    }
}