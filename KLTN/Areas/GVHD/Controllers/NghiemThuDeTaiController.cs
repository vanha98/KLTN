﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.GVHD.Controllers
{
    public class NghiemThuDeTaiController : Controller
    {
        [Area("GVHD")]
        public IActionResult Index()
        {
            return View();
        }
    }
}