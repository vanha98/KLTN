using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class BaiPost
    {
        public BaiPost()
        {
            Comments = new HashSet<Comments>();
            ImgBaiPost = new HashSet<ImgBaiPost>();
        }

        public int Id { get; set; }
        public long IdnguoiTao { get; set; }
        public DateTime? NgayPost { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public int? Loai { get; set; }
        public int? Status { get; set; }
        public int? IdkenhThaoLuan { get; set; }

        public virtual KenhThaoLuan IdkenhThaoLuanNavigation { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<ImgBaiPost> ImgBaiPost { get; set; }
    }
}
