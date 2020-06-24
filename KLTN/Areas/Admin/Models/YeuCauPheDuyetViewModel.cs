using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Areas.Admin.Models
{
    public class YeuCauPheDuyetViewModel
    {
        public int Id { get; set; }
        public long IddeTai { get; set; }
        public string TenDeTai { get; set; }
        public string MoTa { get; set; }
        public string TenTep { get; set; }
        public string TepDinhKem { get; set; }
        public string TenGiangVien { get; set; }
        public string IdNguoiDuyet { get; set; }
        public string LoaiYeuCau { get; set; }
        public string NgayTao { get; set; }
        public string NgayDuyet { get; set; }
        public int Status { get; set; }
    }
}
