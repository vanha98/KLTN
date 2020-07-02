using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Areas.GVHD.Models
{
    public class DeTaiNghienCuuViewModel
    {
        public string NgayLap { get; set; }

        public long Id { get; set; }
        [Required(ErrorMessage = "Tên đề tài không được trống")]
        public string TenDeTai { get; set; }
        public long? IdgiangVien { get; set; }
        public string Mssv { get; set; }
        public string MoTa { get; set; }
        public string TepDinhKem { get; set; }
        public bool? Loai { get; set; }
        public string TenTep { get; set; }
        public string HoTenGVHD { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string TinhTrangDangKy { get; set; }
        public string TinhTrangPheDuyet { get; set; }
        public string TinhTrangDeTai { get; set; }

        public IFormFile Files { get; set; }

        
    }
}
