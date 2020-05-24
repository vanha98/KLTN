using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Views.Shared.Components.HienThiNoiDungThaoLuan
{
    public class HienThiNoiDungThaoLuanViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            Models.Views views = new Models.Views();
            return View(views);
        }
    }
}
