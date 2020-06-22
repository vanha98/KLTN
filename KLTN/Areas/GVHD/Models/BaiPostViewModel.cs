using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Areas.GVHD.Models
{
    public class BaiPostViewModel
    {
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public int Loai { get; set; }
        public long IdDeTaiNghienCuu { get; set; }
        public long MaDeTai { get; set; }
        public int[] currentImg { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
