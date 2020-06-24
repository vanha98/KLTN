using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class YeuCauPheDuyet
    {
        public int Id { get; set; }
        public long IddeTai { get; set; }
        public long IdNguoiDuyet { get; set; }
        public int LoaiYeuCau { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayDuyet { get; set; }
        public int Status { get; set; }

        public virtual DeTaiNghienCuu IddeTaiNghienCuuNavigation { get; set; }
    }
}
