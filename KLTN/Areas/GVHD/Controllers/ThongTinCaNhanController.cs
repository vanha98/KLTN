using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.GVHD.Controllers
{
    [Area("GVHD")]
    [Authorize(Roles = "GVHD")]
    public class ThongTinCaNhanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}