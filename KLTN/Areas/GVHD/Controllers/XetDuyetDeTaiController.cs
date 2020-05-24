using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.GVHD.Controllers
{
    [Area("GVHD")]
    public class XetDuyetDeTaiController : Controller
    {
        private readonly ISinhVien _service;
        public XetDuyetDeTaiController(ISinhVien service)
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