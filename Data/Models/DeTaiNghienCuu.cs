using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class DeTaiNghienCuu
    {
        public DeTaiNghienCuu()
        {
            BaoCaoTienDo = new HashSet<BaoCaoTienDo>();
            CtkenhThaoLuan = new HashSet<CtkenhThaoLuan>();
            XetDuyetVaDanhGia = new HashSet<XetDuyetVaDanhGia>();
        }

        public long Id { get; set; }
        public string TenDeTai { get; set; }
        public string MoTa { get; set; }
        public string TenTep { get; set; }
        public byte[] TepDinhKem { get; set; }
        public long? IdgiangVien { get; set; }
        public int? Idnhom { get; set; }
        public DateTime? NgayLap { get; set; }
        public bool? Loai { get; set; }
        public int? TinhTrangDangKy { get; set; }
        public int? TinhTrangPheDuyet { get; set; }

        public virtual GiangVien IdgiangVienNavigation { get; set; }
        public virtual Nhom IdnhomNavigation { get; set; }
        public virtual ICollection<BaoCaoTienDo> BaoCaoTienDo { get; set; }
        public virtual ICollection<CtkenhThaoLuan> CtkenhThaoLuan { get; set; }
        public virtual ICollection<XetDuyetVaDanhGia> XetDuyetVaDanhGia { get; set; }
    }
}
