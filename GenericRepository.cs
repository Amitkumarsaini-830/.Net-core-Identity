using com.aadviktech.IMS.Contract.Repository_Interfaces;
using com.aadviktech.IMS.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.aadviktech.IMS.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public MyDBContext db = null;
        public DbSet<T> table = null;

        //public GenericRepository()
        //{
        //    this.db = new MyDBContext();
        //    table = db.Set<T>();
        //}

        public GenericRepository(MyDBContext db)
        {
            this.db = db;
            table = db.Set<T>();
        }

        public IQueryable<T> SelectAll()
        {
            return table;
        }

        public IQueryable<T> Select(Func<T, bool> predicate)
        {
            return table.Where<T>(predicate).AsQueryable();
        }

        public T SelectByID(object id)
        {
            return table.Find(id);
        }

        public T Insert(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("no item");
            }
            table.Add(item);
            return item;
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            db.Entry(obj).State = EntityState.Modified;
        }

        public void Reload(ref T obj)
        {
            try
            {
                db.Entry<T>(obj).Reload();
            }
            catch (Exception ex) { }
        }

        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public bool Exist(Func<T, bool> predicate)
        {
            return table.Where(predicate).Any();
        }
    }
}
