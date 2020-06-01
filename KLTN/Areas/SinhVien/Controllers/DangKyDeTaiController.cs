using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data.Bo;
using Data.Interfaces;
using Data.Models;
using AutoMapper;
using Data.Enum;
using System.ComponentModel;
using KLTN.Areas.GVHD.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;

namespace KLTN.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class DangKyDeTaiController : Controller
    {
        private readonly IDeTaiNghienCuu _service;
        private readonly IMapper _mapper;
        public DangKyDeTaiController (IDeTaiNghienCuu service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();

                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();

                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;

                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                // getting all Customer data  
                var entity = await _service.GetAll(x=>x.TinhTrangPheDuyet == (int)StatusPheDuyetDeTai.DaDuyet);
                //foreach(var item in entity)
                //{
                //    string name = item.IdgiangVienNavigation.Ho +" "+ item.IdgiangVienNavigation.Ten;
                //}

                //Mapping
                var list = _mapper.Map<IEnumerable<DeTaiNghienCuu>, IEnumerable<DeTaiNghienCuuViewModel>>(entity);
                foreach (var item in list)
                {
                    if (int.Parse(item.TinhTrangDangKy) == (int)StatusDangKyDeTai.Con)
                    {
                        item.TinhTrangDangKy = "Còn";
                    }
                    else
                        item.TinhTrangDangKy = "Hết";
                }

                //Sorting  
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(DeTaiNghienCuuViewModel)).Find(sortColumn, false);
                    if (sortColumnDirection.Equals("asc"))
                        list = list.OrderBy(x => prop.GetValue(x));
                    else
                        list = list.OrderByDescending(x => prop.GetValue(x));
                }

                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    list = list.Where(x => x.Id.ToString().Contains(searchValue)
                    || x.TenDeTai.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.TinhTrangDangKy.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.MoTa.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.HoTenGVHD.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.Email.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.TenTep.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    );
                }

                //total number of rows counts   
                recordsTotal = list.Count();
                //Paging   
                var data = list.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = data
                });

            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateEditPopup", new DeTaiNghienCuuViewModel { });
        }

        [HttpPost]
        public async Task<ActionResult> Create(DeTaiNghienCuuViewModel vmodel)
        {
            IEnumerable<DeTaiNghienCuu> list = await _service.GetAll();
            if (list.Count() != 0)
            {
                DeTaiNghienCuu LastE = list.OrderBy(x => x.Id).LastOrDefault();
                vmodel.Id = _service.KhoiTaoMa(LastE);
            }
            else
                vmodel.Id = long.Parse(DateTime.Now.Year.ToString() + "001");
            var model = new DeTaiNghienCuu()
            {
                Id = vmodel.Id,
                TenDeTai = vmodel.TenDeTai,
                MoTa = vmodel.MoTa,
                //IdgiangVien = 
                NgayLap = DateTime.Now,
                TinhTrangDangKy = (int)StatusDangKyDeTai.Het,
                TinhTrangPheDuyet = (int)StatusPheDuyetDeTai.ChuaGui,
                Loai = LoaiDeTai.DeXuat
            };
            if (await UpLoadFile(vmodel.Files, model) == false)
            {
                return Json(new { status = false, mess = MessageResult.UpLoadFileFail });
            }
            try
            {
                await _service.Add(model);
                return Json(new { status = true, create = true, data = new { NgayLap = DateTime.Now.ToString("dd/MM/yyyy"), Id = vmodel.Id, TenTep = model.TenTep }, mess = MessageResult.CreateSuccess });
            }
            catch
            {
                return Json(new { status = false, mess = MessageResult.Fail });
            }

        }

        [NonAction]
        public async Task<bool> UpLoadFile(IFormFile file, DeTaiNghienCuu model)
        {
            if (file == null) return true;
            string[] permittedExtensions = { ".txt", ".pdf", ".doc", ".docx", ".xlsx", ".xls" };

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    model.TenTep = file.FileName;
                    model.TepDinhKem = memoryStream.ToArray();
                    return true;
                }
                else return false;
            }
        }

        [HttpPost]
        public async Task<IActionResult> LuuDeTai(long id)
        {
            var data = await _service.GetById(id);
            if(data != null)
            {
                return Json(new
                {
                    status = true,
                    data = data,
                });
            }
            else
                return Ok(new
                {
                    status = false,
                });
        }
    }
}