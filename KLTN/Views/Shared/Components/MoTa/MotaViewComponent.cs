using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace KLTN.Views.Shared.Components.MoTa
{
    //[ViewComponent(Name = "MoTa")]
    public class MotaViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
