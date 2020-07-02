using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Areas.GVHD.Models
{
    public class ctXetDuyetDanhGiaViewModel
    {
        public int Id { get; set; }
        public long idDeTai { get; set; }
        public int? IdxetDuyet { get; set; }
        public string IdnguoiTao { get; set; }
        public string VaiTro { get; set; }
        public double? Diem { get; set; }
        public string NhanXet { get; set; }
        public string CauHoi { get; set; }
        public string CauTraLoi { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Status { get; set; }
        public IFormFile File { get; set; }
    }
}
