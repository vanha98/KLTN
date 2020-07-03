using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using KLTN.Areas.SinhVien.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class XetDuyetDeTaiController : Controller
    {
        private readonly IMoDot _serviceMoDot;
        private readonly IctXetDuyetDanhGia _serviceCT;
        private readonly IXetDuyetDanhGia _serviceXDDG;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly KLTNContext _context;
        public XetDuyetDeTaiController(IMoDot serviceMoDot, IXetDuyetDanhGia serviceXDDG,
            IHostingEnvironment hostingEnvironment, IctXetDuyetDanhGia serviceCT, KLTNContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _serviceCT = serviceCT;
            _serviceXDDG = serviceXDDG;
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
            if(DetaiXetDuyet != null)
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
            
            MoDot moDot = await _serviceMoDot.GetEntity(x => x.Status == (int)MoDotStatus.Mo && x.Loai == (int)MoDotLoai.XetDuyetDeTai);
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

        public async Task<IActionResult> LoadCauHoi(int id)
        {
            var ct = await _serviceCT.GetById(id);
            return PartialView("_TraLoiCauHoi", ct);
        }

        public async Task<IActionResult> LoadBaoCao(int id)
        {
            var XDDG = await _serviceXDDG.GetById(id);
            return PartialView("_NopBaoCao", XDDG);
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

            string UploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FileUpload/CauTraLoiXetDuyetDanhGia");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(UploadsFolder, uniqueFileName);
            await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            model.TepDinhKemCauTraLoi = uniqueFileName;
            model.TenTepCauTraLoi = file.FileName;
            return true;
        }

        [NonAction]
        public async Task<bool> UpLoadFileXDDG(IFormFile file, XetDuyetVaDanhGia model)
        {
            if (file == null) return true;
            string[] permittedExtensions = { ".txt", ".pdf", ".doc", ".docx", ".xlsx", ".xls" };

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }

            string UploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FileUpload/CauTraLoiXetDuyetDanhGia");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(UploadsFolder, uniqueFileName);
            await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            model.TepDinhKem = uniqueFileName;
            model.TenTep = file.FileName;
            return true;
        }

        public async Task<IActionResult> TraLoi(CauTraLoi model)
        {
            var ct = await _serviceCT.GetById(model.IdCT);
            ct.CauTraLoi = model.TraLoi;
            if(await UpLoadFile(model.File,ct))
            {
                await _serviceCT.Update(ct);
                return Ok(new
                {
                    status = true,
                    mess = MessageResult.UpdateSuccess
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

        public async Task<IActionResult> BaoCao(BaoCaoXDDGViewModel model)
        {
            var XDDG = await _serviceXDDG.GetById(model.IdXDDG);
            XDDG.NoiDung = model.NoiDung;
            if (await UpLoadFileXDDG(model.File, XDDG))
            {
                await _serviceXDDG.Update(XDDG);
                return Ok(new
                {
                    status = true,
                    mess = MessageResult.UpdateSuccess
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
    }
}