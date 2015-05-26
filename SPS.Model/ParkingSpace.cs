using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SPS.Model
{
    public class ParkingSpace
    {
        [Key]
        public virtual int Id { get; set; }

        public virtual int Number { get; set; }

        public virtual ParkingSpaceState Status { get; set; }

        public virtual Parking Parking { get; set; }
    }
}

