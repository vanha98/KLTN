using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class DeTaiNghienCuu
    {
        public DeTaiNghienCuu()
        {
            BaoCaoTienDo = new HashSet<BaoCaoTienDo>();
            BaiPost = new HashSet<BaiPost>();
            XetDuyetVaDanhGia = new HashSet<XetDuyetVaDanhGia>();
            NhomSinhVien = new HashSet<NhomSinhVien>();
        }

        public long Id { get; set; }
        public string TenDeTai { get; set; }
        public string MoTa { get; set; }
        public string TenTep { get; set; }
        public string TepDinhKem { get; set; }
        public long? IdgiangVien { get; set; }
        //public int? Idnhom { get; set; }
        public DateTime? NgayLap { get; set; }
        public DateTime? NgayThucHien { get; set; }
        public bool? Loai { get; set; }
        public int? TinhTrangDangKy { get; set; }
        public int? TinhTrangPheDuyet { get; set; }

        public virtual GiangVien IdgiangVienNavigation { get; set; }
        //public virtual Nhom IdnhomNavigation { get; set; }
        public virtual ICollection<BaoCaoTienDo> BaoCaoTienDo { get; set; }
        public virtual ICollection<BaiPost> BaiPost { get; set; }
        public virtual ICollection<XetDuyetVaDanhGia> XetDuyetVaDanhGia { get; set; }
        public virtual ICollection<NhomSinhVien> NhomSinhVien { get; set; }
    }
}
