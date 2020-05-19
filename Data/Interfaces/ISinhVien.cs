using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ISinhVien : IRepository<SinhVien>
    {
        Task<Nhom> GetGroupInfo(int idgroup);
        Task<IEnumerable<SinhVien>> GetMemberInfo(int idgroup);
    }
}
