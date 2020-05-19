using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class NamHoc
    {
        public NamHoc()
        {
            MoDot = new HashSet<MoDot>();
            Nhom = new HashSet<Nhom>();
        }

        public int Id { get; set; }
        public string HocKy { get; set; }
        public int? Nam { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<MoDot> MoDot { get; set; }
        public virtual ICollection<Nhom> Nhom { get; set; }
    }
}
