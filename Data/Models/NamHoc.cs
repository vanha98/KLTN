using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class NamHoc
    {
        public NamHoc()
        {
            MoDot = new HashSet<MoDot>();
            NhomSv = new HashSet<NhomSv>();
        }

        public int Id { get; set; }
        public string HocKy { get; set; }
        public int? Nam { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<MoDot> MoDot { get; set; }
        public virtual ICollection<NhomSv> NhomSv { get; set; }
    }
}
