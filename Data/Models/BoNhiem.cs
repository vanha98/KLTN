using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class BoNhiem
    {
        public int Id { get; set; }
        public int? IdgiangVien { get; set; }
        public int? IdquanLy { get; set; }
        public int? IdhoiDong { get; set; }
        public int? VaiTro { get; set; }
        public DateTime? NgayBoNhiem { get; set; }
        public int? Status { get; set; }

        public virtual GiangVien IdgiangVienNavigation { get; set; }
        public virtual HoiDong IdhoiDongNavigation { get; set; }
        public virtual QuanLy IdquanLyNavigation { get; set; }
    }
}
