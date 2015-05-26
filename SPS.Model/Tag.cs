using System.ComponentModel.DataAnnotations;

namespace SPS.Model
{
    public class Tag
    {
        [Key]
        public virtual string Id { get; set; }

        public virtual Client Client { get; set; }
    }
}

