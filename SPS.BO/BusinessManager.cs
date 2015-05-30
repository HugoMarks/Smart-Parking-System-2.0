using SPS.Model;
using System;
using System.Collections.Generic;
using System.Linq;

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
        
        private ClientBO montlyClientBO;
        private CollaboratorBO collaboratorBO;
        private LocalManagerBO localManagerBO;
        private GlobalManagerBO globalManagerBO;
        private ParkingBO parkingBO;
        private ParkingSpaceBO parkingSpaceBO;
        private PriceBO priceBO;
        private AddressBO addressBO;
        private TagBO tagBO;
        private UsageRecordBO usageRecordsBO;

        private BusinessManager()
        {
            this.montlyClientBO = new ClientBO();
            this.collaboratorBO = new CollaboratorBO();
            this.localManagerBO = new LocalManagerBO();
            this.globalManagerBO = new GlobalManagerBO();
            this.parkingBO = new ParkingBO();
            this.parkingSpaceBO = new ParkingSpaceBO();
            this.priceBO = new PriceBO();
            this.addressBO = new AddressBO();
            this.tagBO = new TagBO();
            this.usageRecordsBO = new UsageRecordBO();
        }

        public ClientBO Clients
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

        public ParkingSpaceBO ParkingsSpaces
        {
            get
            {
                return this.parkingSpaceBO;
            }
        }


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

        public void AddOrUpdateRecord(Tag tag, Parking parking, out bool isNew)
        {
            UsageRecord lastRecord = BusinessManager.Instance.UsageRecords.FindAll()
                                           .Where(r => r.EnterDateTime.Date == DateTime.Now.Date && r.Client.CPF == tag.Client.CPF && r.IsDirty)
                                           .OrderBy(r => r.EnterDateTime)
                                           .LastOrDefault();

            if (lastRecord == null)
            {
                lastRecord = new UsageRecord()
                {
                    Client = tag.Client,
                    EnterDateTime = DateTime.Now,
                    ExitDateTime = DateTime.Now,
                    IsDirty = true,
                    Parking = parking,
                    Tag = tag
                };

                isNew = true;
                BusinessManager.Instance.UsageRecords.Add(lastRecord);
            }
            else
            {
                lastRecord.IsDirty = false;
                lastRecord.ExitDateTime = DateTime.Now;
                lastRecord.TotalHours = Convert.ToSingle((lastRecord.ExitDateTime - lastRecord.EnterDateTime).TotalHours);
                lastRecord.TotalValue = CalculatePrice(parking.CNPJ, lastRecord.EnterDateTime.TimeOfDay, lastRecord.ExitDateTime.TimeOfDay);

                isNew = false;
                BusinessManager.Instance.UsageRecords.Update(lastRecord);
            }
        }

        public decimal CalculatePrice(string parkingCNPJ, TimeSpan startTime, TimeSpan endTime)
        {
            Parking parking = Parkings.Find(parkingCNPJ);

            if (parking == null)
            {
                throw new ArgumentException("No parking with the provided CNPJ");
            }

            Price price = parking.Prices.SingleOrDefault(p => p.StartTime >= startTime && endTime <= p.EndTime);
            decimal priceValue = price.Value * Convert.ToDecimal((endTime - startTime).TotalHours);

            return priceValue;
        }
    }
}
