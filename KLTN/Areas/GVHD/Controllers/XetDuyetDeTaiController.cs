using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using KLTN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.GVHD.Controllers
{
    [Area("GVHD")]
    [Authorize(Roles = "GVHD")]
    public class XetDuyetDeTaiController : Controller
    {
        private readonly IBoNhiem _service;
        private readonly KLTNContext _context;
        public XetDuyetDeTaiController(IBoNhiem service, KLTNContext context)
        {
            _service = service;
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<DeTaiNghienCuu> listDetaiXetDuyet = from t0 in _context.DeTaiNghienCuu
                                    join t1 in _context.XetDuyetVaDanhGia on t0.Id equals t1.IddeTai
                                    join t2 in _context.BoNhiem on t1.IdhoiDong equals t2.IdhoiDong
                                    where t2.IdgiangVien == long.Parse(User.Identity.Name)
                                    select t0;

            IEnumerable<DeTaiNghienCuu> tabDot1 = listDetaiXetDuyet.Where(x => x.TinhTrangPheDuyet == (int)StatusDeTai.DaDangKy);
            IEnumerable<DeTaiNghienCuu> tabDot2 = listDetaiXetDuyet.Where(x => x.TinhTrangPheDuyet == (int)StatusDeTai.DanhGiaLai);

            TabDotViewModel viewx = new TabDotViewModel();
            viewx.ListDeTaiDuocPhanCong = listDetaiXetDuyet;


            return View();
        }
    }
}