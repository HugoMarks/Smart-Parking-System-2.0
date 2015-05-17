using System;
using System.ComponentModel.DataAnnotations;

namespace SPS.Model
{
    public class UsageRecord
    {
        [Key]
        public int Id { get; set; }

        public virtual Tag Tag { get; set; }

        public virtual Parking Parking { get; set; }

        public virtual DateTime EnterDateTime { get; set; }

        public virtual DateTime ExitDateTime { get; set; }

        public virtual long TotalHours { get; set; }

        public virtual decimal TotalCash { get; set; }
    }
}
