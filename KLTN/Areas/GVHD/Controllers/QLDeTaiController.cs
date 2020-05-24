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

        [HttpPost]
        public async Task<IActionResult> Create(DeTaiNghienCuuViewModel vmodel)
        {
            using (var memoryStream = new MemoryStream())
            {
                await vmodel.Files.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    var model = new DeTaiNghienCuu()
                    {
                        Id = 2020001,
                        TenDeTai = vmodel.TenDeTai,
                        MoTa = vmodel.MoTa,
                        TenTep = vmodel.Files.FileName,
                        NgayLap = DateTime.Now,
                        TepDinhKem = memoryStream.ToArray(),
                        Status = 1
                    };

                    if(await _service.Add(model) == true)
                        return Json(new {status = 1 });
                    else
                        return Json(new { status = 0});
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                    return Json(new { status = 0 });
                }
            }
        }
    }
}