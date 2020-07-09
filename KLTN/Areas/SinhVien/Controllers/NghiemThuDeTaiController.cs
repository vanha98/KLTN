using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    [Authorize(Roles = "SinhVien")]
    public class NghiemThuDeTaiController : Controller
    {
        private readonly IMoDot _serviceMoDot;
        private readonly KLTNContext _context;
        public NghiemThuDeTaiController(IMoDot serviceMoDot, KLTNContext context)
        {
            _context = context;
            _serviceMoDot = serviceMoDot;
        }
        public async Task<IActionResult> Index()
        {
            DeTaiNghienCuu DetaiXetDuyet = (from t0 in _context.DeTaiNghienCuu
                                            join t1 in _context.XetDuyetVaDanhGia on t0.Id equals t1.IddeTai
                                            join t2 in _context.NhomSinhVien on t0.Id equals t2.IddeTai
                                            where t2.IdsinhVien == long.Parse(User.Identity.Name) && t2.IdnhomNavigation.Status == 1
                                            select t0).SingleOrDefault();
            List<CtxetDuyetVaDanhGia> ct = new List<CtxetDuyetVaDanhGia>();
            if (DetaiXetDuyet != null)
            {
                ViewBag.TenDeTai = DetaiXetDuyet.TenDeTai;
                var xetDuyetVaDanhGia = DetaiXetDuyet.XetDuyetVaDanhGia.SingleOrDefault(x => x.Status == 1);
                if (xetDuyetVaDanhGia != null)
                {
                    ViewBag.XDDG = xetDuyetVaDanhGia;
                    ct = xetDuyetVaDanhGia.CtxetDuyetVaDanhGia.ToList();
                    double diemtb = 0;
                    int chia = 0;
                    foreach (var item in ct)
                    {
                        if (item.Diem.HasValue)
                        {
                            if (item.VaiTro == (int)LoaiVaiTro.PhanBien)
                            {
                                diemtb = diemtb + (2 * item.Diem.Value);
                                chia = chia + 2;
                            }
                            else
                            {
                                diemtb = diemtb + item.Diem.Value;
                                chia++;
                            }
                        }
                    }
                    if (chia == 0)
                    {
                        ViewBag.DiemTB = 0;
                    }
                    else
                    {
                        ViewBag.DiemTB = diemtb / chia * 1.0;
                    }
                }
            }

            MoDot moDot = await _serviceMoDot.GetEntity(x => x.Status == (int)MoDotStatus.Mo && x.Loai == (int)MoDotLoai.NghiemThuDeTai);
            if (moDot != null)
            {
                ViewBag.Dot = moDot;
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
            return View(ct);
        }
    }
}