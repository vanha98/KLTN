using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class Nhom
    {
        public Nhom()
        {
            DeTaiNghienCuu = new HashSet<DeTaiNghienCuu>();
            NhomSinhVien = new HashSet<NhomSinhVien>();
        }

        public int Id { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<DeTaiNghienCuu> DeTaiNghienCuu { get; set; }
        public virtual ICollection<NhomSinhVien> NhomSinhVien { get; set; }
    }
}
