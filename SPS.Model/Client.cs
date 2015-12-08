using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SPS.Model
{
    public class Client : User
    {
        public virtual IList<Tag> Tags { get; set; }

        public virtual IList<Plate> Plates { get; set; }

        public virtual IList<UsageRecord> Records { get; set; }

        public virtual IList<Parking> Parkings { get; set; }
    }
}
