using System.Linq;
using Precio.Domain;

namespace CodeBaseBlog.DomainModel
{
    public static class Categories
    {
        public static Category GetById(int categoryId)
        {
            return UoW
                .Query<Category>()
                .FirstOrDefault(c => c.Id == categoryId);
        }


        public static void Add(Category category)
        {
            UoW.Add(category);
        }

        public static void Remove(Category category)
        {
            UoW.Delete(category);
        }
    }


    public partial class Category
    {
    }
}