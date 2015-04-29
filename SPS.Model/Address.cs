using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SPS.Model
{
	[Serializable]
    public class Address
    {
        [Key]
        public virtual int Id
        {
            get;
            set;
        }

        public virtual string Street
        {
            get;
            set;
        }

        public virtual string ZipCode
        {
            get;
            set;
        }

        public virtual string City
        {
            get;
            set;
        }

        public virtual uint Number
        {
            get;
            set;
        }

        public virtual string State
        {
            get;
            set;
        }
    }
}

