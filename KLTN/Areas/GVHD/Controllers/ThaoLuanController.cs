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

namespace KLTN.Areas.GVHD.Controllers
{
    [Authorize]
    [Area("GVHD")]
    public class ThaoLuanController : Controller
    {
        private readonly IBaiPost _service;
        private readonly IKenhThaoLuan _serviceKenhThaoLuan;
        private readonly IImgBaiPost _serviceimgBaiPost;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        public ThaoLuanController(IBaiPost service, IKenhThaoLuan serviceKenhThaoLuan, IImgBaiPost imgBaiPost, IHostingEnvironment hostingEnvironment, IMapper mapper, IAuthorizationService authorizationService)
        {
            _service = service;
            _serviceimgBaiPost = imgBaiPost;
            _serviceKenhThaoLuan = serviceKenhThaoLuan;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<BaiPost> baiPosts = await _service.GetAll(x => x.Status.Value == (int)BaseStatus.Active);
            return View(baiPosts);
        }

        [NonAction]
        public async Task<bool> UpLoadFile(List<IFormFile> files, BaiPost model, IImgBaiPost service)
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
                    AnhDinhKem = uniqueFileName
                };
                await service.Add(imgBaiPost);
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
            await _service.Add(entity);
            KenhThaoLuan kenhThaoLuan = await _serviceKenhThaoLuan.GetEntity(x=>x.IddeTai == vmodel.IdDeTaiNghienCuu);
            entity.TieuDe = vmodel.TieuDe;
            entity.NoiDung = vmodel.NoiDung;
            entity.Loai = vmodel.Loai;
            entity.IdnguoiTao = long.Parse(User.Identity.Name);
            entity.NgayPost = DateTime.Now;
            if (kenhThaoLuan != null)
            {
                entity.IdkenhThaoLuan = kenhThaoLuan.Id;
            }
            entity.Status = (int)BaseStatus.Active;
            await _service.Update(entity);
            if (await UpLoadFile(vmodel.Files,entity,_serviceimgBaiPost))
            {
                return Ok(new
                {
                    status = true,
                    data = entity,
                    temp = tempNgay,
                    mess = MessageResult.CreateSuccess
                });
            }
            return Ok();
        }

        public List<BaiPostDTO> LoadBaiPost(List<BaiPost> listBaiPost,bool CongKhaiTab)
        {
            List<BaiPostDTO> datas = new List<BaiPostDTO>();
            foreach (BaiPost item in listBaiPost)
            {
                BaiPostDTO bai = new BaiPostDTO();
                bai.Id = item.Id;
                if(CongKhaiTab==false)
                    bai.IdDeTai = item.IdkenhThaoLuanNavigation.IddeTai;
                bai.IdnguoiTao = item.IdnguoiTao;
                bai.TieuDe = item.TieuDe;
                bai.NgayPost = item.NgayPost.Value.ToString("dd/MM/yyyy");

                datas.Add(bai);
            }
            return datas;
        }

        public async Task<JsonResult> SearchBaiPost (string SearchString, bool CongKhaiTab)
        {
            if(!String.IsNullOrEmpty(SearchString))
            {
                List<BaiPost> listBaiPost = new List<BaiPost>();
                if (CongKhaiTab)
                {
                    IEnumerable<BaiPost> result = await _service.GetAll(x => x.TieuDe.Contains(SearchString) && x.Loai == (int)BaiPostType.CongKhai);
                    listBaiPost = result.ToList();
                    
                }
                else
                {
                    IEnumerable<BaiPost> result = await _service.GetAll(x => x.TieuDe.Contains(SearchString) && x.Loai == (int)BaiPostType.RiengTu);
                    listBaiPost = result.ToList();
                }
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
            else
                return Json(new
                {
                    status = false
                });

        }
        public async Task<JsonResult> RefreshList(bool CongKhaiTab)
        {
            List<BaiPost> listBaiPost = new List<BaiPost>();
            if (CongKhaiTab)
            {
                IEnumerable<BaiPost> result = await _service.GetAll(x => x.Loai == (int)BaiPostType.CongKhai);
                listBaiPost = result.ToList();
            }
            else
            {
                IEnumerable<BaiPost> result = await _service.GetAll(x => x.Loai == (int)BaiPostType.RiengTu);
                listBaiPost = result.ToList();
            }

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
        public async Task<JsonResult> NoiDungBaiPost(int idbaipost)
        {
                BaiPost baiPost = await _service.GetById(idbaipost);
                return Json(new
                {
                    status = true,
                    data = baiPost
                });
        }
            
    }
}