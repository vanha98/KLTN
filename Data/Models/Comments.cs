using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class Comments
    {
        public int Id { get; set; }
        public int IdnguoiTao { get; set; }
        public int? IdbaiPost { get; set; }
        public DateTime? NoiDungComment { get; set; }
        public int? Status { get; set; }

        public virtual BaiPost IdbaiPostNavigation { get; set; }
    }
}
