using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class BaiPostDTO
    {
        public int Id { get; set; }
        public int IdnguoiTao { get; set; }
        public int? IdctkenhThaoLuan { get; set; }
        public string NgayPost { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public int? Loai { get; set; }
        public int? Status { get; set; }
    }
}
