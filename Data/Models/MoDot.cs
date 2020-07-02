using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class MoDot
    {
        public MoDot()
        {
            XetDuyetVaDanhGia = new HashSet<XetDuyetVaDanhGia>();
        }

        public int Id { get; set; }
        public int? IdnamHoc { get; set; }
        public long? IdquanLy { get; set; }
        public DateTime? ThoiGianBd { get; set; }
        public DateTime? ThoiGianKt { get; set; }
        public int? Loai { get; set; }
        public int? DiemToiDa { get; set; }
        public int? DiemToiThieu { get; set; }
        public int? Status { get; set; }

        public virtual NamHoc IdnamHocNavigation { get; set; }
        public virtual QuanLy IdquanLyNavigation { get; set; }
        public virtual ICollection<XetDuyetVaDanhGia> XetDuyetVaDanhGia { get; set; }
    }
}
