namespace AllSopFoodService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(FoodDBContext context) : base(context)
        {
        }

        public IQueryable<Category> GetCategoryDataWithEagerLoad(int id) => this.context.Categories.Include(c => c.FoodProducts.Where(p => p.CategoryId == id)).ThenInclude(fp => fp.Category);
    }
}
