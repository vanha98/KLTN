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
        public int? IdctkenhThaoLuan { get; set; }
        public DateTime? NgayPost { get; set; }
        public string TieuDe { get; set; }
        public DateTime? NoiDung { get; set; }
        public int? Loai { get; set; }
        public int? Status { get; set; }

        public virtual CtkenhThaoLuan IdctkenhThaoLuanNavigation { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
    }
}
