using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class DeTaiNghienCuu
    {
        public DeTaiNghienCuu()
        {
            BaoCaoHangTuan = new HashSet<BaoCaoHangTuan>();
            CtkenhThaoLuan = new HashSet<CtkenhThaoLuan>();
            XetDuyetVaDanhGia = new HashSet<XetDuyetVaDanhGia>();
        }

        public int Id { get; set; }
        public string TenDeTai { get; set; }
        public string MoTa { get; set; }
        public int? IdgiangVien { get; set; }
        public int? Idnhom { get; set; }
        public DateTime? NgayLap { get; set; }
        public bool? Loai { get; set; }
        public int? Status { get; set; }

        public virtual GiangVien IdgiangVienNavigation { get; set; }
        public virtual NhomSv IdnhomNavigation { get; set; }
        public virtual ICollection<BaoCaoHangTuan> BaoCaoHangTuan { get; set; }
        public virtual ICollection<CtkenhThaoLuan> CtkenhThaoLuan { get; set; }
        public virtual ICollection<XetDuyetVaDanhGia> XetDuyetVaDanhGia { get; set; }
    }
}
