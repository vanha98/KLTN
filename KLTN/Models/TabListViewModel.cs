using Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Models
{
    public class TabListViewModel
    {
        public int ViewType { get; set; }

        //View Thảo Luận
        public IEnumerable<BaiPost> ListBaiPostThaoLuan { get; set; }
        public IEnumerable<BaiPost> tabCongKhai { get; set; }
        public IEnumerable<BaiPost> tabRiengTu { get; set; }

        //View Xét Duyệt
        public IEnumerable<DeTaiNghienCuu> ListDeTaiXetDuyet { get; set; }
    }
}
