namespace AllSopFoodService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Model;

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal readonly FoodDbContext context;

        public GenericRepository(FoodDbContext context) => this.context = context;

        public void Add(T entity) => this.context.Set<T>().Add(entity);

        public void AddRange(IEnumerable<T> entities) => this.context.Set<T>().AddRange(entities);

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression) => this.context.Set<T>().Where(expression);

        public async Task<IEnumerable<T>> GetAllAsync() => await this.context.Set<T>().ToListAsync().ConfigureAwait(true);

        public T GetById(int id) => this.context.Set<T>().Find(id);

        public void Delete(T entity) => this.context.Set<T>().Remove(entity);

        public void DeleteRange(IEnumerable<T> entities) => this.context.Set<T>().RemoveRange(entities);

        public void Update(T entity) => this.context.Set<T>().Update(entity);

        public async Task<IEnumerable<T>> EntitiesWithEagerLoadAsync(Expression<Func<T, bool>>? filter, string[] children)
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
    }
}
