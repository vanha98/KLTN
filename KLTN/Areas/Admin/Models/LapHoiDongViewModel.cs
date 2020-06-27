using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Areas.Admin.Models
{
    public class LapHoiDongViewModel
    {
        public int Id { get; set; }
        public string TenHoiDong { get; set; }
        public List<ThanhVien> ThanhViens { get; set; }
        public int[] DelThanhViens { get; set; }
    }

    public class ThanhVien
    {
        public long IdThanhVien { get; set; }
        public int VaiTro { get; set; }
    }
}
