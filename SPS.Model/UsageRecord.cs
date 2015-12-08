using System;
using System.ComponentModel.DataAnnotations;

namespace SPS.Model
{
    public class UsageRecord
    {
        [Key]
        public int Id { get; set; }

        public virtual Tag Tag { get; set; }

        public virtual Plate Plate { get; set; }

        public virtual Client Client { get; set; }

        public virtual Parking Parking { get; set; }

        public virtual int SpaceNumber { get; set; }

        public virtual DateTime EnterDateTime { get; set; }

        public virtual DateTime ExitDateTime { get; set; }

        public virtual float TotalHours { get; set; }

        public virtual decimal TotalValue { get; set; }

        public virtual PaymentStatus PaymentStatus { get; set; }

        public virtual bool IsDirty { get; set; }
    }
}
