using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DeTaiNghienCuuController : Controller
    {
        private readonly IDeTaiNghienCuu _service;
        private readonly IMoDot _serviceMoDot;
        public DeTaiNghienCuuController(IDeTaiNghienCuu service, IMoDot serviceMoDot)
        {
            _service = service;
            _serviceMoDot = serviceMoDot;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<MoDot> listDotDangKy = await _serviceMoDot.GetAll(x => x.Loai == (int)MoDotLoai.DangKy);
            IEnumerable<DeTaiNghienCuu> listDeTai = await _service.GetAll(x => x.TinhTrangDeTai == (int)StatusDeTai.DaDuyet && x.TinhTrangDeTai == (int)StatusDeTai.DaDangKy);
            
            return View();
        }
    }
}