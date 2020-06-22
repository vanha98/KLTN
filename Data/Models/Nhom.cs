using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class Nhom
    {
        public Nhom()
        {
            NhomSinhVien = new HashSet<NhomSinhVien>();
        }

        public int Id { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<NhomSinhVien> NhomSinhVien { get; set; }
    }
}
