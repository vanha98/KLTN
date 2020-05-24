using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Views.Shared.Components.ToggleThongTinSinhVien
{
    public class ToggleThongTinSinhVienViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<SinhVien> listSinhVien)
        {
            return View(listSinhVien);
        }
    }
}
