using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Areas.Admin.Models
{
    public class DeTaiNghienCuuAdminViewModel
    {
        public IEnumerable<DeTaiNghienCuu> listDeTaiHienTai { get; set; }
        public IEnumerable<DeTaiNghienCuu> listDeTaiDeXuatHienTai { get; set; }
    }
}
