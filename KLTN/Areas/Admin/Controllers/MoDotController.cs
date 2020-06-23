using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data.Interfaces;
using Data.Enum;
using Data.Models;

namespace KLTN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoDotController : Controller
    {
        private readonly IMoDot _service;
        private readonly IDeTaiNghienCuu _serviceDeTai;

        public MoDotController(IMoDot service, IDeTaiNghienCuu serviceDeTai)
        {
            _service = service;
            _serviceDeTai = serviceDeTai;
        }

        public async Task<IActionResult> Index()
        {
            MoDot moDot = await _service.GetEntity(x => x.Status == (int)MoDotStatus.Mo);
            DeTaiNghienCuu detai = await _serviceDeTai.GetEntity(x => x.NgayThucHien != null);
            if(detai != null)
            {
                ViewBag.NgayBdDeTai = detai.NgayThucHien.Value.ToString("yyyy-MM-dd'T'HH:mm:ss");
                ViewBag.NgayKtDeTai = detai.NgayKetThuc.Value.ToString("yyyy-MM-dd'T'HH:mm:ss");
            }

            if(moDot != null)
            {
                if(DateTime.Now > moDot.ThoiGianKt.Value)
                {
                    moDot.Status = (int)MoDotStatus.Dong;
                    await _service.Update(moDot);
                }
            }

            return View(moDot);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MoDot moDot)
        {
            moDot.Status = 1;
            await _service.Add(moDot);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MoDot moDot)
        {
            
            await _service.Update(moDot);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            MoDot moDot = await _service.GetById(id);
            await _service.Delete(moDot);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTGThucHienDeTai(DateTime NgayBdDeTai, DateTime NgayKtDeTai)
        {
            IEnumerable<DeTaiNghienCuu> deTaiNghienCuus = await _serviceDeTai.GetAll();
            if(deTaiNghienCuus.Any())
            {
                foreach (var item in deTaiNghienCuus)
                {
                    item.NgayThucHien = NgayBdDeTai;
                    item.NgayKetThuc = NgayKtDeTai;
                    await _serviceDeTai.Update(item);
                }
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditTGThucHienDeTai(DateTime NgayBdDeTai, DateTime NgayKtDeTai)
        {
            IEnumerable<DeTaiNghienCuu> deTaiNghienCuus = await _serviceDeTai.GetAll();
            if (deTaiNghienCuus.Any())
            {
                foreach (var item in deTaiNghienCuus)
                {
                    item.NgayThucHien = NgayBdDeTai;
                    item.NgayKetThuc = NgayKtDeTai;
                    await _serviceDeTai.Update(item);
                }
            }

            return RedirectToAction("Index");
        }
    }
}