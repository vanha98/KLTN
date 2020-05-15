using Microsoft.AspNetCore.Mvc;


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
