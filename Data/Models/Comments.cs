using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class Comments
    {
        public int Id { get; set; }
        public long IdnguoiTao { get; set; }
        public int IdbaiPost { get; set; }
        public string NoiDungComment { get; set; }
        public string AnhDinhKem { get; set; }
        public DateTime? NgayPost { get; set; }
        public int? Status { get; set; }

        public virtual BaiPost IdbaiPostNavigation { get; set; }
    }
}
