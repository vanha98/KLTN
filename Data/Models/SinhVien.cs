using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class SinhVien
    {
        public int Id { get; set; }
        public string Ho { get; set; }
        public string Ten { get; set; }
        public int? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }
        public string Email { get; set; }
        public int? Idnhom { get; set; }
        public int? Status { get; set; }

        public virtual NhomSv IdnhomNavigation { get; set; }
    }
}
