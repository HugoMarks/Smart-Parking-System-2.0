using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SPS.Model
{
    public class GlobalManager : User
    {
        public virtual string TokenHash { get; set; }
    }
}
