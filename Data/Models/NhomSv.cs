using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class NhomSv
    {
        public NhomSv()
        {
            DeTaiNghienCuu = new HashSet<DeTaiNghienCuu>();
            SinhVien = new HashSet<SinhVien>();
        }

        public int Id { get; set; }
        public string TenNhom { get; set; }
        public int? Nam { get; set; }
        public int? IdnamHoc { get; set; }
        public int? Status { get; set; }

        public virtual NamHoc IdnamHocNavigation { get; set; }
        public virtual ICollection<DeTaiNghienCuu> DeTaiNghienCuu { get; set; }
        public virtual ICollection<SinhVien> SinhVien { get; set; }
    }
}
