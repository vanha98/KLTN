using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KLTN.Areas.GVHD.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using KLTN.Models;
using Microsoft.AspNetCore.Identity;
using KLTN.Authorization;

namespace KLTN.Areas.GVHD.Controllers
{
    [Area("GVHD")]
    [Authorize(Roles ="GVHD")]
    public class ThaoLuanController : Controller
    {
        private readonly IBaiPost _service;
        private readonly IDeTaiNghienCuu _serviceDeTai;
        private readonly IImgBaiPost _serviceimgBaiPost;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        public ThaoLuanController(UserManager<AppUser> userManager,IBaiPost service, 
             IImgBaiPost imgBaiPost, IHostingEnvironment hostingEnvironment, 
             IDeTaiNghienCuu serviceDeTai, IMapper mapper, IAuthorizationService authorizationService)
        {
            _serviceDeTai = serviceDeTai;
            _userManager = userManager;
            _service = service;
            _serviceimgBaiPost = imgBaiPost;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<BaiPost> baiPosts = await _service.GetAll(x => x.IdnguoiTao == long.Parse(User.Identity.Name) && x.Status.Value == (int)BaseStatus.Active); ;
            var DeTai = await _serviceDeTai.GetAll(x => x.IdgiangVien == long.Parse(User.Identity.Name)
                                                    && x.TinhTrangPheDuyet != (int)StatusDeTai.ChuaGui
                                                    && x.TinhTrangPheDuyet != (int)StatusDeTai.DaGui);
            foreach(var item in DeTai)
            {
                var temp = await _service.GetAll(x =>x.IddeTaiNghienCuu == item.Id && x.IdnguoiTao != long.Parse(User.Identity.Name) && x.Status.Value == (int)BaseStatus.Active);
                baiPosts = baiPosts.Concat(temp);
            }
            
            return View(baiPosts.OrderByDescending(x=>x.NgayPost));
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
                ImgBaiPost imgBaiPost = new ImgBaiPost {
                    IdbaiPost = model.Id,
                    TenAnh = file.FileName,
                    KichThuoc = FileSizeFormatter.FormatSize(file.Length),
                    AnhDinhKem = uniqueFileName
                };
                model.ImgBaiPost.Add(imgBaiPost);
            }
            return true;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreatePopup");
        }

        [HttpPost]
        public async Task<IActionResult> Create(BaiPostViewModel vmodel)
        {
            BaiPost entity = new BaiPost();
            string tempNgay = DateTime.Now.ToString("dd/MM/yyyy");
            entity.TieuDe = vmodel.TieuDe;
            if(vmodel.Loai == (int)BaiPostType.CongKhai)
            {
                entity.IddeTaiNghienCuu = DefaultValue.IddeTaiNghienCuu;
            }
            else
                entity.IddeTaiNghienCuu = vmodel.IdDeTaiNghienCuu;
            entity.NoiDung = vmodel.NoiDung;
            entity.Loai = vmodel.Loai;
            entity.IdnguoiTao = long.Parse(User.Identity.Name);
            entity.NgayPost = DateTime.Now;
            entity.Status = (int)BaseStatus.Active;
            await _service.Add(entity);
            if (await UpLoadFile(vmodel.Files,entity))
            {
                await _service.Update(entity);
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

        public List<BaiPostDTO> LoadBaiPost(List<BaiPost> listBaiPost,bool CongKhaiTab)
        {
            List<BaiPostDTO> datas = new List<BaiPostDTO>();
            foreach (BaiPost item in listBaiPost)
            {
                BaiPostDTO bai = new BaiPostDTO();
                bai.Id = item.Id;
                if (CongKhaiTab == false)
                    bai.IdDeTai = item.IddeTaiNghienCuu;
                else
                    bai.IdDeTai = DefaultValue.IddeTaiNghienCuu;
                bai.IdnguoiTao = item.IdnguoiTao;
                bai.TieuDe = item.TieuDe;
                bai.NgayPost = item.NgayPost.Value.ToString("dd/MM/yyyy");

                datas.Add(bai);
            }
            return datas;
        }

        public async Task<List<BaiPost>> LoadData(IBaiPost _service, string SearchString, bool CongKhaiTab)
        {
            List<BaiPost> listBaiPost = new List<BaiPost>();
            if (!String.IsNullOrEmpty(SearchString))
            {
                if (CongKhaiTab)
                {
                    IEnumerable<BaiPost> result = await _service.GetAll(x => x.TieuDe.ToLower().Contains(SearchString.ToLower()) 
                                                                        && x.IdnguoiTao == long.Parse(User.Identity.Name) 
                                                                        && x.Loai == (int)BaiPostType.CongKhai 
                                                                        && x.Status == (int)BaseStatus.Active);
                    listBaiPost = result.ToList();

                }
                else
                {
                    var DeTai = await _serviceDeTai.GetAll(x => x.IdgiangVien == long.Parse(User.Identity.Name)
                                                            && x.TinhTrangPheDuyet != (int)StatusDeTai.ChuaGui
                                                            && x.TinhTrangPheDuyet != (int)StatusDeTai.DaGui);
                    foreach (var item in DeTai)
                    {
                        var temp = await _service.GetAll(x => x.IddeTaiNghienCuu == item.Id 
                                                        && x.TieuDe.ToLower().Contains(SearchString.ToLower())
                                                        && x.IdnguoiTao != long.Parse(User.Identity.Name) 
                                                        && x.Status.Value == (int)BaseStatus.Active);
                        listBaiPost = listBaiPost.Concat(temp).ToList();
                    }
                }
            }
            else
            {
                if (CongKhaiTab)
                {
                    IEnumerable<BaiPost> result = await _service.GetAll(x => x.Loai == (int)BaiPostType.CongKhai && x.Status == (int)BaseStatus.Active);
                    listBaiPost = result.ToList();
                }
                else
                {
                    IEnumerable<BaiPost> result = await _service.GetAll(x => x.Loai == (int)BaiPostType.RiengTu && x.Status == (int)BaseStatus.Active);
                    listBaiPost = result.ToList();
                }
            }
            return listBaiPost;
        }

        public async Task<JsonResult> SearchBaiPost (string SearchString, bool CongKhaiTab)
        {
            List<BaiPost> listBaiPost = await LoadData(_service, SearchString, CongKhaiTab);
            if (listBaiPost.Any() && listBaiPost != null)
            {
                var datas = LoadBaiPost(listBaiPost,CongKhaiTab);
                return Json(new
                {
                    status = true,
                    loai = CongKhaiTab,
                    data = datas
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        public async Task<JsonResult> RefreshList(bool CongKhaiTab)
        {
            List<BaiPost> listBaiPost = await LoadData(_service, null, CongKhaiTab);
            if (listBaiPost.Any() && listBaiPost != null)
            {
                var datas = LoadBaiPost(listBaiPost, CongKhaiTab);
                return Json(new
                {
                    status = true,
                    loai = CongKhaiTab,
                    data = datas
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }


        public async Task<IActionResult> NoiDungBaiPost(int id)
        {
            BaiPost baiPost = await _service.GetById(id);
            return PartialView("_NoiDung", baiPost);
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
        public async Task<IActionResult> Edit(BaiPostViewModel data)
        {
            if(data != null)
            {
                var BaiPost = await _service.GetById(data.Id);
                var isAuthorize = await _authorizationService.AuthorizeAsync(User, BaiPost, ThaoLuanOperation.Update);
                if (!isAuthorize.Succeeded)
                    return Ok(new
                    {
                        status = false,
                        mess = MessageResult.AccessDenied
                    });
                BaiPost.TieuDe = data.TieuDe;
                BaiPost.NoiDung = data.NoiDung;
                if (!DeleteImg(data.currentImg, BaiPost))
                {
                    return Ok(new
                    {
                        status = false,
                        mess = MessageResult.Fail
                    });
                }
                if (await UpLoadFile(data.Files,BaiPost))
                {
                    await _service.Update(BaiPost);
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

        [HttpPost]
        public async Task<IActionResult> XoaBaiPost(int id)
        {
            BaiPost baiPost = await _service.GetById(id);
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
                await _service.Update(baiPost);
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

        //[HttpGet]
        //public async Task<IActionResult> LoadComments(int id)
        //{
        //    BaiPost BaiPost = await _service.GetEntity(x=>x.Id == id);
        //    var Comments = BaiPost.Comments.ToList();
        //    List<LoadCommentModel> data = new List<LoadCommentModel>();
        //    foreach(var item in Comments)
        //    {
        //        var user = await _userManager.FindByNameAsync(item.IdnguoiTao.ToString());
        //        var claim = await _userManager.GetClaimsAsync(user);
        //        LoadCommentModel model = new LoadCommentModel {
        //            NoiDungComment = item.NoiDungComment,
        //            NgayPost = item.NgayPost.Value.ToString("HH:mm dd/MM/yyyy"),
        //            NguoiComment = claim[0].Value,
        //            AnhDinhKem = item.AnhDinhKem
        //        };
        //        data.Add(model);
        //    }
        //    return Json(data);
        //}

        [HttpPost]
        public async Task<IActionResult> SendComment(CommentsViewModel data)
        {
            var BaiPost = await _service.GetById(data.IdbaiPost);
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
                    NoiDungComment=data.NoiDungComment,
                    AnhDinhKem = uniqueFileName,
                    Status = (int)BaseStatus.Active
                };
            BaiPost.Comments.Add(cmt);
            await _service.Update(BaiPost);

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