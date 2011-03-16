using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using NHibernate;

namespace Precio.Domain
{
    public class NHibernateUoW : BaseUoW
    {
        private ISession session;
        public NHibernateUoW(ISession session)
        {
            this.session = session;
        }

        public override void Add<T>(T aggregateRoot)
        {
            session.SaveOrUpdate(aggregateRoot);
        }

        public override void Remove<T>(T aggregateRoot)
        {
            session.Delete(aggregateRoot);
        }

        public override IQueryable<T> Query<T>()
        {
            return session.Linq<T>();
        }

        public override void Commit()
        {
            session.Flush();
        }
    }
}
