using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SPS.Model
{
    [Table("GlobalManagers")]
    public class GlobalManager : User
    {
        public virtual string TokenHash { get; set; }
    }
}
