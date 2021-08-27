namespace AllSopFoodService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly FoodDBContext context;

        public GenericRepository(FoodDBContext context) => this.context = context;

        public void Add(T entity) => this.context.Set<T>().Add(entity);

        public void AddRange(IEnumerable<T> entities) => this.context.Set<T>().AddRange(entities);

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression) => this.context.Set<T>().Where(expression);

        public async Task<IEnumerable<T>> GetAllAsync() => await this.context.Set<T>().ToListAsync().ConfigureAwait(true);

        public T GetById(int id) => this.context.Set<T>().Find(id);

        public void Delete(T entity) => this.context.Set<T>().Remove(entity);

        public void DeleteRange(IEnumerable<T> entities) => this.context.Set<T>().RemoveRange(entities);

        public void Update(T entity) => this.context.Set<T>().Update(entity);

        public async Task<IEnumerable<T>> EntitiesWithEagerLoad(Expression<Func<T, bool>>? filter, string[] children)
        {
            try
            {
                IQueryable<T> query = this.context.Set<T>();
                foreach (var entity in children)
                {
                    query = query.Include(entity);
                }

                if (filter != null)
                {
                    return await query.Where(filter).ToListAsync().ConfigureAwait(true);
                }
                return await query.ToListAsync().ConfigureAwait(true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
