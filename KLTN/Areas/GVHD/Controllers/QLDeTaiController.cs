using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using KLTN.Areas.GVHD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.GVHD.Controllers
{
    [Area("GVHD")]
    public class QLDeTaiController : Controller
    {
        private readonly IDeTaiNghienCuu _service;
        public QLDeTaiController(IDeTaiNghienCuu service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAll());
        }

        [NonAction]
        public async Task<bool> UpLoadFile(IFormFile file, DeTaiNghienCuu model)
        {
            if (file == null) return true;
            string[] permittedExtensions = { ".txt", ".pdf",".doc",".docx",".xlsx" };

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    model.TenTep = file.FileName;
                    model.TepDinhKem = memoryStream.ToArray();
                    return true;
                }
                else return false;
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create(DeTaiNghienCuuViewModel vmodel)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<DeTaiNghienCuu> list = await _service.GetAll();
                if (list.Count() != 0)
                {
                    DeTaiNghienCuu LastE = list.OrderBy(x=>x.Id).LastOrDefault();
                    vmodel.Id = _service.KhoiTaoMa(LastE);
                }
                else
                    vmodel.Id = long.Parse(DateTime.Now.Year.ToString() + "001");
                var model = new DeTaiNghienCuu()
                {
                    Id = vmodel.Id,
                    TenDeTai = vmodel.TenDeTai,
                    MoTa = vmodel.MoTa,
                    NgayLap = DateTime.Now,
                    TinhTrangDangKy = (int)StatusDangKyDeTai.Con,
                    TinhTrangPheDuyet = (int)StatusPheDuyetDeTai.ChuaGui,
                    Loai = LoaiDeTai.CoSan
                };
                if (await UpLoadFile(vmodel.Files, model) == false)
                {
                    return Json(new { status = 0, mess = "Tệp sai định dạng hoặc kích thước quá lớn" });
                }
                try
                {
                    await _service.Add(model);
                    return Json(new { status = 1, data = new { NgayLap = DateTime.Now.ToString("dd/MM/yyyy"), Id = vmodel.Id, TenTep = model.TenTep }, mess = "Thêm thành công" });
                }
                catch
                {
                    return Json(new { status = 0, mess = "Thêm thất bại" });
                }
            }
            else
            {
                return this.View();
            }

        }

        [HttpGet]
        public async Task<ActionResult> Edit (long? id)
        {
            DeTaiNghienCuuViewModel model = new DeTaiNghienCuuViewModel();
            if (id.HasValue)
            {
                var entity = await _service.GetById(id.Value);
                model.Id = entity.Id;
                model.TenDeTai = entity.TenDeTai;
                model.MoTa = entity.MoTa;
                model.TenTep = entity.TenTep;
            }
            return PartialView("_CreateEditPopup",model);
        }
    }
}