using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Areas.SinhVien.Models
{
    public class CauTraLoi
    {
        public int IdCT { get; set; }
        public string TraLoi { get; set; }
        public IFormFile File { get; set; }
    }
}
