using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.GVHD.Controllers
{
    [Area("GVHD")]
    [Authorize(Roles = "GVHD")]
    public class XetDuyetDeTaiController : Controller
    {
        private readonly ISinhVien _service;
        public XetDuyetDeTaiController(ISinhVien service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}