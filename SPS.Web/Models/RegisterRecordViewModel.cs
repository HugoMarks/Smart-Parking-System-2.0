using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPS.Web.Models
{
    public class SaveUsageRecordViewModel
    {
        [Required(ErrorMessage = "O número da tag é obrigatório")]
        [Display(Name = "Tag")]
        public string Tag { get; set; }

        [Required(ErrorMessage = "A data de entrada é obrigatória")]
        [Display(Name = "Horário de entrada")]
        public string EnterDateTime { get; set; }

        [Required(ErrorMessage = "A data de saída é obrigatória")]
        [Display(Name = "Horário de saída")]
        public string ExitDateTime { get; set; }

        [Required(ErrorMessage = "O total de horas é obrigatório")]
        [Display(Name = "Total de horas")]
        public string TotalHours { get; set; }

        [Required(ErrorMessage = "O total à pagar é obrigatório")]
        [Display(Name = "Total à pagar")]
        public string TotalCash { get; set; }

        public string ParkingCNPJ { get; set; }
    }
}