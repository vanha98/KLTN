using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    [Authorize(Roles ="SinhVien")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //var model = _db.SinhVien.FirstOrDefault(x => x.Mssv == MSSV);
            return View();
        }
    }
}