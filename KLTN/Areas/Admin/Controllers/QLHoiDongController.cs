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
    public class QLHoiDongController : Controller
    {
        private readonly IHoiDong _serviceHoiDong;
        private readonly IBoNhiem _serviceBoNhiem;
        private readonly IDeTaiNghienCuu _serviceDeTai;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public QLHoiDongController(IHoiDong serviceHoiDong, IBoNhiem serviceBoNhiem, IDeTaiNghienCuu serviceDeTai, IMapper mapper, UserManager<AppUser> userManager)
        {
            _serviceBoNhiem = serviceBoNhiem;
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
                var entity = await _serviceBoNhiem.GetAll(x=> x.IdhoiDongNavigation.Status==1);
                List<DataHoiDongViewModel> list = new List<DataHoiDongViewModel>();
                foreach (var item in entity)
                {
                    var user = await _userManager.FindByNameAsync(item.IdhoiDongNavigation.IdNguoiTao.ToString());
                    var claim = await _userManager.GetClaimsAsync(user);
                    DataHoiDongViewModel dataHD = new DataHoiDongViewModel
                    {
                        Id= item.IdhoiDong.Value,
                        HoTenGV = item.IdgiangVienNavigation.Ho +" "+ item.IdgiangVienNavigation.Ten,
                        HoiDong = item.IdhoiDongNavigation.TenHoiDong  + " ----------- Người lập: " + claim[0].Value + " ----------- Ngày lập: " + item.IdhoiDongNavigation.NgayLap.Value.ToString("dd/MM/yyyy"),
                    };
                    if (item.VaiTro.Value == (int)LoaiVaiTro.ChuTich)
                        dataHD.VaiTro = "Chủ tịch";
                    else if(item.VaiTro.Value == (int)LoaiVaiTro.ThuKy)
                        dataHD.VaiTro = "Thư ký";
                    else if (item.VaiTro.Value == (int)LoaiVaiTro.UyVien)
                        dataHD.VaiTro = "Ủy viên";
                    else 
                        dataHD.VaiTro = "Phản biện";
                    list.Add(dataHD);
                }

                //Mapping

                //Sorting  
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(DataHoiDongViewModel)).Find(sortColumn, false);
                    if (sortColumnDirection.Equals("asc"))
                        list = list.OrderBy(x=>prop.GetValue(x)).ToList();
                    else
                        list = list.OrderByDescending(x => prop.GetValue(x)).ToList();
                }

                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    list = list.Where(x => x.HoTenGV.ToString().IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.HoiDong.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    //|| x.MoTa.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    //|| x.TenTep.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    //|| x.TenGiangVien.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    //|| x.TenDeTai.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    ).ToList();
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

        public async Task<IActionResult> ThanhLapHoiDong(int? id)
        {
            if (id == null)
                return PartialView("_ThanhLapHoiDong");
            else
            {
                var hoiDong = await _serviceHoiDong.GetById(id.Value);
                return PartialView("_ThanhLapHoiDong", hoiDong);
            }
        }

        public void BoNhiemHD(LapHoiDongViewModel obj, HoiDong hoiDong)
        {
            foreach (var item in obj.ThanhViens)
            {
                BoNhiem boNhiem = new BoNhiem
                {
                    IdgiangVien = item.IdThanhVien,
                    VaiTro = item.VaiTro,
                };
                hoiDong.BoNhiem.Add(boNhiem);
            }
        }

        [HttpPost]
        public async Task<IActionResult> LapHD(LapHoiDongViewModel obj)
        {
            if(obj.Id > 0)
            {
                var hoiDong = await _serviceHoiDong.GetById(obj.Id);
                if(await EditHoiDong(obj, hoiDong))
                    return Ok(new
                    {
                        status = true,
                        mess = MessageResult.UpdateSuccess
                    });
                else
                    return Ok(new
                    {
                        status = false,
                        mess = MessageResult.Fail
                    });
            }
            else
            {
                HoiDong hoiDong = new HoiDong
                {
                    IdNguoiTao = long.Parse(User.Identity.Name),
                    TenHoiDong = obj.TenHoiDong,
                    NgayLap = DateTime.Now
                };
                await _serviceHoiDong.Add(hoiDong);
                
                BoNhiemHD(obj, hoiDong);

                await _serviceHoiDong.Update(hoiDong);
                return Ok(new
                {
                    status = true,
                    mess = MessageResult.CreateSuccess
                });
            }
            
        }
        [NonAction]
        public async Task<bool> EditHoiDong(LapHoiDongViewModel obj, HoiDong hoiDong)
        {
            if (obj.ThanhViens != null)
            {
                BoNhiemHD(obj, hoiDong);
            }
            if (obj.DelThanhViens != null)
            {
                for(int i = 0; i<hoiDong.BoNhiem.Count(); i++)
                {
                    var item = hoiDong.BoNhiem.ToList()[i];
                    if (obj.DelThanhViens.Contains(item.Id))
                    {
                        hoiDong.BoNhiem.Remove(item);
                        i--;
                    }
                }
            }
            hoiDong.TenHoiDong = obj.TenHoiDong;
            hoiDong.NguoiSua = long.Parse(User.Identity.Name);
            hoiDong.NgaySua = DateTime.Now;
            await _serviceHoiDong.Update(hoiDong);
            return true;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var hoiDong = await _serviceHoiDong.GetById(id);
            if(hoiDong.XetDuyetVaDanhGia.Where(x=>x.Status == 1).Any())
                return Ok(new { status = false, mess = "Thất bại, hội đồng này đã được phân công xét duyệt/đánh giá đề tài" });
            hoiDong.Status = 0;
            await _serviceHoiDong.Update(hoiDong);
            return Ok(new { status = true, mess = MessageResult.UpdateSuccess });
        }
    }
}