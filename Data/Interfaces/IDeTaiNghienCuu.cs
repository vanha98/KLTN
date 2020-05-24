using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IDeTaiNghienCuu : IRepository<DeTaiNghienCuu>
    {
        long KhoiTaoMa(string MaHienTai);
    }
}
