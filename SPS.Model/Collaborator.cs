﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPS.Model
{
    public class Collaborator : IUser
    {
        public virtual decimal Salary
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string Telephone
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public string RG
        {
            get;
            set;
        }

        public string CPF
        {
            get;
            set;
        }

        public Address Address
        {
            get;
            set;
        }

        public string Password
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

