using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;

namespace KLTN.Models
{
    public class TabDotViewModel
    {
        public IEnumerable<DeTaiNghienCuu> ListDeTaiDuocPhanCong { get; set; }
        public IEnumerable<DeTaiNghienCuu> tabDot1 { get; set; }
        public IEnumerable<DeTaiNghienCuu> tabDot2 { get; set; }
    }
}
