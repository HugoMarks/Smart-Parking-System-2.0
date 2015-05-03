using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SPS.Web.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CPFAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            string input = (string)value;
            var cpf = digitsOnly.Replace(input, "");

            if (cpf == string.Empty)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            if (cpf.Length != 11 ||
                cpf == "00000000000" ||
                cpf == "11111111111" ||
                cpf == "22222222222" ||
                cpf == "33333333333" ||
                cpf == "44444444444" ||
                cpf == "55555555555" ||
                cpf == "66666666666" ||
                cpf == "77777777777" ||
                cpf == "88888888888" ||
                cpf == "99999999999")
            {
                return new ValidationResult("CPF inválido");
            }

            // Valida primeiro dígito 
            var add = 0;

            for (int i = 0; i < 9; i++)
                add += Convert.ToInt32(cpf[i]) * (10 - i);

            var rev = 11 - (add % 11);

            if (rev == 10 || rev == 11)
                rev = 0;

            if (rev != Convert.ToInt32(cpf[9]))
            {
                return new ValidationResult("CPF inválido");
            }

            // Valida segundo dígito 
            add = 0;

            for (int i = 0; i < 10; i++)
                add += Convert.ToInt32(cpf[i]) * (11 - i);

            rev = 11 - (add % 11);

            if (rev == 10 || rev == 11)
                rev = 0;

            if (rev != Convert.ToInt32(cpf[10]))
            {
                return new ValidationResult("CPF inválido");
            }

            return ValidationResult.Success;
        }
    }
}