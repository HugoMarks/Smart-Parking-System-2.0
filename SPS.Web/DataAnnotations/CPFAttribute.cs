using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SPS.Web.DataAnnotations
{
    public class CPFAttribute : ValidationAttribute
    {
        private const string InvalidCPFErrorMessage = "CPF inválido";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!ValidateCPF((string)value))
            {
                return new ValidationResult(InvalidCPFErrorMessage);
            }

            return ValidationResult.Success;
        }

        private bool ValidateCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            var onlyDigits = new Regex(@"[^\d]");

            cpf = onlyDigits.Replace(cpf, "");

            if (cpf.Length != 11)
                return false;

            bool equal = true;

            for (int i = 1; i < 11 && equal; i++)
            {
                if (cpf[i] != cpf[0])
                {
                    equal = false;
                }
            }

            if (equal || cpf == "12345678909")
            {
                return false;
            }

            int[] numbers = new int[11];

            for (int i = 0; i < 11; i++)
            {
                numbers[i] = int.Parse(cpf[i].ToString());
            }

            int sum = 0;

            for (int i = 0; i < 9; i++)
            {
                sum += (10 - i) * numbers[i];
            }

            int result = sum % 11;

            if (result == 1 || result == 0)
            {
                if (numbers[9] != 0)
                {
                    return false;
                }
            }
            else if (numbers[9] != 11 - result)
            {
                return false;
            }

            sum = 0;

            for (int i = 0; i < 10; i++)
            {
                sum += (11 - i) * numbers[i];
            }

            result = sum % 11;

            if (result == 1 || result == 0)
            {
                if (numbers[10] != 0)
                {
                    return false;
                }
            }
            else if (numbers[10] != 11 - result)
            {
                return false;
            }

            return true;
        }
    }
}