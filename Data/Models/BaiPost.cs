using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class BaiPost
    {
        public BaiPost()
        {
            Comments = new HashSet<Comments>();
        }

        public int Id { get; set; }
        public int IdnguoiTao { get; set; }
        public int? IdKenhThaoLuan { get; set; }
        public DateTime? NgayPost { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public int? Loai { get; set; }
        public int? Status { get; set; }

        public virtual KenhThaoLuan IdKenhThaoLuanNavigation { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
    }
}
