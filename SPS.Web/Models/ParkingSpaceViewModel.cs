using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SPS.Model;

namespace SPS.Web.Models
{
    public class ParkingSpaceViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O número da vaga é obrigatório")]
        [Display(Name = "Número")]
        public int Number { get; set; }

        [Required(ErrorMessage = "O estado da vaga é obrigatório")]
        [Display(Name = "Estado")]
        public ParkingSpaceState Status { get; set; }
    }
}