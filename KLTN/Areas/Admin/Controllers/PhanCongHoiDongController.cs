using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhanCongHoiDongController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}