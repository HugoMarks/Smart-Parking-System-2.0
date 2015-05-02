using System.ComponentModel.DataAnnotations;
namespace SPS.Model
{
    public class Root
    {
        [Key]
        public string Token { get; set; }

        public string Password { get; set; }
    }
}
