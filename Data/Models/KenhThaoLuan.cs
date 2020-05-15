using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class KenhThaoLuan
    {
        public KenhThaoLuan()
        {
            CtkenhThaoLuan = new HashSet<CtkenhThaoLuan>();
        }

        public int Id { get; set; }
        public int? IdgiangVien { get; set; }
        public int? Status { get; set; }

        public virtual GiangVien IdgiangVienNavigation { get; set; }
        public virtual ICollection<CtkenhThaoLuan> CtkenhThaoLuan { get; set; }
    }
}
