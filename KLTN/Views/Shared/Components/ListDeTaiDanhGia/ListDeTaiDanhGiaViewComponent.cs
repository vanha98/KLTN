using KLTN.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Views.Shared.Components.ListDeTaiDanhGia
{
    public class ListDeTaiDanhGiaViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(TabDotViewModel ViewX)
        {
            //ViewX.tabDot1 = ViewX.ListDeTai.Where(x => x.Loai == (int)BaiPostType.CongKhai);
            //ViewX.tabDot2 = ViewX.ListDeTai.Where(x => x.Loai == (int)BaiPostType.RiengTu);
            return View(ViewX);
        }
    }
}
