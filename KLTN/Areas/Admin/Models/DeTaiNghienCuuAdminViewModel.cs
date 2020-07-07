using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Areas.Admin.Models
{
    public class DeTaiNghienCuuAdminViewModel
    {
        public MoDot DotDangKyHienTai { get; set; }
        public IEnumerable<DeTaiNghienCuu> listDeTaiHienTai { get; set; }
        public IEnumerable<DeTaiNghienCuu> listDeTaiDeXuatHienTai { get; set; }
        public List<SelectListItem> listGiangVien { get; set; }
    }
}
