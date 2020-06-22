using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace KLTN.Areas.GVHD.Controllers
{
    [Area("GVHD")]
    [Authorize(Roles = "GVHD")]
    public class HomeController : Controller
    {
        private readonly ISinhVien _service;
        public HomeController(ISinhVien service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var listSinhVien = await _service.GetMemberInfo(1);
            return View(listSinhVien);
        }
    }
}