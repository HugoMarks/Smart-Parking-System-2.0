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
            var savedTag = Context.Tags.SingleOrDefault(t => t.Id == tag.Id);

            if (savedTag != null)
            {
                savedTag = tag;
                Context.SaveChanges();
            }
        }

        public virtual Tag Find(int id)
        {
            return Context.Tags.Find(id);
        }

        public virtual IList<Tag> FindAll()
        {
            return Context.Tags.ToList();
        }

    }
}

