using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Areas.GVHD.Models
{
    public class CommentsViewModel
    {
        public int Id { get; set; }
        public int IdnguoiTao { get; set; }
        public int IdbaiPost { get; set; }
        public string NoiDungComment { get; set; }
        public string AnhDinhKem { get; set; }
        public DateTime? NgayPost { get; set; }
        public int? Status { get; set; }
        public IFormFile Files { get; set; }
    }
}
