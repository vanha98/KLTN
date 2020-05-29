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
            IEnumerable<DeTaiNghienCuu> model = await _service.GetAll();
            return View(model.OrderBy(x=>x.TinhTrangPheDuyet));
        }

        [NonAction]
        public async Task<bool> UpLoadFile(IFormFile file, DeTaiNghienCuu model)
        {
            if (file == null) return true;
            string[] permittedExtensions = { ".txt", ".pdf",".doc",".docx",".xlsx", ".xls" };

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
                    //IdgiangVien = 
                    NgayLap = DateTime.Now,
                    TinhTrangDangKy = (int)StatusDangKyDeTai.Con,
                    TinhTrangPheDuyet = (int)StatusPheDuyetDeTai.ChuaGui,
                    Loai = LoaiDeTai.CoSan
                };
                if (await UpLoadFile(vmodel.Files, model) == false)
                {
                    return Json(new { status = false, mess = MessageResult.UpLoadFileFail });
                }
                try
                {
                    await _service.Add(model);
                    return Json(new { status = true, create=true, data = new { NgayLap = DateTime.Now.ToString("dd/MM/yyyy"), Id = vmodel.Id, TenTep = model.TenTep }, mess = MessageResult.CreateSuccess });
                }
                catch
                {
                    return Json(new { status = false, mess = MessageResult.Fail });
                }
            

        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateEditPopup");
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
                //if (entity.TepDinhKem != null && entity.TepDinhKem.Length > 0)
                //{   var stream = new MemoryStream(entity.TepDinhKem);
                //    var ext = Path.GetExtension(entity.TenTep).ToLowerInvariant();
                //    model.Files = new FormFile(stream, 0, entity.TepDinhKem.Length, "Files", entity.TenTep);
                //}
            }
            return PartialView("_CreateEditPopup",model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit (DeTaiNghienCuuViewModel model)
        {
            if (ModelState.IsValid)
            {
                DeTaiNghienCuu entity = new DeTaiNghienCuu();
                if (model.Id != 0)
                {
                    entity = await _service.GetById(model.Id);
                    entity.TenDeTai = model.TenDeTai;
                    entity.MoTa = model.MoTa;
                }
                if (await UpLoadFile(model.Files, entity) == false)
                {
                    return Ok(new 
                    { 
                        status = false, 
                        mess = MessageResult.UpLoadFileFail
                    });
                }
                await _service.Update(entity);
                return Ok(new { status = true, mess = MessageResult.UpdateSuccess});
            }
            else { return Ok(new { status = false, mess = MessageResult.Fail }); }
        }

        private Dictionary<string,string> GetMyTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt","text/plain" },
                {".doc","application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".pdf","application/pdf" },
                {".xls", "application/vnd.ms-excel" },
                {".xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"  }
            };
        }

        [HttpGet]
        public async Task<IActionResult> DownLoadFile(long id)
        {
            DeTaiNghienCuu entity = await _service.GetById(id);
            var ext = Path.GetExtension(entity.TenTep).ToLowerInvariant();
            return File(entity.TepDinhKem, GetMyTypes()[ext], entity.TenTep);
        }

        [HttpPost]
        public async Task<IActionResult> GuiDeTai(long[] data)
        {
            if (data.Count() == 0)
                return Ok(new
                {
                    status = false,
                    mess = MessageResult.Fail
                }) ;          
            DeTaiNghienCuu entity = new DeTaiNghienCuu();
            foreach (long item in data)
            {
                entity = await _service.GetById(item);
                entity.TinhTrangPheDuyet = (int)StatusPheDuyetDeTai.DaGui;
            }
            await _service.Update(entity);
            return Ok(new
            {
                status = true,
                mess = MessageResult.UpdateSuccess
            });
        }
    }
}