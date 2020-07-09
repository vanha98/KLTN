using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Areas.Admin.Models
{
    public class ThongTinTaiKhoan
    {
        public string TenTaiKhoan { get; set; }
        public string HoTen { get; set; }
        public string Sdt { get; set; }
        public string Email { get; set; }
        public string VaiTro { get; set; }
        public bool TrangThai { get; set; }
    }
}
