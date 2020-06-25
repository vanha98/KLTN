using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Interfaces;
using Data.Models;
using KLTN.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QLHoiDongController : Controller
    {
        private readonly IHoiDong _serviceHoiDong;
        private readonly IDeTaiNghienCuu _serviceDeTai;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public QLHoiDongController(IHoiDong serviceHoiDong, IDeTaiNghienCuu serviceDeTai, IMapper mapper, UserManager<AppUser> userManager)
        {
            _serviceHoiDong = serviceHoiDong;
            _serviceDeTai = serviceDeTai;
            _mapper = mapper;
            _userManager = userManager;
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
                var entity = await _serviceHoiDong.GetAll();
                //foreach(var item in entity)
                //{
                //    string name = item.IdgiangVienNavigation.Ho +" "+ item.IdgiangVienNavigation.Ten;
                //}

                //Mapping
                
                //Sorting  
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(HoiDong)).Find(sortColumn, false);
                    if (sortColumnDirection.Equals("asc"))
                        entity = entity.OrderBy(x => prop.GetValue(x));
                    else
                        entity = entity.OrderByDescending(x => prop.GetValue(x));
                }

                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    entity = entity.Where(x => x.IdNguoiTao.ToString().IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.NgayLap.Value.ToString().IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.TenHoiDong.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    //|| x.MoTa.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    //|| x.TenTep.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    //|| x.TenGiangVien.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    //|| x.TenDeTai.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    );
                }

                //total number of rows counts   
                recordsTotal = entity.Count();
                //Paging   
                var data = entity.Skip(skip).Take(pageSize).ToList().OrderBy(x => x.Status);
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

        [HttpPost]
        public async Task<IActionResult> LapHD(LapHoiDongViewModel obj)
        {
            return Ok();
        } 
    }
}