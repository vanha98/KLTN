using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Areas.SinhVien.Models
{
    public class BaoCaoXDDGViewModel
    {
        public int IdXDDG { get; set; }
        public string NoiDung { get; set; }
        public IFormFile File { get; set; }
    }
}
