﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SPS.Model
{
    [Table("Clients")]
    public class MonthlyClient : User
    {
        public virtual IList<Tag> Tags
        {
            get;
            set;
        }

        public Parking Parking
        {
            get;
            set;
        }
    }
}
