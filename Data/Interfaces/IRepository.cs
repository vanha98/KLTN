using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetBy(int id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
