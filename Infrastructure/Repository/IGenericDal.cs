using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public interface IGenericDal<T> where T : class
    {
        public DbSet<T> DbSet { get; }
        //Get
        /*T Get(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = false);
        Task<T> GetAsync(Expression<Func<T, T>> selector, Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool disableTracking = false, CancellationToken cancellationToken = default);

        List<T> GetMany(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = false);

        Task<List<T>> GetManyAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = false, CancellationToken cancellationToken = default);*/

        T Get(params object[] keyValues);
        Task<T> GetAsync(CancellationToken cancellationToken = default, params object[] keyValues);

        //Insert
        void Insert(T item);
        void InsertAsync(T item, CancellationToken cancellationToken = default);
        void InsertBulk(List<T> item);
        void InsertBulkAsync(IEnumerable<T> item, CancellationToken cancellationToken = default);

        //Delete
        void Delete(T item);
        void DeleteBulk(List<T> item);

        //Update
        void Update(T item);
        void UpdateBulk(List<T> item);

        void Commit();
    }
}
