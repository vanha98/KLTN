using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class CtkenhThaoLuan
    {
        public CtkenhThaoLuan()
        {
            BaiPost = new HashSet<BaiPost>();
        }

        public int Id { get; set; }
        public int? IdkenhThaoLuan { get; set; }
        public long? IddeTai { get; set; }
        public int? Status { get; set; }

        public virtual DeTaiNghienCuu IddeTaiNavigation { get; set; }
        public virtual KenhThaoLuan IdkenhThaoLuanNavigation { get; set; }
        public virtual ICollection<BaiPost> BaiPost { get; set; }
    }
}
