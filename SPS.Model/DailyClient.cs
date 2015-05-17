using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPS.Model
{
    public class DailyClient
    {
        public uint Number { get; set; }

        public virtual Parking Parking { get; set; }
    }
}
