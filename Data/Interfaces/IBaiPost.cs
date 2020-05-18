using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBaiPost : IRepository<BaiPost>
    {
        Task<IEnumerable<Comments>> GetAllComments(int idbaipost);
    }
}
