namespace AllSopFoodService.Repositories
{
    using System.Linq;
    using Model;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(FoodDbContext context) : base(context)
        {
        }

        public IQueryable<Category> GetCategoryDataWithEagerLoad(int id) => this.context.Categories.Include(c => c.FoodProducts.Where(p => p.CategoryId == id)).ThenInclude(fp => fp.Category);
    }
}
