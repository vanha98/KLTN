using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //var model = _db.SinhVien.FirstOrDefault(x => x.Mssv == MSSV);
            return View();
        }
    }
}