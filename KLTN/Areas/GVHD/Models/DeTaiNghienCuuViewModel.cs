using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Areas.GVHD.Models
{
    public class DeTaiNghienCuuViewModel
    {
        public string TenDeTai { get; set; }
        public string MoTa { get; set; }
        public IFormFile Files { get; set; }
    }
}
