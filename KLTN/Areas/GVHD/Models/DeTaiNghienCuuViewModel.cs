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
        public long Id { get; set; }
        [Required(ErrorMessage = "Tên đề tài không được trống")]
        public string TenDeTai { get; set; }
        public string MoTa { get; set; }
        public string NgayLap { get; set; }
        public string TenTep { get; set; }

        public IFormFile Files { get; set; }
    }
}
