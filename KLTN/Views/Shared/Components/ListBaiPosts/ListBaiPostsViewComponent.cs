using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Views.Shared.Components.ListBaiPosts
{
    public class ListBaiPostsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<BaiPost> listbaipost)
        {
            return View(listbaipost);
        }
    }
}
