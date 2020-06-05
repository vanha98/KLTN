using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class GiangVien
    {
        public GiangVien()
        {
            BoNhiem = new HashSet<BoNhiem>();
            DeTaiNghienCuu = new HashSet<DeTaiNghienCuu>();
            KenhThaoLuan = new HashSet<KenhThaoLuan>();
        }

        public long Id { get; set; }
        public string Ho { get; set; }
        public string Ten { get; set; }
        public int? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }
        public string Email { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<BoNhiem> BoNhiem { get; set; }
        public virtual ICollection<DeTaiNghienCuu> DeTaiNghienCuu { get; set; }
        public virtual ICollection<KenhThaoLuan> KenhThaoLuan { get; set; }
    }
}
