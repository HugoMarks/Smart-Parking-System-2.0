using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPS.BO
{
    public class UsageRecordBO : IBusiness<UsageRecord, int>
    {
        public void Add(UsageRecord usageRecord)
        {
            using (var context = new SPSDb())
            {
                if (usageRecord.Tag != null)
                {
                    usageRecord.Tag = context.Tags.Find(usageRecord.Tag.Id);
                }

                if (usageRecord.Plate != null)
                {
                    usageRecord.Plate = context.Plates.Find(usageRecord.Plate.Id);
                }

                if (usageRecord.Parking != null)
                {
                    usageRecord.Parking = context.Parkings.Find(usageRecord.Parking.CNPJ);
                }

                if (usageRecord.Client != null)
                {
                    usageRecord.Client = context.Clients.Find(usageRecord.Client.Id);
                }

                context.UsageRecords.Add(usageRecord);
                context.SaveChanges();
            }
        }

        public UsageRecord Find(int id)
        {
            UsageRecord usageRecord = null;

            using (var context = new SPSDb())
            {
                usageRecord = context.UsageRecords
                    .Include("Tag")
                    .Include("Plate")
                    .Include("Parking")
                    .Include("Client")
                    .SingleOrDefault(c => c.Id == id);
            }

            return usageRecord;
        }

        public IList<UsageRecord> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.UsageRecords
                    .Include("Tag")
                    .Include("Plate")
                    .Include("Parking")
                    .Include("Client")
                    .ToList();
            }
        }

        public void Remove(UsageRecord usageRecord)
        {
            using (var context = new SPSDb())
            {
                context.UsageRecords.Remove(usageRecord);
                context.SaveChanges();
            }
        }

        public void Update(UsageRecord usageRecord)
        {
            using (var context = new SPSDb())
            {
                var entity = context.UsageRecords.Find(usageRecord.Id);

                if (entity == null)
                    return;

                context.Entry(entity).CurrentValues.SetValues(usageRecord);

                if (usageRecord.Tag != null)
                {
                    entity.Tag = context.Tags.Find(usageRecord.Tag.Id);
                }

                if (usageRecord.Plate != null)
                {
                    entity.Plate = context.Plates.Find(usageRecord.Plate.Id);
                }

                if (usageRecord.Parking != null)
                {
                    entity.Parking = context.Parkings.Find(usageRecord.Parking.CNPJ);
                }

                context.SaveChanges();
            }
        }
    }
}
