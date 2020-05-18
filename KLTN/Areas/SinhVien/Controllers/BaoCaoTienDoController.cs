using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class BaoCaoTienDoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}