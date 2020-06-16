using Data.Models;
using KLTN.Areas.GVHD.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Views.Shared.Components.ListBinhLuan
{
    public class ListBinhLuanViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        public ListBinhLuanViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(BaiPost model)
        {
            var Comments = model.Comments.ToList();
            List<LoadCommentModel> data = new List<LoadCommentModel>();
            foreach (var item in Comments)
            {
                var user = await _userManager.FindByNameAsync(item.IdnguoiTao.ToString());
                var claim = await _userManager.GetClaimsAsync(user);
                LoadCommentModel loadCommentModel = new LoadCommentModel
                {
                    NoiDungComment = item.NoiDungComment,
                    NgayPost = item.NgayPost.Value.ToString("HH:mm dd/MM/yyyy"),
                    NguoiComment = claim[0].Value,
                    AnhDinhKem = item.AnhDinhKem
                };
                data.Add(loadCommentModel);
            }
            data.Reverse();
            return await Task.FromResult<IViewComponentResult>(View(data));
        }
    }
}
