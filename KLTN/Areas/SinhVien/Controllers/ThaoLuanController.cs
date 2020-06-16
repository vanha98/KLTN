using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.DTO;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using KLTN.Areas.GVHD.Models;
using KLTN.Authorization;
using KLTN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    [Authorize(Roles ="SinhVien")]
    public class ThaoLuanController : Controller
    {
        private readonly IBaiPost _serviceBaiPost;
        private readonly IDeTaiNghienCuu _serviceDeTai;
        private readonly IKenhThaoLuan _serviceKenhThaoLuan;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly INhomSinhVien _serviceNhomSV;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        public ThaoLuanController(IBaiPost serviceBaiPost, IKenhThaoLuan serviceKenhThaoLuan,
            IDeTaiNghienCuu serviceDeTai, INhomSinhVien serviceNhomSV, UserManager<AppUser> userManager,
            IHostingEnvironment hostingEnvironment, IAuthorizationService authorizationService)
        {
            _serviceBaiPost = serviceBaiPost;
            _serviceDeTai = serviceDeTai;
            _serviceKenhThaoLuan = serviceKenhThaoLuan;
            _serviceNhomSV = serviceNhomSV;
            _hostingEnvironment = hostingEnvironment;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var listNhom = await _serviceNhomSV.GetAll(x => x.IdsinhVien == long.Parse(User.Identity.Name));
            NhomSinhVien nhom = listNhom.SingleOrDefault(x => x.IdnhomNavigation.Status == (int)BaseStatus.Active);
            IEnumerable<BaiPost> baiPosts = await _serviceBaiPost.GetAll(x =>x.IdkenhThaoLuanNavigation.IddeTai == nhom.IddeTai && x.Status.Value == (int)BaseStatus.Active);
            ViewBag.DeTai = nhom.IddeTaiNavigation;
            ViewBag.ListDeTai = listNhom.Select(x => x.IddeTaiNavigation);
            return View(baiPosts);
        }
        
        public async Task<IActionResult> SearchBaiPost(string searchString, long id)
        {
            IEnumerable<BaiPost> baiPosts = await _serviceBaiPost.GetAll(x => x.IdkenhThaoLuanNavigation.IddeTai == id && x.Status.Value == (int)BaseStatus.Active);
            return ViewComponent("ListBaiPosts", baiPosts.Where(x=>x.TieuDe.Contains(searchString) && x.Loai == (int)BaiPostType.RiengTu));
        }

        public async Task<IActionResult> ChangeDeTai(long id)
        {
            IEnumerable<BaiPost> baiPosts = await _serviceBaiPost.GetAll(x => x.IdkenhThaoLuanNavigation.IddeTai == id && x.Status.Value == (int)BaseStatus.Active);
            return ViewComponent("ListBaiPosts", baiPosts);
        }

        public async Task<IActionResult> RefreshList(long id)
        {
            IEnumerable<BaiPost> baiPosts = await _serviceBaiPost.GetAll(x => x.IdkenhThaoLuanNavigation.IddeTai == id && x.Status.Value == (int)BaseStatus.Active);
            return ViewComponent("ListBaiPosts", baiPosts);
        }

        public async Task<IActionResult> NoiDungBaiPost(int id)
        {
            BaiPost baiPost = await _serviceBaiPost.GetById(id);
            return PartialView("_NoiDung", baiPost);
        }

        [NonAction]
        public async Task<bool> UpLoadFile(List<IFormFile> files, BaiPost model)
        {
            if (files == null) return true;
            string[] permittedExtensions = { ".png", ".jpeg", ".jpg" };
            string uniqueFileName = null;
            foreach (var file in files)
            {
                ImgBaiPost img = new ImgBaiPost();
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                {
                    return false;
                }
                string UploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img/ThaoLuan");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(UploadsFolder, uniqueFileName);
                await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
                ImgBaiPost imgBaiPost = new ImgBaiPost
                {
                    IdbaiPost = model.Id,
                    TenAnh = file.FileName,
                    KichThuoc = FileSizeFormatter.FormatSize(file.Length),
                    AnhDinhKem = uniqueFileName
                };
                model.ImgBaiPost.Add(imgBaiPost);
            }
            return true;
        }

        [NonAction]
        public bool DeleteImg(int[] idImg, BaiPost model)
        {
            foreach (int i in idImg)
            {
                ImgBaiPost imgBaiPost = model.ImgBaiPost.SingleOrDefault(x => x.Id == i);
                if (imgBaiPost == null)
                {
                    return false;
                }
                model.ImgBaiPost.Remove(imgBaiPost);
            }
            return true;
        }

        [HttpPost]
        public async Task<IActionResult> Create(BaiPostViewModel vmodel)
        {
            string tempNgay = DateTime.Now.ToString("dd/MM/yyyy");
            BaiPost entity = new BaiPost();
            entity.TieuDe = vmodel.TieuDe;
            entity.NoiDung = vmodel.NoiDung;
            entity.Loai = (int)BaiPostType.RiengTu;
            entity.IdnguoiTao = long.Parse(User.Identity.Name);
            entity.NgayPost = DateTime.Now;
            KenhThaoLuan kenhThaoLuan = await _serviceKenhThaoLuan.GetEntity(x => x.IddeTai == vmodel.IdDeTaiNghienCuu);
            if (kenhThaoLuan == null)
            {
                return Ok(new
                {
                    status = false,
                    mess = MessageResult.Fail
                });
            }
            entity.IdkenhThaoLuan = kenhThaoLuan.Id;
            entity.Status = (int)BaseStatus.Active;
            await _serviceBaiPost.Add(entity);
            if (await UpLoadFile(vmodel.Files, entity))
            {
                await _serviceBaiPost.Update(entity);
                return Ok(new
                {
                    status = true,
                    data = entity,
                    temp = tempNgay,
                    mess = MessageResult.CreateSuccess
                });
            }
            else
                return Ok(new
                {
                    status = false,
                    mess = MessageResult.UpLoadFileFail
                });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BaiPostViewModel data)
        {
            if (data != null)
            {
                var BaiPost = await _serviceBaiPost.GetById(data.Id);
                var isAuthorize = await _authorizationService.AuthorizeAsync(User, BaiPost, ThaoLuanOperation.Update);
                if (!isAuthorize.Succeeded)
                    return Ok(new
                    {
                        status = false,
                        mess = MessageResult.AccessDenied
                    });
                BaiPost.TieuDe = data.TieuDe;
                BaiPost.NoiDung = data.NoiDung;
                if(!DeleteImg(data.currentImg, BaiPost))
                {
                    return Ok(new
                    {
                        status = false,
                        mess = MessageResult.Fail
                    });
                }
                if (await UpLoadFile(data.Files, BaiPost))
                {
                    await _serviceBaiPost.Update(BaiPost);
                    return Ok(new
                    {
                        status = true,
                        mess = MessageResult.UpdateSuccess
                    });
                }
                else
                    return Ok(new
                    {
                        status = false,
                        mess = MessageResult.UpLoadFileFail
                    });
            }
            else
                return Ok(new
                {
                    status = false,
                    mess = MessageResult.Fail
                });
        }
        public async Task<IActionResult> XoaBaiPost(int id)
        {
            BaiPost baiPost = await _serviceBaiPost.GetById(id);
            var isAuthorize = await _authorizationService.AuthorizeAsync(User, baiPost, ThaoLuanOperation.Delete);
            if (!isAuthorize.Succeeded)
                return Ok(new
                {
                    status = false,
                    mess = MessageResult.AccessDenied
                });
            if (baiPost != null)
            {
                baiPost.Status = (int)BaseStatus.Disable;
                await _serviceBaiPost.Update(baiPost);
                return Ok(new
                {
                    status = true,
                    mess = MessageResult.UpdateSuccess
                });
            }
            return Ok(new
            {
                status = false,
            });
        }

        [HttpPost]
        public async Task<IActionResult> SendComment(CommentsViewModel data)
        {
            var BaiPost = await _serviceBaiPost.GetById(data.IdbaiPost);
            var files = data.Files;
            string uniqueFileName = null;
            if (files != null)
            {
                string[] permittedExtensions = { ".png", ".jpeg", ".jpg" };
                var ext = Path.GetExtension(files.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                {
                    return Ok(new
                    {
                        status = false,
                        mess = MessageResult.UpLoadFileFail
                    });
                }
                else
                {
                    string UploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img/GVHD/Comments");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + files.FileName;
                    string filePath = Path.Combine(UploadsFolder, uniqueFileName);
                    await files.CopyToAsync(new FileStream(filePath, FileMode.Create));
                }
            }
            Comments cmt = new Comments
            {
                IdnguoiTao = long.Parse(User.Identity.Name),
                NgayPost = DateTime.Now,
                NoiDungComment = data.NoiDungComment,
                AnhDinhKem = uniqueFileName,
                Status = (int)BaseStatus.Active
            };
            BaiPost.Comments.Add(cmt);
            await _serviceBaiPost.Update(BaiPost);

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var role = await _userManager.GetRolesAsync(user);
            LoadCommentModel model = new LoadCommentModel
            {
                NguoiComment = User.FindFirst("Name").Value,
                NgayPost = DateTime.Now.ToString("HH:mm dd/MM/yyyy"),
                NoiDungComment = data.NoiDungComment,
                AnhDinhKem = uniqueFileName,
            };

            return Ok(new { status = true, data = model });
        }

    }
}