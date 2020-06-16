using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Views.Shared.Components.HienThiNoiDungThaoLuan
{
    public class HienThiNoiDungThaoLuanViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(BaiPost model)
        {
            List<ImgBaiPost> listImg = model.ImgBaiPost.Reverse().ToList();
            ViewBag.listImg = listImg;
            return View(model);
        }
    }
}
