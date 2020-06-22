using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QLTaiKhoanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}