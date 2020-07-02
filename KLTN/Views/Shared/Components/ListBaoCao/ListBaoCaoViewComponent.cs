using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KLTN.Views.Shared.Components.ListBaoCao
{
    public class ListBaoCaoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<BaoCaoTienDo> list)
        {
            return View(list);
        }
    }
}
