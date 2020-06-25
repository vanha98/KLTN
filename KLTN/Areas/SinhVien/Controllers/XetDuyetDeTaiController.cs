using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class XetDuyetDeTaiController : Controller
    {
        private readonly IMoDot _serviceMoDot;
        public XetDuyetDeTaiController(IMoDot serviceMoDot)
        {
            _serviceMoDot = serviceMoDot;
        }
        public async Task<IActionResult> Index()
        {
            MoDot moDot = await _serviceMoDot.GetEntity(x => x.Status == (int)MoDotStatus.Mo && x.Loai == (int)MoDotLoai.XetDuyetDeTai);
            if (moDot != null)
            {
                if (DateTime.Now >= moDot.ThoiGianBd && DateTime.Now <= moDot.ThoiGianKt)
                {
                    double thoigian = (moDot.ThoiGianKt - DateTime.Now).Value.TotalSeconds;
                    HttpContext.Response.Headers.Add("refresh", "" + thoigian + "; url=" + Url.Action("Index"));
                    ViewBag.DangMoDot = true;
                }
                else
                {
                    ViewBag.DangMoDot = false;
                }
            }
            else
            {
                ViewBag.DangMoDot = false;
            }
            return View();
        }
    }
}