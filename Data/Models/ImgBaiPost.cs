using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class ImgBaiPost
    {
        public int Id { get; set; }
        public int? IdbaiPost { get; set; }
        public string TenAnh { get; set; }
        public string KichThuoc { get; set; }
        public string AnhDinhKem { get; set; }

        public virtual BaiPost IdbaiPostNavigation { get; set; }
    }
}
