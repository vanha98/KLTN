using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using KLTN.Areas.GVHD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    [Authorize(Roles = "SinhVien")]
    public class DeTaiDaDangKyController : Controller
    {
        private readonly IDeTaiNghienCuu _serviceDeTai;
        private readonly ISinhVien _serviceSV;
        private readonly INhomSinhVien _serviceNhomSV;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        public DeTaiDaDangKyController(IDeTaiNghienCuu serviceDeTai, ISinhVien serviceSV, INhomSinhVien serviceNhomSV, IMapper mapper)
        {
            _serviceDeTai = serviceDeTai;
            _serviceSV = serviceSV;
            _serviceNhomSV = serviceNhomSV;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> XemNhom(long id)
        {
            var DeTai = await _serviceDeTai.GetById(id);
            var NhomSV = await _serviceNhomSV.GetAll(x=>x.IddeTai == DeTai.Id);
            return ViewComponent("ToggleThongTinSinhVien", NhomSV.Select(x => x.IdsinhVienNavigation));
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
                Data.Models.SinhVien sinhVien = await _serviceSV.GetById(long.Parse(User.Identity.Name));
                List<NhomSinhVien> nhomSv = sinhVien.NhomSinhVien.ToList();
                var query = nhomSv.Select(x => x.IddeTaiNavigation);
                //Mapping
                var list = _mapper.Map<IEnumerable<DeTaiNghienCuu>, IEnumerable<DeTaiNghienCuuViewModel>>(query);
                foreach (var item in list)
                {
                    //var nhomSinhVien = await _serviceNhomSV.GetAll(x=>x.Idnhom == item.Idnhom);
                    //foreach(var x in nhomSinhVien)
                    //{
                    //    item.Mssv = item.Mssv + x.IdsinhVien.ToString() + '   ';
                    //}
                    if (int.Parse(item.TinhTrangPheDuyet) == (int)StatusDeTai.ChuaGui)
                    {
                        item.TinhTrangPheDuyet = "Chưa gửi";
                    }
                    else if (int.Parse(item.TinhTrangPheDuyet) == (int)StatusDeTai.DaGui)
                        item.TinhTrangPheDuyet = "Đã gửi";
                    else if (int.Parse(item.TinhTrangPheDuyet) == (int)StatusDeTai.DaDuyet)
                        item.TinhTrangPheDuyet = "Đã duyệt";
                    else if (int.Parse(item.TinhTrangPheDuyet) == (int)StatusDeTai.DangThucHien)
                        item.TinhTrangPheDuyet = "Đang thực hiện";
                    else if (int.Parse(item.TinhTrangPheDuyet) == (int)StatusDeTai.HoanThanh)
                        item.TinhTrangPheDuyet = "Hoàn thành";
                    else
                        item.TinhTrangPheDuyet = "Đã hủy";
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
                    || x.TinhTrangPheDuyet.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
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

        public async Task<IActionResult> HuyDeTai(long id)
        {
            var DeTai = await _serviceDeTai.GetById(id);
            if(DeTai != null)
            {
                DeTai.TinhTrangPheDuyet = (int)StatusDeTai.Huy;
                await _serviceDeTai.Update(DeTai);
                return Ok(new{
                    status =true,
                    mess = MessageResult.UpdateSuccess
                });
            }
            else
                return Ok(new
                {
                    status = false,
                    mess = MessageResult.Fail
                });
        }
    }
}