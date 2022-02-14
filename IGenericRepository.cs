using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.aadviktech.IMS.Contract.Repository_Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> SelectAll();
        IQueryable<T> Select(Func<T, bool> predicate);
        T SelectByID(object id);
        T Insert(T obj);
        void Update(T obj);
        void Reload(ref T obj);
        void Delete(object id);
        void Save();
        bool Exist(Func<T, bool> predicate);
    }
}
