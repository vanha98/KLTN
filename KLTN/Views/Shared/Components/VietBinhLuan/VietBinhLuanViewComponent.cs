﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Views.Shared.Components.VietBinhLuan
{
    public class VietBinhLuanViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int ViewX)
        {
            var view = new Models.Views { ViewType = ViewX };
            return View(view);
        }
    }
}
