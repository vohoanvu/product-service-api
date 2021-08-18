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

        public IEnumerable<T> GetAll() => this.context.Set<T>().ToList();

        public T GetById(int id) => this.context.Set<T>().Find(id);

        public void Delete(T entity) => this.context.Set<T>().Remove(entity);

        public void DeleteRange(IEnumerable<T> entities) => this.context.Set<T>().RemoveRange(entities);

        public void Update(T entity) => this.context.Set<T>().Update(entity);
    }
}
