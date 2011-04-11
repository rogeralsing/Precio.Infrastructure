using System.Linq;
using System;
namespace Precio.Domain
{
    public interface IHaveId<TId>
    {
        TId Id { get; }
    }

    public interface AggregateRoot<T, TId> :  IHaveId<TId> where T : class,IHaveId<TId> 
    {

    }

    public static class EntityExtensions
    {
        public static IQueryable<T> Query<T, TId>(this AggregateRoot<T, TId> self) where T : class,IHaveId<TId>
        {
            return UoW
                .Query<T>()
                .Where(ar => ar.Id.Equals(self.Id));
        }
    }
}