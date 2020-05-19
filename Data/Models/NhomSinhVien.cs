using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class NhomSinhVien
    {
        public int Idnhom { get; set; }
        public long IdsinhVien { get; set; }
        public int? Status { get; set; }

        public virtual Nhom IdnhomNavigation { get; set; }
        public virtual SinhVien IdsinhVienNavigation { get; set; }
    }
}
