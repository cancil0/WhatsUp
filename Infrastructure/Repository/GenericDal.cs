using Infrastructure.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class GenericDal<T> : IGenericDal<T> where T : class
    {
        protected Context context;

        public DbSet<T> DbSet => context.Set<T>();

        public GenericDal(Context context)
        {
            this.context = context;
        }

        public T Get(params object[] keyValues)
        {
            return DbSet.Find(keyValues);
        }
        public async Task<T> GetAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await DbSet.FindAsync(keyValues, cancellationToken);
        }

        public void Insert(T item)
        {
            DbSet.Add(item);
        }
        public async void InsertAsync(T item, CancellationToken cancellationToken)
        {
            await DbSet.AddAsync(item, cancellationToken);
        }

        public void InsertBulk(List<T> item)
        {
            DbSet.AddRange(item);
        }
        public async void InsertBulkAsync(IEnumerable<T> item, CancellationToken cancellationToken)
        {
            await DbSet.AddRangeAsync(item, cancellationToken);
        }

        public void Update(T item)
        {
            DbSet.Update(item);
        }
        public void UpdateBulk(List<T> item)
        {
            DbSet.UpdateRange(item);
        }

        public void Delete(T item)
        {
            DbSet.Remove(item);
        }
        public void DeleteBulk(List<T> item)
        {
            DbSet.RemoveRange(item);
        }

        public void Commit()
        {
            if (context != null)
            {
                context.SaveChanges();
            }
        }

        /*public T Get(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            bool disableTracking)
        {

            IQueryable<T> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Gets the first or default entity based on a predicate, orderby delegate and include delegate. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{T}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public async Task<T> GetAsync(Expression<Func<T, T>> selector, 
                                      Expression<Func<T, bool>> predicate,
                                      Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
                                      Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
                                      bool disableTracking, CancellationToken cancellationToken)
        {

            IQueryable<T> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).Select(selector).FirstOrDefaultAsync(cancellationToken);
            }

            return await query.Select(selector).FirstOrDefaultAsync(cancellationToken);
        }
        public List<T> GetMany(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            bool disableTracking)
        {

            IQueryable<T> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.ToList();
        }
        public async Task<List<T>> GetManyAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            bool disableTracking, CancellationToken cancellationToken)
        {

            IQueryable<T> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync(cancellationToken);
        }*/
    }
}
