using Data.Enum;
using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Views.Shared.Components.GVHD
{
    public class GVHDViewComponent : ViewComponent
    {
        private readonly IGiangVien _service;
        public GVHDViewComponent(IGiangVien service)
        {
            _service = service;
        }
        public async Task<IViewComponentResult> InvokeAsync(long? idGiangVien)
        {
            if(idGiangVien.HasValue)
            {
                ViewBag.Selected = idGiangVien.Value;
            }
            IEnumerable<GiangVien> model = await _service.GetAll(x => x.Status == (int)BaseStatus.Active);
            return await Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}
