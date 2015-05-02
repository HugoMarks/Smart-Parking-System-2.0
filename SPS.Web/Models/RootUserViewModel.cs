using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPS.Web.Models
{
    public class RootUserViewModel
    {
        [Required]
        public string CPF { get; set; }

        [Required]
        public string Token { get; set; }
    }
}