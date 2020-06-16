using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class KenhThaoLuan
    {
        public KenhThaoLuan()
        {
            BaiPost = new HashSet<BaiPost>();
        }

        public int Id { get; set; }
        //public long? IdgiangVien { get; set; }
        public int? Status { get; set; }
        public long? IddeTai { get; set; }

        public virtual DeTaiNghienCuu IddeTaiNavigation { get; set; }
        //public virtual GiangVien IdgiangVienNavigation { get; set; }
        public virtual ICollection<BaiPost> BaiPost { get; set; }
    }
}
