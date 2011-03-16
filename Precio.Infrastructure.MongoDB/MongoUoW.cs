using System;
using System.Collections.Generic;
using System.Linq;
using Norm;

namespace Precio.Domain
{
    public class MongoUoW : BaseUoW
    {
        private readonly List<Action> actions = new List<Action>();
        private readonly IMongo mongo;

        public MongoUoW(IMongo mongo)
        {
            this.mongo = mongo;
        }

        public override void Add<T>(T entity)
        {
            Action a = () => mongo.GetCollection<T>().Insert(entity);
            actions.Add(a);
        }

        public override void Remove<T>(T entity)
        {
            Action a = () => mongo.GetCollection<T>().Delete(entity);
            actions.Add(a);
        }

        public override IQueryable<T> Query<T>()
        {
            return mongo.GetCollection<T>().AsQueryable();
        }

        public override void Commit()
        {
            foreach (Action action in actions)
            {
                action();
            }
            actions.Clear();
        }
    }
}