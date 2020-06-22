using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Views.Shared.Components.ChonDeTai
{
    public class ChonDeTaiViewComponent : ViewComponent
    {
        private readonly IDeTaiNghienCuu _service;
        public ChonDeTaiViewComponent(IDeTaiNghienCuu service)
        {
            _service = service;
        }
        public async Task<IViewComponentResult> InvokeAsync(string ma)
        {
            long id;
            long.TryParse(ma,out id);
            IEnumerable<DeTaiNghienCuu> model = await _service.GetAll(x=>x.IdgiangVien == id && x.NhomSinhVien.Any()==true);
            return await Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}
