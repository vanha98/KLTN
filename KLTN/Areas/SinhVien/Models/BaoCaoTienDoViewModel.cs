using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Areas.SinhVien.Models
{
    public class BaoCaoTienDoViewModel
    {
        public int Id { get; set; }
        public long IddeTai { get; set; }
        public string NoiDung { get; set; }
        public string TenTep { get; set; }
        public string TepDinhKem { get; set; }
        public string TienDo { get; set; }
        public string DanhGia { get; set; }
        public string NgayNop { get; set; }
        public string HanNop { get; set; }
        public string Status { get; set; }
        public IFormFile File { get; set; }
    }
}
