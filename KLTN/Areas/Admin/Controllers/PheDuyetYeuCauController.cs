using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using KLTN.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PheDuyetYeuCauController : Controller
    {
        private readonly IPheDuyetYeuCau _servicePheDuyetYeuCau;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public PheDuyetYeuCauController(IPheDuyetYeuCau pheDuyetYeuCau, IMapper mapper, UserManager<AppUser> userManager)
        {
            _servicePheDuyetYeuCau = pheDuyetYeuCau;
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
                var entity = await _servicePheDuyetYeuCau.GetAll();
                //foreach(var item in entity)
                //{
                //    string name = item.IdgiangVienNavigation.Ho +" "+ item.IdgiangVienNavigation.Ten;
                //}

                //Mapping
                var list = _mapper.Map<IEnumerable<YeuCauPheDuyet>, IEnumerable<YeuCauPheDuyetViewModel>>(entity);
                foreach(var item in list)
                {
                    if (item.IdNguoiDuyet != "0")
                    {
                        var user = await _userManager.FindByNameAsync(item.IdNguoiDuyet);
                        var claim = await _userManager.GetClaimsAsync(user);
                        item.IdNguoiDuyet = claim[0].Value;
                    }
                    else
                        item.IdNguoiDuyet = "";
                }
                //Sorting  
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(YeuCauPheDuyetViewModel)).Find(sortColumn, false);
                    if (sortColumnDirection.Equals("asc"))
                        list = list.OrderBy(x => prop.GetValue(x));
                    else
                        list = list.OrderByDescending(x => prop.GetValue(x));
                }

                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    list = list.Where(x => x.IddeTai.ToString().IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.NgayTao.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.IdNguoiDuyet.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.MoTa.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.TenTep.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.TenGiangVien.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.TenDeTai.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    );
                }

                //total number of rows counts   
                recordsTotal = list.Count();
                //Paging   
                var data = list.Skip(skip).Take(pageSize).ToList().OrderBy(x=>x.Status);
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
        public async Task<IActionResult> ChangeStatus (long idDeTai, int type)
        {
            var yeuCau = await _servicePheDuyetYeuCau.GetEntity(x => x.IddeTai == idDeTai && x.Status == (int)StatusYeuCauPheDuyet.ChuaXuLy);
            if(type == 1) //approve
            {
                yeuCau.IddeTaiNghienCuuNavigation.TinhTrangPheDuyet = (int)StatusPheDuyetDeTai.DaDuyet;
                yeuCau.Status = (int)StatusYeuCauPheDuyet.DaDuyet;
            }
            else
            {
                yeuCau.IddeTaiNghienCuuNavigation.TinhTrangPheDuyet = (int)StatusPheDuyetDeTai.ChuaGui;
                yeuCau.Status = (int)StatusYeuCauPheDuyet.Huy;
            }
            yeuCau.NgayDuyet = DateTime.Now;
            yeuCau.IdNguoiDuyet = long.Parse(User.Identity.Name);
            await _servicePheDuyetYeuCau.Update(yeuCau);
            return Ok(new
            {
                status = true,
                mess = MessageResult.UpdateSuccess
            });
        }
    }
}