using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using KLTN.Areas.SinhVien.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class BaoCaoTienDoController : Controller
    {
        private readonly IDeTaiNghienCuu _service;
        private readonly IBaoCaoTienDo _serviceBaoCao;
        private readonly INhom _serviceNhom;
        private readonly ISinhVien _serviceSV;
        private readonly INhomSinhVien _serviceNhomSV;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public BaoCaoTienDoController(IDeTaiNghienCuu service, IBaoCaoTienDo serviceBaoCao, 
            IAuthorizationService authorizationService, ISinhVien serviceSV, 
            INhomSinhVien serviceNhomSV, INhom serviceNhom, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _mapper = mapper;
            _serviceSV = serviceSV;
            _service = service;
            _serviceBaoCao = serviceBaoCao;
            _serviceNhom = serviceNhom;
            _serviceNhomSV = serviceNhomSV;
            _hostingEnvironment = hostingEnvironment;
            _authorizationService = authorizationService;
        }
        public async Task<IActionResult> Index()
        {
            var SV = await _serviceSV.GetById(long.Parse(User.Identity.Name));
            //Kiểm tra SV đã có đề tài?
            var nhomSV = SV.NhomSinhVien.SingleOrDefault(x => x.IdnhomNavigation.Status == (int)BaseStatus.Active);
            if (nhomSV == null)
            {
                ViewBag.Check = false;
            }
            else
            {
                ViewBag.Check = true;
                ViewBag.TenDeTai = nhomSV.IddeTaiNavigation.TenDeTai;
                ViewBag.IdDeTai = nhomSV.IddeTai;
            }
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

                //
                var SV = await _serviceSV.GetById(long.Parse(User.Identity.Name));
                //Kiểm tra SV đã có đề tài?
                var nhomSV = SV.NhomSinhVien.SingleOrDefault(x => x.IdnhomNavigation.Status == (int)BaseStatus.Active);
                if(nhomSV == null)
                {
                    return Json(new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = ""
                    });
                    
                }
                // getting all Customer data  
                var entity = await _serviceBaoCao.GetAll(x => x.IddeTai == nhomSV.IddeTai);
                //foreach(var item in entity)
                //{
                //    string name = item.IdgiangVienNavigation.Ho +" "+ item.IdgiangVienNavigation.Ten;
                //}

                //Mapping
                var list = _mapper.Map<IEnumerable<BaoCaoTienDo>, IEnumerable<BaoCaoTienDoViewModel>>(entity);
                //Sorting  
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(BaoCaoTienDoViewModel)).Find(sortColumn, false);
                    if (sortColumnDirection.Equals("asc"))
                        list = list.OrderBy(x => prop.GetValue(x));
                    else
                        list = list.OrderByDescending(x => prop.GetValue(x));
                }

                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    list = list.Where(x => x.Id.ToString().Contains(searchValue)
                    || x.NoiDung.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.NgayNop.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
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

        [NonAction]
        public async Task<bool> UpLoadFile(IFormFile file, BaoCaoTienDo model)
        {
            if (file == null) return true;
            string[] permittedExtensions = { ".txt", ".pdf", ".doc", ".docx", ".xlsx", ".xls" };

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }

            string UploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FileUpload/BaoCaoTienDo");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(UploadsFolder, uniqueFileName);
            await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            model.TepDinhKem = uniqueFileName;
            model.TenTep = file.FileName;
            return true;
        }

        public async Task<IActionResult> CreateEdit(int? Id)
        {
            if (Id > 0)
            {
                BaoCaoTienDo baoCao = await _serviceBaoCao.GetById(Id.Value);
                BaoCaoTienDoViewModel vmodal = new BaoCaoTienDoViewModel
                {
                    Id = baoCao.Id,
                    NoiDung = baoCao.NoiDung,
                    TienDo = baoCao.TienDo,
                    TenTep = baoCao.TenTep,
                };
                return PartialView("_CreateEditPopup", vmodal);
            }
            else
                return PartialView("_CreateEditPopup");
        }

        public bool CheckTuanNop(DateTime ngayThucHien, int tuanDaNop, BaoCaoTienDo baoCao)
        {
            int week = ((baoCao.NgayNop.Value - ngayThucHien).Days)/7 + 1;
            if(tuanDaNop < week)
            {
                baoCao.Status = (int)StatusBaoCao.DaNop;
                baoCao.TuanDaNop = week;
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEdit(BaoCaoTienDoViewModel vmodel)
        {
            var DeTai = await _service.GetById(vmodel.IddeTai);
            if (!DeTai.NgayThucHien.HasValue)
            {
                return Ok(new
                {
                    status = false,
                    toastr = MessageResult.ChuaDenTGThucHien
                });
            }
            if ( vmodel.Id == 0)
            {
                BaoCaoTienDo baoCao = new BaoCaoTienDo();
                int tuanDaNop = DeTai.BaoCaoTienDo.LastOrDefault().TuanDaNop;
                baoCao.NoiDung = vmodel.NoiDung;
                baoCao.TienDo = vmodel.TienDo;
                baoCao.NgayNop = DateTime.Now;
                if (CheckTuanNop(baoCao.NgayNop.Value,tuanDaNop,baoCao))
                {
                    if (await UpLoadFile(vmodel.File, baoCao))
                    {
                        DeTai.BaoCaoTienDo.Add(baoCao);
                        await _service.Update(DeTai);
                        return Ok(new { status = true, mess = MessageResult.CreateSuccess });
                    }
                    else
                    {
                        return Ok(new { status = false, mess = MessageResult.UpLoadFileFail });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        status = false,
                        toastr = MessageResult.DaNopBaoCao + tuanDaNop,
                    });
                }
            }
            else
            {
                BaoCaoTienDo baoCao = await _serviceBaoCao.GetById(vmodel.Id);
                int week = ((DateTime.Now - DeTai.NgayThucHien.Value).Days / 7) + 1;
                if(baoCao.TuanDaNop == week)
                {
                    baoCao.NoiDung = vmodel.NoiDung;
                    baoCao.TienDo = vmodel.TienDo;
                    baoCao.NgayNop = DateTime.Now;
                    if (await UpLoadFile(vmodel.File, baoCao))
                    {
                        DeTai.BaoCaoTienDo.Add(baoCao);
                        await _service.Update(DeTai);
                        return Ok(new { status = true, mess = MessageResult.CreateSuccess });
                    }
                    else
                    {
                        return Ok(new { status = false, mess = MessageResult.UpLoadFileFail });
                    }
                }
                return Ok();
            }
        }
    }
}