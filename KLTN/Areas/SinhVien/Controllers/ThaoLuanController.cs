using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using KLTN.Areas.GVHD.Models;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class ThaoLuanController : Controller
    {
        private readonly IBaiPost _service;
        public ThaoLuanController(IBaiPost service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<BaiPost> baiPosts = await _service.GetAll(x => x.Status.Value == (int)BaseStatus.Active);
            return View(baiPosts);
        }

        public async Task<JsonResult> NoiDungBaiPost(int idbaipost)
        {
            BaiPost baiPost = await _service.GetById(idbaipost);
            List<ImgBaiPost> listImg = baiPost.ImgBaiPost.ToList();
            return Json(new
            {
                status = true,
                data = baiPost,
                listImg = listImg
            });
        }

        //[HttpPost]
        //public async Task<IActionResult> Edit(BaiPostViewModel data)
        //{
        //    if (data != null)
        //    {
        //        var BaiPost = await _service.GetById(data.Id);
        //        BaiPost.TieuDe = data.TieuDe;
        //        BaiPost.NoiDung = data.NoiDung;
        //        if (await UpLoadFile(data.Files, BaiPost))
        //        {
        //            await _service.Update(BaiPost);
        //            return Ok(new
        //            {
        //                status = true,
        //                mess = MessageResult.UpdateSuccess
        //            });
        //        }
        //        else
        //            return Ok(new
        //            {
        //                status = false,
        //                mess = MessageResult.UpLoadFileFail
        //            });
        //    }
        //    else
        //        return Ok(new
        //        {
        //            status = false,
        //            mess = MessageResult.Fail
        //        });
        //}
    }
}