namespace AllSopFoodService.Repositories.Interfaces
{
    using System.Linq;
    using Model;

    public interface ICategoryRepository : IGenericRepository<Category>
    {
        IQueryable<Category> GetCategoryDataWithEagerLoad(int id);
    }
}
