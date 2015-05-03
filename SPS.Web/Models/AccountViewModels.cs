using System.ComponentModel.DataAnnotations;

namespace SPS.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O/A {0} deve ter no mínimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nova senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a senha de confirmação não coincidem.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembrar-me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Primeiro nome*")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        [Display(Name = "Sobrenome*")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "O celular é obrigatório")]
        [Display(Name = "Celular*")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório")]
        [Display(Name = "CEP*")]
        [StringLength(9)]
        public string PostalCode { get; set; }

        [Display(Name = "Rua")]
        public string Street { get; set; }

		[Required(ErrorMessage = "O número da residência é obrigatório")]
        [Display(Name = "Número*")]
        public string Number { get; set; }

        [Display(Name = "Complemento")]
        public string Complement { get; set; }

        [Display(Name = "Bairro")]
        public string Square { get; set; }

        [Display(Name = "Cidade")]
        public string City { get; set; }

        [Display(Name = "Estado")]
        public string State { get; set; }

        [Required(ErrorMessage = "O RG é obrigatório")]
        [Display(Name = "RG*")]
        public string RG { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [Display(Name = "CPF*")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress]
        [Display(Name = "Email*")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, ErrorMessage = "O/A {0} deve ter no mínimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha*")]
        public string Password { get; set; }

		[Required(ErrorMessage = "A confirmação de senha é obrigatória")]
		[DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha*")]
        [Compare("Password", ErrorMessage = "A senha e a senha de confirmação não correspondem.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("Password", ErrorMessage = "A senha e a senha de confirmação não coincidem.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
