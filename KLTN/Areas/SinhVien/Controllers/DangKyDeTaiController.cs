using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data.Bo;
using Data.Interfaces;

namespace KLTN.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class DangKyDeTaiController : Controller
    {
        private readonly DangKyDeTaiBo _service;
        public DangKyDeTaiController (DangKyDeTaiBo service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAll());
        }
    }
}