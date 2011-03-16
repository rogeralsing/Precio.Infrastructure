using System;
using System.Linq;

namespace Precio.Domain
{
    public static partial class UoW
    {
        public static Action SubscribeToDomainEvents = () => { };

        public static Func<BaseUoW> UoWFactory;

        [ThreadStatic] private static BaseUoW current;

        private static BaseUoW Current
        {
            get { return current; }
        }

        public static void Delete<T>(T aggregateRoot) where T : class
        {
            Current.Remove(aggregateRoot);
        }

        //Expression<Func<T,object>> path
        public static IQueryable<T> Query<T>() where T : class
        {
            return Current.Query<T>();
        }

        public static void Add<T>(T aggregateRoot) where T : class
        {
            Current.Add(aggregateRoot);
        }

        public static void Commit()
        {
            Current.Commit();
        }

        public static UoWScope CreateOrContinue()
        {
            if (Current == null)
                return Create();

            return new UoWExistingScope();
        }

        public static UoWScope Create()
        {
            if (Current != null)
                throw new NotSupportedException("Can not nest unit of works");

            DomainEvents.Init();
            SubscribeToDomainEvents();
            current = UoWFactory();

            return new UoWNewScope();
        }

        private static void End()
        {
            if (Current == null)
                throw new NotSupportedException("No active unit of work");

            current = null;
        }
    }
}