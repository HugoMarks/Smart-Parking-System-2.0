using SPS.Model;
using SPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPS.BO
{
    public class TagBO : IBusiness<Tag, string>
    {
        public virtual void Add(Tag tag)
        {
            using (var context = new SPSDb())
            {
                context.Tags.Add(tag);
                context.SaveChanges();
            }
        }

        public virtual void Remove(Tag tag)
        {
            using (var context = new SPSDb())
            {
                context.Tags.Remove(tag);
                context.SaveChanges();
            }
        }

        public virtual void Update(Tag tag)
        {
            using (var context = new SPSDb())
            {
                var entity = context.Tags.Find(tag.Id);

                if (entity == null)
                    return;

                context.Entry(entity).CurrentValues.SetValues(tag);
                entity.User = tag.User;

                context.SaveChanges();
            }
        }

        public virtual Tag Find(string id)
        {
            Tag tag = null;

            using (var context = new SPSDb())
            {
                tag = context.Tags
                    .Include("User")
                    .SingleOrDefault(c => c.Id == id);
            }

            return tag;
        }

        public virtual IList<Tag> FindAll()
        {
            using (var context = new SPSDb())
            {
                return context.Tags.ToList();
            }
        }

    }
}

