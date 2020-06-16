using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Views.Shared.Components.MarkEditDelete
{
    public class MarkEditDeleteViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        public MarkEditDeleteViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(BaiPost model)
        {
            ViewBag.NgayPost = model.NgayPost.Value.ToString("HH:mm dd/MM/yyyy");
            var user =  await _userManager.FindByNameAsync(model.IdnguoiTao.ToString());
            var claim = await _userManager.GetClaimsAsync(user);
            ViewBag.Claim = claim[0].Value;
            ViewBag.IdnguoiTao = model.IdnguoiTao.ToString();
            return View();
        }
    }
}
