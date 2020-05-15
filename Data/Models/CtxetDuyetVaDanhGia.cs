using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class CtxetDuyetVaDanhGia
    {
        public int Id { get; set; }
        public int? IdxetDuyet { get; set; }
        public int IdnguoiTao { get; set; }
        public double? Diem { get; set; }
        public string NhanXet { get; set; }
        public string CauHoi { get; set; }
        public string CauTraLoi { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Status { get; set; }

        public virtual XetDuyetVaDanhGia IdxetDuyetNavigation { get; set; }
    }
}
