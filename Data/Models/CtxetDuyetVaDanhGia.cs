using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class CtxetDuyetVaDanhGia
    {
        public int Id { get; set; }
        public int? IdxetDuyet { get; set; }
        public long IdgiangVien { get; set; }
        public int VaiTro { get; set; }
        public double? Diem { get; set; }
        public string NhanXet { get; set; }
        public string CauHoi { get; set; }
        public string TenTepCauHoi { get; set; }
        public string TepDinhKemCauHoi { get; set; }
        public string CauTraLoi { get; set; }
        public string TenTepCauTraLoi { get; set; }
        public string TepDinhKemCauTraLoi { get; set; }
        public DateTime? NgayTaoCauHoi { get; set; }
        public DateTime? NgayDanhGia { get; set; }
        public int? Status { get; set; }

        public virtual XetDuyetVaDanhGia IdxetDuyetNavigation { get; set; }
        public virtual GiangVien IdgiangVienNavigation { get; set; }
    }
}
