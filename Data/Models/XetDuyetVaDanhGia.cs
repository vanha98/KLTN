using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class XetDuyetVaDanhGia
    {
        public XetDuyetVaDanhGia()
        {
            CtxetDuyetVaDanhGia = new HashSet<CtxetDuyetVaDanhGia>();
        }

        public int Id { get; set; }
        public long? IddeTai { get; set; }
        public int? IdhoiDong { get; set; }
        public int? IdmoDot { get; set; }
        public string NoiDung { get; set; }
        public string TenTep { get; set; }
        public string TepDinhKem { get; set; }
        public int? Status { get; set; }

        public virtual DeTaiNghienCuu IddeTaiNavigation { get; set; }
        public virtual HoiDong IdhoiDongNavigation { get; set; }
        public virtual MoDot IdmoDotNavigation { get; set; }
        public virtual ICollection<CtxetDuyetVaDanhGia> CtxetDuyetVaDanhGia { get; set; }
    }
}
