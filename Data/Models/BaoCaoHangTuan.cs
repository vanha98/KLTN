using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class BaoCaoHangTuan
    {
        public int Id { get; set; }
        public int? IddeTai { get; set; }
        public string NoiDung { get; set; }
        public string TienDo { get; set; }
        public string DanhGia { get; set; }
        public DateTime? NgayNop { get; set; }
        public DateTime? HanNop { get; set; }
        public int? Status { get; set; }

        public virtual DeTaiNghienCuu IddeTaiNavigation { get; set; }
    }
}
