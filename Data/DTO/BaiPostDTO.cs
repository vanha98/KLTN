using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class BaiPostDTO
    {
        public int Id { get; set; }
        public long IdnguoiTao { get; set; }
        public int? IdKenhThaoLuan { get; set; }
        public long? IdDeTai { get; set; }
        public string NgayPost { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public int? Loai { get; set; }
        public int? Status { get; set; }
    }
}
