using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using KLTN.Areas.GVHD.Models;
using KLTN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.GVHD.Controllers
{
    [Area("GVHD")]
    [Authorize(Roles = "GVHD")]
    public class XetDuyetDeTaiController : Controller
    {
        private readonly IBoNhiem _service;
        private readonly IMoDot _serviceMoDot;
        private readonly INhomSinhVien _serviceNhomSV;
        private readonly IDeTaiNghienCuu _serviceDeTai;
        private readonly IctXetDuyetDanhGia _serviceCT;
        private readonly KLTNContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private static MoDot DotHienTai;
        public XetDuyetDeTaiController(IBoNhiem service, INhomSinhVien serviceNhomSV,IMoDot serviceMoDot, IctXetDuyetDanhGia serviceCT, IHostingEnvironment hostingEnvironment, IDeTaiNghienCuu serviceDeTai, KLTNContext context)
        {
            _serviceNhomSV = serviceNhomSV;
            _serviceCT = serviceCT;
            _hostingEnvironment = hostingEnvironment;
            _service = service;
            _serviceDeTai = serviceDeTai;
            _serviceMoDot = serviceMoDot;
            _context = context;
        }
        [NonAction]
        public List<TinhTrangXDDG> LoadList()
        {
            List<DeTaiNghienCuu> listDetaiXetDuyet = (from t0 in _context.DeTaiNghienCuu
                                                      join t1 in _context.XetDuyetVaDanhGia on t0.Id equals t1.IddeTai 
                                                      join t2 in _context.BoNhiem on t1.IdhoiDong equals t2.IdhoiDong
                                                      where t2.IdgiangVien == long.Parse(User.Identity.Name) 
                                                            && t2.Status == 1
                                                      select t0).ToList();
            List<CtxetDuyetVaDanhGia> listCT = (from t0 in _context.CtxetDuyetVaDanhGia
                                                where t0.IdgiangVien == long.Parse(User.Identity.Name) && t0.Diem > 0 && t0.Status == 1
                                                select t0).ToList();
            List<TinhTrangXDDG> data = new List<TinhTrangXDDG>();
            for (int i = 0; i < listDetaiXetDuyet.Count(); i++)
            {
                TinhTrangXDDG obj = new TinhTrangXDDG();
                obj.IdDeTai = listDetaiXetDuyet[i].Id;
                obj.TenDeTai = listDetaiXetDuyet[i].TenDeTai;
                obj.TinhTrangDeTai = listDetaiXetDuyet[i].TinhTrangDeTai.Value;
                obj.TinhTrang = "Chưa đánh giá";
                if (listCT.Where(x => x.IdxetDuyetNavigation.IddeTai == listDetaiXetDuyet.ToList()[i].Id).Any())
                {
                    obj.TinhTrang = "Đã đánh giá";
                }
                data.Add(obj);
            }
            return data;
        }

        public async Task<IActionResult> Index()
        {
            var allDot = await _serviceMoDot.GetAll();
            if (!allDot.Any())
                return View();
            DotHienTai = await _serviceMoDot.GetEntity(x => x.Status == (int)MoDotStatus.Mo && x.Loai == (int)MoDotLoai.XetDuyetDeTai);
            if (DotHienTai == null)
                return View();
            ViewBag.MoDot = DotHienTai;
            ViewBag.Dot = 1;
            if(allDot.Count() > 1 && allDot.ToList()[allDot.Count()-2].Loai == DotHienTai.Loai)
            {
                ViewBag.Dot = 2;
            }
                
            List<TinhTrangXDDG> data = LoadList();
            List<TinhTrangXDDG> tabDot1 = data.Where(x => x.TinhTrangDeTai == (int)StatusDeTai.DaDangKy).ToList();
            List<TinhTrangXDDG> tabDot2 = data.Where(x => x.TinhTrangDeTai == (int)StatusDeTai.DanhGiaLai).ToList();

            TabDotViewModel viewx = new TabDotViewModel();
            //viewx.ListDeTaiDuocPhanCong = listDetaiXetDuyet;
            viewx.tabDot1 = tabDot1;
            viewx.tabDot2 = tabDot2;

            return View(viewx);
        }

        [NonAction]
        public List<TinhTrangXDDG> LoadData(string SearchString, bool ActiveTabDot1)
        {
            List<TinhTrangXDDG> data = LoadList();
            if (!String.IsNullOrEmpty(SearchString))
            {
                if (ActiveTabDot1)
                {
                    List<TinhTrangXDDG> tabDot1 = data.Where(x => x.TinhTrangDeTai == (int)StatusDeTai.DaDangKy
                                                            && x.TenDeTai.ToLower().Contains(SearchString.ToLower())).ToList();

                    data = tabDot1;

                }
                else
                {
                    List<TinhTrangXDDG> tabDot2 = data.Where(x => x.TinhTrangDeTai == (int)StatusDeTai.DanhGiaLai
                                                            && x.TenDeTai.ToLower().Contains(SearchString.ToLower())).ToList();
                    data = tabDot2;
                }
            }
            else
            {
                if (ActiveTabDot1)
                {
                    List<TinhTrangXDDG> tabDot1 = data.Where(x => x.TinhTrangDeTai == (int)StatusDeTai.DaDangKy).ToList();
                    data = tabDot1;
                }
                else
                {
                    List<TinhTrangXDDG> tabDot2 = data.Where(x => x.TinhTrangDeTai == (int)StatusDeTai.DanhGiaLai).ToList();
                    data = tabDot2;
                }
            }
            return data;
        }

        public JsonResult SearchBaiPost(string SearchString, bool ActiveTabDot1)
        {
            List<TinhTrangXDDG> list = LoadData(SearchString, ActiveTabDot1);
            if (list.Any() && list != null)
            {
                return Json(new
                {
                    status = true,
                    loai = ActiveTabDot1,
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

        public JsonResult RefreshList(bool ActiveTabDot1)
        {
            List<TinhTrangXDDG> list = LoadData(null, ActiveTabDot1);
            if (list.Any() && list != null)
            {
                return Json(new
                {
                    status = true,
                    loai = ActiveTabDot1,
                    data = list
                });
            }
            else
            {
                return Json(new
                {
                    status = false,
                });
            }
        }

        public async Task<IActionResult> LoadNoiDung(long id)
        {
            var deTai = await _serviceDeTai.GetById(id);
            ViewBag.TenDeTai = deTai.TenDeTai;
            ViewBag.NhomSV = deTai.NhomSinhVien.Select(x => x.IdsinhVienNavigation);
            var xetDuyetVaDanhGia = deTai.XetDuyetVaDanhGia.SingleOrDefault(x => x.Status == 1);
            ViewBag.XDDG = xetDuyetVaDanhGia;
            var ct = xetDuyetVaDanhGia.CtxetDuyetVaDanhGia;
            double diemtb = 0;
            int chia = 0;
            foreach(var item in ct)
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
            if(chia == 0)
            {
                ViewBag.DiemTB = 0;
            }
            else
            {
                ViewBag.DiemTB = diemtb/chia * 1.0;
            }
            ViewBag.ctUSer = ct.SingleOrDefault(x => x.IdgiangVien == long.Parse(User.Identity.Name) && x.Status == 1);
            //foreach()
            return PartialView("_NoiDungXetDuyetGV", ct);
        }

        [NonAction]
        public async Task<bool> UpLoadFile(IFormFile file, CtxetDuyetVaDanhGia model)
        {
            if (file == null) return true;
            string[] permittedExtensions = { ".txt", ".pdf", ".doc", ".docx", ".xlsx", ".xls" };

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }

            string UploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FileUpload/CauHoiXetDuyetDanhGia");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(UploadsFolder, uniqueFileName);
            await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            model.TepDinhKemCauHoi = uniqueFileName;
            model.TenTepCauHoi = file.FileName;
            return true;
        }

        public async Task<IActionResult> DatCauHoi(ctXetDuyetDanhGiaViewModel model)
        {
            var deTai = await _serviceDeTai.GetById(model.idDeTai);
            var xetDuyetVaDanhGia = deTai.XetDuyetVaDanhGia.SingleOrDefault(x => x.Status == 1);
            var checkCT = await _serviceCT.GetEntity(x => x.IdgiangVien == long.Parse(User.Identity.Name) && x.Status == 1
                                                    && x.IdxetDuyet == xetDuyetVaDanhGia.Id);
            if (checkCT != null) // update
            {
                checkCT.CauHoi = model.CauHoi;
                checkCT.NgayTaoCauHoi = DateTime.Now;
                if (await UpLoadFile(model.File, checkCT))
                {
                    if (model.File == null)
                    {
                        checkCT.TenTepCauHoi = "";
                        checkCT.TepDinhKemCauHoi = "";
                    }
                    await _serviceCT.Update(checkCT);
                    return Ok(new
                    {
                        status = true,
                        mess = "Cập nhật câu hỏi thành công"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = false,
                        mess = MessageResult.UpLoadFileFail
                    });
                }

            }
            //create new
            CtxetDuyetVaDanhGia ct = new CtxetDuyetVaDanhGia
            {
                CauHoi = model.CauHoi,
                IdgiangVien = long.Parse(User.Identity.Name),
                NgayTaoCauHoi = DateTime.Now,
                IdxetDuyet = DotHienTai.Id,
            };
            var vaitro = _service.GetEntity(x => x.IdhoiDong == xetDuyetVaDanhGia.IdhoiDong
                                            && x.IdgiangVien == long.Parse(User.Identity.Name)).Result.VaiTro;
            ct.VaiTro = vaitro.Value;
            if (await UpLoadFile(model.File, ct))
            {
                if (model.File == null)
                {
                    ct.TenTepCauHoi = "";
                    ct.TepDinhKemCauHoi = "";
                }
                xetDuyetVaDanhGia.CtxetDuyetVaDanhGia.Add(ct);
                await _serviceDeTai.Update(deTai);
                return Ok(new
                {
                    status = true,
                    mess = "Đặt câu hỏi thành công"
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    mess = MessageResult.UpLoadFileFail
                });
            }
        }

        public async Task<IActionResult> DanhGia (ctXetDuyetDanhGiaViewModel model)
        {
            var deTai = await _serviceDeTai.GetById(model.idDeTai);
            var xetDuyetVaDanhGia = deTai.XetDuyetVaDanhGia.SingleOrDefault(x => x.Status == 1);
            var checkCT = await _serviceCT.GetEntity(x => x.IdgiangVien == long.Parse(User.Identity.Name) && x.Status == 1
                                                    && x.IdxetDuyet == xetDuyetVaDanhGia.Id);
            if (checkCT == null)
            {
                CtxetDuyetVaDanhGia ct = new CtxetDuyetVaDanhGia
                {
                    IdgiangVien = long.Parse(User.Identity.Name),
                    IdxetDuyet = DotHienTai.Id,
                    IdxetDuyetNavigation = xetDuyetVaDanhGia
                };
                var vaitro = _service.GetEntity(x => x.IdhoiDong == xetDuyetVaDanhGia.IdhoiDong
                                            && x.IdgiangVien == long.Parse(User.Identity.Name)).Result.VaiTro;
                ct.VaiTro = vaitro.Value;
                ct.NhanXet = model.NhanXet;
                ct.NgayDanhGia = DateTime.Now;
                ct.Diem = model.Diem;
                await _serviceCT.Add(ct);
                return Ok(new { status = true, mess = "Đánh giá thành công" });
            }
            else
            {
                checkCT.NhanXet = model.NhanXet;
                checkCT.NgayDanhGia = DateTime.Now;
                checkCT.Diem = model.Diem;
                await _serviceCT.Update(checkCT);
                return Ok(new { status = true, mess = MessageResult.UpdateSuccess });
            }
        }

        public async Task<IActionResult> XemNhom(long id)
        {
            var NhomSV = await _serviceNhomSV.GetAll(x => x.IddeTai == id);
            return ViewComponent("ToggleThongTinSinhVien", NhomSV.Select(x => x.IdsinhVienNavigation));
        }

        public async Task<IActionResult> LoadCauTraLoi(int id)
        {
            var ct = await _serviceCT.GetById(id);
            return PartialView("_XemTraLoi", ct);
        }
    }
}