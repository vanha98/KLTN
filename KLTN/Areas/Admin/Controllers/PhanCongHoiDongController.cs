using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhanCongHoiDongController : Controller
    {
        private readonly IHoiDong _serviceHoiDong;
        private readonly IDeTaiNghienCuu _serviceDeTai;
        private readonly IMoDot _serviceMoDot;
        private readonly IXetDuyetDanhGia _serviceXetDuyetDanhGia;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private static int Dot;
        public PhanCongHoiDongController(UserManager<AppUser> userManager, IHoiDong serviceHoiDong,
             IDeTaiNghienCuu serviceDeTai, IMapper mapper, IMoDot serviceMoDot,
             IAuthorizationService authorizationService, IXetDuyetDanhGia serviceXetDuyetDanhGia)
        {
            _serviceXetDuyetDanhGia = serviceXetDuyetDanhGia;
            _serviceDeTai = serviceDeTai;
            _userManager = userManager;
            _serviceMoDot = serviceMoDot;
            _serviceHoiDong = serviceHoiDong;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> Index()
        {
            Dot = 1;
            var moDot = await _serviceMoDot.GetEntity(x => x.Status == 1);
            var hoiDong = await _serviceHoiDong.GetAll(x => x.Status == 1);
            var deTai = await _serviceDeTai.GetAll(x => x.TinhTrangPhanCong == (int)StatusPhanCong.ChuaPhanCong && x.TinhTrangDeTai == (int)StatusDeTai.DaDangKy);
            var allDot = await _serviceMoDot.GetAll();
            ViewBag.DeTai = deTai;
            if (!allDot.Any())
                return View(hoiDong);
            if (moDot != null && moDot.Loai != (int)MoDotLoai.DangKy)
            {
                string temp = "";
                if (moDot.Loai == (int)MoDotLoai.XetDuyetDeTai)
                    temp = "Xét duyệt đề tài ("+moDot.ThoiGianBd.Value.ToString("HH:mm:ss dd/MM/yyyy")+ " - "
                            +moDot.ThoiGianKt.Value.ToString("HH:mm:ss dd/MM/yyyy")+")";
                else
                    temp = "Đánh giá nghiệm thu đề tài (" + moDot.ThoiGianBd.Value.ToString("HH:mm:ss dd/MM/yyyy") + " - "
                            + moDot.ThoiGianKt.Value.ToString("HH:mm:ss dd/MM/yyyy") + ")";
                ViewBag.MoDot = temp;
                ViewBag.IdMoDot = moDot.Id;
            }
            if (allDot.Count() > 1 && allDot.ToList()[allDot.Count() - 2].Loai == moDot.Loai)
            {
                Dot = 2;
                deTai = await _serviceDeTai.GetAll(x => x.TinhTrangPhanCong == (int)StatusPhanCong.ChuaPhanCong && x.TinhTrangDeTai == (int)StatusDeTai.DanhGiaLai);
                ViewBag.DeTai = deTai;
            }
            
            return View(hoiDong.OrderBy(x=>x.StatusPhanCong));
        }

        public async Task<IActionResult> LoadThanhVien(int id)
        {
            HoiDong hoiDong = await _serviceHoiDong.GetById(id);
            return PartialView("_LoadThanhVien", hoiDong.BoNhiem);
        }

        public async Task<List<HoiDong>> LoadData(IHoiDong _service, string SearchString)
        {
            List<HoiDong> list = new List<HoiDong>();
            if (!String.IsNullOrEmpty(SearchString))
            {
                var result = await _service.GetAll(x => x.TenHoiDong.ToLower().Contains(SearchString.ToLower()) && x.Status == 1);
                list = result.ToList();
            }
            else
            {
                var result = await _service.GetAll(x => x.Status==1);
                list = result.ToList();
            }
            return list.OrderBy(x=>x.StatusPhanCong).ToList();
        }
        public async Task<JsonResult> SearchHoiDong(string SearchString)
        {
            List<HoiDong> list = await LoadData(_serviceHoiDong, SearchString);
            if (list.Any())
            {
                return Json(new
                {
                    status = true,
                    data = list
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        public async Task<JsonResult> RefreshList()
        {
            List<HoiDong> list = await LoadData(_serviceHoiDong, null);
            if (list.Any())
            {
                return Json(new
                {
                    status = true,
                    data = list
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        public async Task<IActionResult> DeTaiDaPhanCong(int idHoiDong)
        {
            var hoiDong = await _serviceHoiDong.GetById(idHoiDong);
            var list = hoiDong.XetDuyetVaDanhGia.Where(x=>x.Status == 1);
            var deTai = await _serviceDeTai.GetAll(x => x.TinhTrangPhanCong == (int)StatusPhanCong.ChuaPhanCong && x.TinhTrangDeTai == (int)StatusDeTai.DaDangKy);
            if (Dot == 2)
                deTai = await _serviceDeTai.GetAll(x => x.TinhTrangPhanCong == (int)StatusPhanCong.ChuaPhanCong && x.TinhTrangDeTai == (int)StatusDeTai.DanhGiaLai);
            List<DeTaiNghienCuu> datas = new List<DeTaiNghienCuu>();
            if (list.Any())
            {
                foreach (var item in list)
                {
                    datas.Add(item.IddeTaiNavigation);
                }
                datas = datas.Concat(deTai).ToList();
                return PartialView("_PhanCongHoiDong", datas);
            }
            else
            {
                return PartialView("_PhanCongHoiDong", deTai);
            }

        }

        public async Task<IActionResult> PhanCong(int idHoiDong, int idMoDot, long[] idsDeTai)
        {
            var hoiDong = await _serviceHoiDong.GetById(idHoiDong);
            var XDDG = hoiDong.XetDuyetVaDanhGia.Where(x => x.Status == 1).ToList();
            for (int i = 0; i<XDDG.Count();i++)
            {
                XDDG[i].IddeTaiNavigation.TinhTrangDeTai = (int)StatusDeTai.DaDangKy;
                if (Dot == 2)
                    XDDG[i].IddeTaiNavigation.TinhTrangDeTai = (int)StatusDeTai.DanhGiaLai;
                XDDG[i].IddeTaiNavigation.TinhTrangPhanCong = (int)StatusPhanCong.ChuaPhanCong;
                await _serviceXetDuyetDanhGia.Delete(XDDG[i]);
            }
            if(idsDeTai.Length == 0)
            {
                hoiDong.StatusPhanCong = 0; //chưa phân công
                await _serviceHoiDong.Update(hoiDong);
                return Ok(new
                {
                    status = true,
                    mess = MessageResult.UpdateSuccess
                });
            }
            for (int i = 0; i < idsDeTai.Length; i++)
            {
                var deTai = await _serviceDeTai.GetById(idsDeTai[i]);
                deTai.TinhTrangPhanCong = (int)StatusPhanCong.DaPhanCong;
                XetDuyetVaDanhGia entity = new XetDuyetVaDanhGia
                {
                    IddeTai = idsDeTai[i],
                    IdmoDot = idMoDot,
                };
                hoiDong.XetDuyetVaDanhGia.Add(entity);
            }
            hoiDong.StatusPhanCong = 1; //đã phân công
            await _serviceHoiDong.Update(hoiDong);
            return Ok(new
            {
                status = true,
                mess = MessageResult.UpdateSuccess
            });
        }
    }
}