﻿using SPS.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPS.Repository
{
    /// <summary>
    /// The SPS database access layer.
    /// </summary>
    public class SPSDb : DbContext
    {
        private static SPSDb _instance;

        /// <summary>
        /// Gets an instance of the <see cref="SPSDb"/> class.
        /// </summary>
        public static SPSDb Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SPSDb();

                return _instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPSDb"/> class.
        /// </summary>
        public SPSDb()
            : base("DefaultConnection")
        {
        }

        /// <summary>
        /// Gets the Root table.
        /// </summary>
        public DbSet<GlobalManager> GlobalManagers { get; set; }

        /// <summary>
        /// Gets the Client table.
        /// </summary>
        public DbSet<MonthlyClient> Clients { get; set; }

        /// <summary>
        /// Gets the Collaborator table.
        /// </summary>
        public DbSet<Collaborator> Collaborators { get; set; }

        /// <summary>
        /// Gets the LocalManager table.
        /// </summary>
        public DbSet<LocalManager> LocalManagers { get; set; }

        /// <summary>
        /// Gets the Parking table.
        /// </summary>
        public DbSet<Parking> Parkings { get; set; }

        /// <summary>
        /// Gets the ParkingSpace table.
        /// </summary>
        public DbSet<ParkingSpace> ParkingSpaces { get; set; }

        /// <summary>
        /// Gets the Tag table.
        /// </summary>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// Gets the Price table.
        /// </summary>
        public DbSet<Price> Prices { get; set; }

        /// <summary>
        /// Gets the Address table.
        /// </summary>
        public DbSet<Address> Addresses { get; set; }
    }
}
