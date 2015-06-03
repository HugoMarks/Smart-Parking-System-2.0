using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SPS.Model
{
    public class Price
    {
        [Index]
        [Key]
        public int Id { get; set; }

        public virtual decimal Value { get; set; }

        public virtual TimeSpan StartTime { get; set; }

        public virtual Parking Parking { get; set; }

        public virtual TimeSpan EndTime { get; set; }

        public virtual bool IsDefault { get; set; }
    }
}

