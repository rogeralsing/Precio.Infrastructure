using System.Collections.Generic;
using System.Linq;

namespace Precio.Domain
{
    public class InMemUoW : BaseUoW
    {
        private readonly List<object> AggregateRoots = new List<object>();
        private readonly List<object> addedAggregateRoots = new List<object>();
        private readonly List<object> removedAggregateRoots = new List<object>();

        public override void Add<T>(T aggregateRoot)
        {
            AggregateRoots.Add(aggregateRoot);
            addedAggregateRoots.Add(aggregateRoot);
        }

        public override void Remove<T>(T aggregateRoot)
        {
            AggregateRoots.Remove(aggregateRoot);
            addedAggregateRoots.Remove(aggregateRoot);
            removedAggregateRoots.Add(aggregateRoot);
        }

        public override IQueryable<T> Query<T>()
        {
            return AggregateRoots.OfType<T>().AsQueryable();
        }

        public IQueryable<T> QueryAdded<T>()
        {
            return addedAggregateRoots.OfType<T>().AsQueryable();
        }

        public IQueryable<T> QueryDeleted<T>()
        {
            return removedAggregateRoots.OfType<T>().AsQueryable();
        }

        public override void Commit()
        {
        }
    }
}