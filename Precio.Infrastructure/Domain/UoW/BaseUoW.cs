using System.Linq;

namespace Precio.Domain
{
    public abstract class BaseUoW
    {
        public abstract void Add<T>(T aggregateRoot) where T : class;
        public abstract void Remove<T>(T aggregateRoot) where T : class;
        public abstract IQueryable<T> Query<T>() where T : class;
        public abstract void Commit();
    }
}