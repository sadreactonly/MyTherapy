using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SQLite;
using PCLStorage;

namespace MyTherapy
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private SQLiteConnection db;

        public Repository()
		{

		}
        public Repository(string dbName)
        {
            this.db = GetConnection(dbName);
            this.db.CreateTable<T>();
        }

        public SQLiteConnection GetConnection(string sqliteFilename)
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            string path = PortablePath.Combine(folder.Path, sqliteFilename);
            var sqLiteConnection = new SQLiteConnection(path);
            return sqLiteConnection;
        }

        public TableQuery<T> AsQueryable() =>
            db.Table<T>();

        public List<T> Get() => db.Table<T>().ToList();

        public  List<T> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            var query = db.Table<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy(orderBy);

            return  query.ToList();
        }

        public  T Get(int id) => db.Find<T>(id);

        public T Get(Expression<Func<T, bool>> predicate) => db.Table<T>().Where(predicate).FirstOrDefault();

        public int Insert(T entity) =>  db.Insert(entity);

        public int Update(T entity) => db.Update(entity);

        public int Delete(T entity) => db.Delete(entity);

        public int InsertAll(List<T> entities) => db.InsertAll(entities);
    }
}