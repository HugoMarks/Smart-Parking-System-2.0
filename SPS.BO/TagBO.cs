using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class TagBO : IBusiness<Tag>
    {
        private static SPSDb Context = SPSDb.Instance;

        public virtual void Add(Tag tag)
        {
            Context.Tags.Add(tag);
            Context.SaveChanges();
        }

        public virtual void Remove(Tag tag)
        {
            Context.Tags.Remove(tag);
            Context.SaveChanges();
        }

        public virtual void Update(Tag tag)
        {
            var entity = Context.Tags.Find(tag.Id);

            if (entity == null)
                return;

            Context.Entry(entity).CurrentValues.SetValues(tag);
            entity.User = tag.User;

            Context.SaveChanges();
        }

        public virtual Tag Find(params object[] keys)
        {
            return Context.Tags.Find(keys);
        }

        public virtual IList<Tag> FindAll()
        {
            return Context.Tags.ToList();
        }

    }
}

