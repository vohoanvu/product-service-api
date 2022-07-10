namespace AllSopFoodService.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        Task<IEnumerable<T>> GetAllAsync();

        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);

        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

        void Update(T entity);

        Task<IEnumerable<T>> EntitiesWithEagerLoadAsync(Expression<Func<T, bool>>? filter, string[] children);
    }
}
