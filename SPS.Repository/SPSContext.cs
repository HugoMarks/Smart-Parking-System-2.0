using SPS.Model;
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
    public class SPSContext : DbContext
    {
        private static SPSContext _instance;

        /// <summary>
        /// Gets an instance of the <see cref="SPSContext"/> class.
        /// </summary>
        public static SPSContext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SPSContext();

                return _instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPSContext"/> class.
        /// </summary>
        private SPSContext() 
            : base("SPS.Web")
        {
        }

        /// <summary>
        /// Gets the Client table.
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Gets the Collaborator table.
        /// </summary>
        public DbSet<Collaborator> Collaborators { get; set; }

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

        public static SPSContext Create()
        {
            return new SPSContext();
        }
    }

    /// <summary>
    /// Provides custom initialization for the <see cref="SPSContext"/> class.
    /// </summary>
    public class SPSContextInitializer : DropCreateDatabaseAlways<SPSContext>
    {
        /// <summary>
        /// A method that should be overridden to actually add data to the context for seeding.
        /// </summary>
        /// <param name="context">The context to seed.</param>
        protected override void Seed(SPSContext context)
        {
            base.Seed(context);
        }
    }
}
