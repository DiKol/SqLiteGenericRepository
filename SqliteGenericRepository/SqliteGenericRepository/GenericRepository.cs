using SQLite;
using System.Linq.Expressions;

namespace SalesAppApi.Data
{

    /// <summary>
    /// Generic Repository for SQLLite
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        private readonly SQLiteAsyncConnection connection;

        public GenericRepository(string databaseName)
        {
            connection = new SQLiteAsyncConnection(databaseName, SQLiteOpenFlags.ReadOnly, false);
            connection.CreateTableAsync<T>();
        }

        public AsyncTableQuery<T> AsQueryable() => connection.Table<T>();

        public async Task<int> Insert(T entity) => await connection.InsertAsync(entity);

        public async Task<int> Update(T entity) => await connection.UpdateAsync(entity);

        public async Task<int> Delete(T entity) => await connection.DeleteAsync(entity);

        public async Task<T> Get(int id) => await connection.FindAsync<T>(id);

        public async Task<List<T>> Get() => await connection.Table<T>().ToListAsync();

        public async Task<T> Get(Expression<Func<T, bool>> predicate) => await connection.FindAsync<T>(predicate);

        public async Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            var query = connection.Table<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy<TValue>(orderBy);

            return await query.ToListAsync();
        }
    }
}
