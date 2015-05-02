using System.ComponentModel.DataAnnotations;
namespace SPS.Model
{
    public class Root
    {
        [Key]
        public string CPF { get; set; }

        public string PasswordHash { get; set; }
    }
}
