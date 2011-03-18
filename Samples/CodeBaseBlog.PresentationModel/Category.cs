using System.Collections.Generic;
using System.Linq;
using CodeBaseBlog.DomainModel;
using Precio.Domain;

namespace CodeBaseBlog.PresentationModel
{
    public static class CategoryProjections
    {
        public static IList<FlattenedCategoryProjection> GetAll()
        {
            return UoW
                .Query<Category>()
                .AsProjection()
                .ToList();
        }

        private static IQueryable<FlattenedCategoryProjection> AsProjection(this IQueryable<Category> source)
        {
            return source
                .Select(c => new FlattenedCategoryProjection
                                 {
                                     Id = c.Id,
                                     Name = c.Name,
                                     PostCount = c.Posts.Count()
                                 });
        }
    }

    public class CategoryProjection
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class FlattenedCategoryProjection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PostCount { get; set; }
    }
}