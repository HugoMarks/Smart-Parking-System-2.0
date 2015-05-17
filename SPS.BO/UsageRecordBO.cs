using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPS.BO
{
    public class UsageRecordBO : IBusiness<UsageRecord>
    {
        private readonly SPSDb Context = SPSDb.Instance;

        public void Add(UsageRecord usageRecord)
        {
            var client = Context.Clients.Find(usageRecord.Tag.User.Id);

            client.Records.Add(usageRecord);
            BusinessManager.Instance.MontlyClients.Update(client);

            Context.UsageRecords.Add(usageRecord);
            Context.SaveChanges();
        }

        public UsageRecord Find(params object[] keys)
        {
            return Context.UsageRecords.Find(keys);
        }

        public IList<UsageRecord> FindAll()
        {
            return Context.UsageRecords.ToList();
        }

        public void Remove(UsageRecord usageRecord)
        {
            Context.UsageRecords.Remove(usageRecord);
            Context.SaveChanges();
        }

        public void Update(UsageRecord usageRecord)
        {
            var entity = Context.UsageRecords.Find(usageRecord.Id);

            if (entity == null)
                return;

            Context.Entry(entity).CurrentValues.SetValues(usageRecord);
            entity.Parking = usageRecord.Parking;
            entity.Tag = usageRecord.Tag;

            Context.SaveChanges();
        }
    }
}
