using System.Data.Objects;
using System.Linq;

namespace Precio.Domain
{
    public class EFUoW : BaseUoW
    {
        private readonly ObjectContext context;

        public EFUoW(ObjectContext context)
        {
            this.context = context;
        }

        public override void Add<T>(T entity)
        {
            context
                .CreateObjectSet<T>()
                .AddObject(entity);
        }

        public override void Remove<T>(T entity)
        {
            context.DeleteObject(entity);
        }

        public override IQueryable<T> Query<T>()
        {
            return context
                .CreateObjectSet<T>()
                .Extend();
        }

        public override void Commit()
        {
            context.SaveChanges(SaveOptions.DetectChangesBeforeSave);
        }
    }
}