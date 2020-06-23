using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class YCChinhSuaDeTai
    {
        public int Id { get; set; }
        public long IDDeTai { get; set; }
        public string TenDeTai { get; set; }
        public string MoTa { get; set; }
        public string TepDinhKem { get; set; }
        public string TenTep { get; set; }

        public virtual DeTaiNghienCuu IddeTaiNavigation { get; set; }
    }
}
