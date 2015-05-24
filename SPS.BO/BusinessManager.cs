using System;

namespace SPS.BO
{
    public class BusinessManager
    {
        private static BusinessManager _instance;

        public static BusinessManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BusinessManager();

                return _instance;
            }
        }
        
        private MontlyClientBO montlyClientBO;
        private CollaboratorBO collaboratorBO;
        private LocalManagerBO localManagerBO;
        private GlobalManagerBO globalManagerBO;
        private ParkingBO parkingBO;
        //private ParkingSpaceBO parkingSpaceBO;
        private PriceBO priceBO;
        private AddressBO addressBO;
        private TagBO tagBO;
        private UsageRecordBO usageRecordsBO;

        private BusinessManager()
        {
            this.montlyClientBO = new MontlyClientBO();
            this.collaboratorBO = new CollaboratorBO();
            this.localManagerBO = new LocalManagerBO();
            this.globalManagerBO = new GlobalManagerBO();
            this.parkingBO = new ParkingBO();
            //this.parkingSpaceBO = new ParkingSpaceBO();
            this.priceBO = new PriceBO();
            this.addressBO = new AddressBO();
            this.tagBO = new TagBO();
            this.usageRecordsBO = new UsageRecordBO();
        }

        public MontlyClientBO MontlyClients
        {
            get
            {
                return this.montlyClientBO;
            }
        }

        public CollaboratorBO Collaborators
        {
            get
            {
                return this.collaboratorBO;
            }
        }

        public LocalManagerBO LocalManagers
        {
            get
            {
                return this.localManagerBO;
            }
        }

        public GlobalManagerBO GlobalManagers
        {
            get
            {
                return this.globalManagerBO;
            }
        }

        public ParkingBO Parkings
        {
            get
            {
                return this.parkingBO;
            }
        }

        //public ParkingSpaceBO ParkingsSpaces
        //{
        //    get
        //    {
        //        return this.parkingSpaceBO;
        //    }
        //}


        public PriceBO Prices
        {
            get
            {
                return this.priceBO;
            }
        }


        public AddressBO Addresses
        {
            get
            {
                return this.addressBO;
            }
        }

        public TagBO Tags
        {
            get
            {
                return this.tagBO;
            }
        }

        public UsageRecordBO UsageRecords
        {
            get
            {
                return this.usageRecordsBO;
            }
        }
    }
}
