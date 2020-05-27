using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using Data.DTO;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.GVHD.Controllers
{
    [Area("GVHD")]
    public class ThaoLuanController : Controller
    {
        private readonly IBaiPost _service;
        public ThaoLuanController(IBaiPost service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<BaiPost> baiPosts = await _service.GetAll(x => x.Status == 1);
            return View(baiPosts);
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
                    List<BaiPostDTO> datas = new List<BaiPostDTO>();
                    foreach (BaiPost item in listBaiPost)
                    {
                        BaiPostDTO bai = new BaiPostDTO();
                        bai.Id = item.Id;
                        bai.IdctkenhThaoLuan = item.Id;
                        bai.IdnguoiTao = item.IdnguoiTao;
                        bai.TieuDe = item.TieuDe;
                        bai.NgayPost = item.NgayPost.Value.ToString("dd/MM/yyyy");

                        datas.Add(bai);
                    }

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
    }
}