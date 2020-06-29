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
using KLTN.Areas.GVHD.Models;
using KLTN.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Areas.GVHD.Controllers
{
    [Area("GVHD")]
    [Authorize(Roles = "GVHD")]
    public class QLDeTaiController : Controller
    {
        private readonly IDeTaiNghienCuu _service;
        private readonly INhomSinhVien _serviceNhomSV;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public QLDeTaiController(IDeTaiNghienCuu service,INhomSinhVien serviceNhomSV, IMapper mapper, IHostingEnvironment hostingEnvironment, IAuthorizationService authorizationService)
        {
            _service = service;
            _mapper = mapper;
            _serviceNhomSV = serviceNhomSV;
            _authorizationService = authorizationService;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            //IEnumerable<DeTaiNghienCuu> model = await _service.GetAll();
            return View(/*model.OrderBy(x=>x.TinhTrangPheDuyet)*/);
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
                var entity = await _service.GetAll(x=>x.IdgiangVien == long.Parse(User.Identity.Name));
                
                //Sorting  
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(DeTaiNghienCuu)).Find(sortColumn,false);
                    if (sortColumnDirection.Equals("asc"))
                        entity = entity.OrderBy(x=>prop.GetValue(x));
                    else
                        entity = entity.OrderByDescending(x => prop.GetValue(x));
                }

                //Mapping
                var list = _mapper.Map<IEnumerable<DeTaiNghienCuu>, IEnumerable<DeTaiNghienCuuViewModel>>(entity);
                foreach (var item in list)
                {
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
                    else if (int.Parse(item.TinhTrangPheDuyet) == (int)StatusDeTai.DaDangKy)
                        item.TinhTrangPheDuyet = "Đã đăng ký";
                    else if (int.Parse(item.TinhTrangPheDuyet) == (int)StatusDeTai.ChoDuyet)
                        item.TinhTrangPheDuyet = "Chờ duyệt";
                    else
                        item.TinhTrangPheDuyet = "Đã hủy";
                }

            //Search  
            if (!string.IsNullOrEmpty(searchValue))
                {
                    list = list.Where(x => x.Id.ToString().Contains(searchValue)
                    || x.TenDeTai.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.NgayLap.ToString().Contains(searchValue)
                    || x.TinhTrangPheDuyet.IndexOf(searchValue,StringComparison.OrdinalIgnoreCase)>=0
                    || x.MoTa.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    || x.TenTep.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    );
                }
                
                //total number of rows counts   
                recordsTotal = list.Count();
                //Paging   
                var data = list.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data
            });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        public async Task<bool> UpLoadFile(IFormFile file, DeTaiNghienCuu model)
        {
            if (file == null) return true;
            string[] permittedExtensions = { ".txt", ".pdf",".doc",".docx",".xlsx", ".xls" };

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }
            string UploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FileUpload/DeTaiNghienCuu");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(UploadsFolder, uniqueFileName);
            await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            model.TenTep = file.FileName;
            model.TepDinhKem = uniqueFileName;
            return true;
        }

        [NonAction]
        public async Task<bool> UpLoadFileYCChinhSua(IFormFile file, YCChinhSuaDeTai model)
        {
            if (file == null) return true;
            string[] permittedExtensions = { ".txt", ".pdf", ".doc", ".docx", ".xlsx", ".xls" };

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }
            string UploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FileUpload/DeTaiNghienCuu");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(UploadsFolder, uniqueFileName);
            await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            model.TenTep = file.FileName;
            model.TepDinhKem = uniqueFileName;
            return true;
        }

        [HttpPost]
        public async Task<ActionResult> Create(DeTaiNghienCuuViewModel vmodel)
        {
                IEnumerable<DeTaiNghienCuu> list = await _service.GetAll();
                if (list.Count() != 0)
                {
                    DeTaiNghienCuu LastE = list.OrderBy(x=>x.Id).LastOrDefault();
                    vmodel.Id = _service.KhoiTaoMa(LastE);
                }
                else
                    vmodel.Id = long.Parse(DateTime.Now.Year.ToString() + "001");
                var model = new DeTaiNghienCuu()
                {
                    Id = vmodel.Id,
                    TenDeTai = vmodel.TenDeTai,
                    MoTa = vmodel.MoTa,
                    IdgiangVien = long.Parse(User.Identity.Name),
                    NgayLap = DateTime.Now,
                    TinhTrangDangKy = (int)StatusDangKyDeTai.Con,
                    TinhTrangPheDuyet = (int)StatusDeTai.ChuaGui,
                    Loai = LoaiDeTai.CoSan
                };
                if (await UpLoadFile(vmodel.Files, model) == false)
                {
                    return Json(new { status = false, mess = MessageResult.UpLoadFileFail });
                }
                try
                {
                    var isAuthorize = await _authorizationService.AuthorizeAsync(User, model, DeTaiNghienCuuOperation.Create);
                    if (!isAuthorize.Succeeded)
                        return Ok(new
                        {
                            status = false,
                            toastr = MessageResult.AccessDenied
                        });
                    await _service.Add(model);
                    return Json(new { status = true, create=true, data = new { NgayLap = DateTime.Now.ToString("dd/MM/yyyy"), Id = vmodel.Id, TenTep = model.TenTep }, mess = MessageResult.CreateSuccess });
                }
                catch
                {
                    return Json(new { status = false, mess = MessageResult.Fail });
                }
            

        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateEditPopup");
        }

        [HttpGet]
        public async Task<ActionResult> Edit (long? id)
        {
            DeTaiNghienCuuViewModel model = new DeTaiNghienCuuViewModel();
            if (id.HasValue)
            {
                var entity = await _service.GetById(id.Value);
                model.Id = entity.Id;
                model.TenDeTai = entity.TenDeTai;
                model.MoTa = entity.MoTa;
                model.TenTep = entity.TenTep;
                //if (entity.TepDinhKem != null && entity.TepDinhKem.Length > 0)
                //{   var stream = new MemoryStream(entity.TepDinhKem);
                //    var ext = Path.GetExtension(entity.TenTep).ToLowerInvariant();
                //    model.Files = new FormFile(stream, 0, entity.TepDinhKem.Length, "Files", entity.TenTep);
                //}
            }
            return PartialView("_CreateEditPopup",model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit (DeTaiNghienCuuViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = await _service.GetById(model.Id);
                if (entity == null)
                {
                    return Ok(new
                    {
                        status = false,
                        toastr = MessageResult.NotFoundObject
                    });
                }
                var isAuthorize = await _authorizationService.AuthorizeAsync(User, entity, DeTaiNghienCuuOperation.Update);
                if (!isAuthorize.Succeeded)
                    return Ok(new
                    {
                        status = false,
                        toastr = MessageResult.AccessDenied
                    });
                else
                {
                    if (entity.TinhTrangPheDuyet == (int)StatusDeTai.ChuaGui || entity.TinhTrangPheDuyet == (int)StatusDeTai.DaGui)
                    {
                        if (await UpLoadFile(model.Files, entity) == false)
                        {
                            return Ok(new
                            {
                                status = false,
                                mess = MessageResult.UpLoadFileFail
                            });
                        }
                        entity.TenDeTai = model.TenDeTai;
                        entity.MoTa = model.MoTa;
                        await _service.Update(entity);
                        return Ok(new { status = true, mess = MessageResult.UpdateSuccess });
                    }
                    else
                    {
                        YCChinhSuaDeTai yCChinhSua = new YCChinhSuaDeTai
                        {
                            MoTa = model.MoTa,
                            TenDeTai = model.TenDeTai,
                        };
                        YeuCauPheDuyet yeuCau = new YeuCauPheDuyet
                        {
                            LoaiYeuCau = (int)LoaiYeuCauPheDuyet.ChinhSua,
                            NgayTao = DateTime.Now,
                        };
                        if (await UpLoadFileYCChinhSua(model.Files, yCChinhSua) == false)
                        {
                            return Ok(new
                            {
                                status = false,
                                mess = MessageResult.UpLoadFileFail
                            });
                        }
                        var yeuCauHT = entity.YeuCauPheDuyet.FirstOrDefault(x=>x.LoaiYeuCau == (int)LoaiYeuCauPheDuyet.ChinhSua && x.Status == 0);
                        var yCChinhSuaHT = entity.YCChinhSuaDeTai.FirstOrDefault(x => x.Status == 0);

                        entity.YeuCauPheDuyet.Remove(yeuCauHT);
                        entity.YCChinhSuaDeTai.Remove(yCChinhSuaHT);
                        entity.YeuCauPheDuyet.Add(yeuCau);
                        entity.YCChinhSuaDeTai.Add(yCChinhSua);

                        entity.TinhTrangPheDuyet = (int)StatusDeTai.ChoDuyet;
                        await _service.Update(entity);
                        return Ok(new { status = true, mess = MessageResult.UpdateSuccess });
                    }
                }
            }
            else { return Ok(new { status = false, mess = MessageResult.Fail }); }
        }

        //private Dictionary<string,string> GetMyTypes()
        //{
        //    return new Dictionary<string, string>
        //    {
        //        {".txt","text/plain" },
        //        {".doc","application/vnd.ms-word"},
        //        {".docx", "application/vnd.ms-word"},
        //        {".pdf","application/pdf" },
        //        {".xls", "application/vnd.ms-excel" },
        //        {".xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"  }
        //    };
        //}

        //[HttpGet]
        //public async Task<IActionResult> DownLoadFile(long id)
        //{
        //    DeTaiNghienCuu entity = await _service.GetById(id);
        //    var ext = Path.GetExtension(entity.TenTep).ToLowerInvariant();
        //    return File(entity.TepDinhKem, GetMyTypes()[ext], entity.TenTep);
        //}

        //Gửi đề tài (status: đã gửi) - Hủy đề tài (hủy)
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(long[] data, int type)
        {
            if (data.Count() == 0)
                return Ok(new
                {
                    status = false,
                    mess = MessageResult.NotSelectDeTai
                }) ;          
            DeTaiNghienCuu entity = new DeTaiNghienCuu();
            foreach (long item in data)
            {
                entity = await _service.GetById(item);
                var isAuthorize = await _authorizationService.AuthorizeAsync(User, entity, DeTaiNghienCuuOperation.Update);
                if (!isAuthorize.Succeeded)
                    return Ok(new
                    {
                        status = false,
                        toastr = MessageResult.AccessDenied
                    });
                if (type == 2) // xoa detai
                {
                    //if (entity.TinhTrangPheDuyet != (int)StatusDeTai.ChuaGui || entity.TinhTrangPheDuyet != (int)StatusDeTai.DaGui)
                    //{
                    //    YeuCauPheDuyet yeuCau = new YeuCauPheDuyet
                    //    {
                    //        LoaiYeuCau = (int)LoaiYeuCauPheDuyet.Xoa,
                    //        NgayTao = DateTime.Now,
                    //    };
                    //    entity.YeuCauPheDuyet.Clear();
                    //    entity.YeuCauPheDuyet.Add(yeuCau);
                    //    entity.TinhTrangPheDuyet = (int)StatusDeTai.ChoDuyet;
                    //}
                    //else
                    await _service.Delete(entity);
                    return Ok(new
                    {
                        status = true,
                        mess = MessageResult.UpdateSuccess
                    });
                }
                else if (type == 0) // gui duyet dang ky
                {
                    if(entity.TinhTrangPheDuyet.Value == (int)StatusDeTai.DaGui)
                    {
                        return Ok(new
                        {
                            status = false,
                            toastr = "Đã gửi yêu cầu đăng ký cho đề tài " + entity.Id
                        });
                    }
                    entity.TinhTrangPheDuyet = (int)StatusDeTai.DaGui;
                    YeuCauPheDuyet yeuCau = new YeuCauPheDuyet
                    {
                        LoaiYeuCau = (int)LoaiYeuCauPheDuyet.DuyetDangKy,
                        NgayTao = DateTime.Now,
                    };
                    entity.YeuCauPheDuyet.Add(yeuCau);
                }
                else // huy gui
                {
                    if(entity.TinhTrangPheDuyet != (int)StatusDeTai.ChuaGui)
                    { entity.TinhTrangPheDuyet = (int)StatusDeTai.ChuaGui;
                        var yeuCau = entity.YeuCauPheDuyet.FirstOrDefault(x => x.Status == (int)StatusYeuCauPheDuyet.ChuaXuLy);
                        entity.YeuCauPheDuyet.Remove(yeuCau);
                    }
                    else
                        return Ok(new
                        {
                            status = false,
                            toastr = "Đề tài "+entity.Id+ " chưa gửi đăng ký" 
                        });
                }
            }
            await _service.Update(entity);
            return Ok(new
            {
                status = true,
                mess = MessageResult.UpdateSuccess
            });
        }

        public async Task<IActionResult> XemNhom(long id)
        {
            var DeTai = await _service.GetById(id);
            var NhomSV = await _serviceNhomSV.GetAll(x => x.IddeTai == DeTai.Id);
            return ViewComponent("ToggleThongTinSinhVien", NhomSV.Select(x => x.IdsinhVienNavigation));
        }
    }
}