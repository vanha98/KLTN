using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression);
        Task<T> GetEntity(Expression<Func<T, bool>> expression);
        Task<T> GetById(long id);
        Task<T> GetById(int id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);

    }
}
