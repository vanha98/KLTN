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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using KLTN.Areas.GVHD.Models;
using KLTN.Areas.SinhVien.Models;
using Microsoft.AspNetCore.Authorization;

namespace KLTN.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    [Authorize(Roles = "SinhVien")]
    public class DangKyDeTaiController : Controller
    {
        private readonly IDeTaiNghienCuu _service;
        private readonly INhom _serviceNhom;
        private readonly ISinhVien _serviceSV;
        private readonly INhomSinhVien _serviceNhomSV;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMoDot _serviceMoDot;
        public DangKyDeTaiController (IDeTaiNghienCuu service, IAuthorizationService authorizationService, ISinhVien serviceSV, 
                                      INhomSinhVien serviceNhomSV, INhom serviceNhom, IMapper mapper, IHostingEnvironment hostingEnvironment,
                                      IMoDot serviceMoDot)
        {
            _mapper = mapper;
            _serviceSV = serviceSV;
            _service = service;
            _serviceNhom = serviceNhom;
            _serviceNhomSV = serviceNhomSV;
            _hostingEnvironment = hostingEnvironment;
            _authorizationService = authorizationService;
            _serviceMoDot = serviceMoDot;
        }
        public async Task<IActionResult> Index()
        {
            MoDot moDot = await _serviceMoDot.GetEntity(x => x.Status == (int)MoDotStatus.Mo && x.Loai == (int)MoDotLoai.DangKy);
            if(moDot != null)
            {
                if(DateTime.Now >= moDot.ThoiGianBd && DateTime.Now <= moDot.ThoiGianKt)
                {
                    double thoigian = (moDot.ThoiGianKt - DateTime.Now).Value.TotalSeconds;
                    HttpContext.Response.Headers.Add("refresh", ""+ thoigian +"; url=" + Url.Action("Index"));
                    ViewBag.DangMoDot = true;
                }
                else
                {
                    ViewBag.DangMoDot = false;
                }
            }
            else
            {
                ViewBag.DangMoDot = false;
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

                // getting all Customer data  
                var entity = await _service.GetAll(x=>x.TinhTrangDeTai == (int)StatusDeTai.DaDuyet || x.TinhTrangDeTai == (int)StatusDeTai.DaDangKy);
                if(!entity.Any())
                {
                    return Json(new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = ""
                    });
                }
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
        public async Task<IActionResult> CreateEdit()
        {
            DeTaiNghienCuuViewModel model = new DeTaiNghienCuuViewModel();
            var entity = await _service.GetEntity(x=>x.IdNguoiDangKy == long.Parse(User.Identity.Name) && x.Loai == LoaiDeTai.DeXuat);
            if(entity == null)
            {
                return PartialView("_CreateEditPopup");
            }
            else
            {
                model = _mapper.Map<DeTaiNghienCuu, DeTaiNghienCuuViewModel>(entity);
                return PartialView("_CreateEditPopup",model);
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> DeXuatDeTai(DeTaiNghienCuuViewModel vmodel)
        {
            var SV = await _serviceSV.GetById(long.Parse(User.Identity.Name));
            if (SV == null)
            {
                return Ok(new
                {
                    status = false,
                    mess = MessageResult.NotFoundSV
                });
            }
            if (vmodel.IdgiangVien == 0)
                vmodel.IdgiangVien = null;
            //Update DeTai
            if (vmodel.Id > 0)
            {
                var DeTai = await _service.GetById(vmodel.Id);
                DeTai.IdgiangVien = vmodel.IdgiangVien;
                DeTai.MoTa = vmodel.MoTa;
                DeTai.TenDeTai = vmodel.TenDeTai;
                if (await UpLoadFile(vmodel.Files, DeTai) == false)
                {
                    return Json(new { status = false, mess = MessageResult.UpLoadFileFail });
                }
                await _service.Update(DeTai);
                return Ok(new { status=true,mess=MessageResult.UpdateSuccess});
            }
            //Kiểm tra SV đã có đề tài?
            var nhomSV = SV.NhomSinhVien.SingleOrDefault(x => x.IdnhomNavigation.Status == (int)BaseStatus.Active);
            if (nhomSV!=null)
            {
                return Ok(new
                {
                    status = false,
                    mess = MessageResult.ExistDeTai
                });
            }
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
                IdNguoiDangKy = long.Parse(User.Identity.Name),
                IdgiangVien = vmodel.IdgiangVien,
                NgayLap = DateTime.Now,
                TinhTrangDangKy = (int)StatusDangKyDeTai.Het,
                TinhTrangDeTai = (int)StatusDeTai.DaDangKy,
                Loai = LoaiDeTai.DeXuat
            };
            if (await UpLoadFile(vmodel.Files, model) == false)
            {
                return Json(new { status = false, mess = MessageResult.UpLoadFileFail });
            }
            try
            {
                Nhom nhom = new Nhom();
                await _serviceNhom.Add(nhom);
                NhomSinhVien nhomSinhVien = new NhomSinhVien { Idnhom = nhom.Id, IdsinhVien = SV.Mssv };
                model.NhomSinhVien.Add(nhomSinhVien);
                await _service.Add(model);
                
                return Json(new { status = true, create = true, data = model, mess = MessageResult.CreateSuccess });
            }
            catch
            {
                return Json(new { status = false, mess = MessageResult.Fail}) ;
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

            string UploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FileUpload/DeTaiNghienCuu");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(UploadsFolder, uniqueFileName);
            await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            model.TepDinhKem = uniqueFileName;
            model.TenTep = file.FileName;
            return true;
        }

        [HttpPost]
        public async Task<IActionResult> LuuDeTai(long id)
        {
            var DeTai = await _service.GetById(id);
            if(DeTai.TinhTrangDangKy == (int)StatusDangKyDeTai.Het)
            {
                return Ok(new
                {
                    status = false,
                    mess = "Đề tài đã hết"
                });
            }
            NhomSinhVien nhomSinhVien = new NhomSinhVien
            {
                IdsinhVien = long.Parse(User.Identity.Name),
                IddeTai = DeTai.Id
            };
            
            if(DeTai != null)
            {
                Nhom nhom = new Nhom();
                nhom.Status = (int)BaseStatus.Active;
                nhom.NhomSinhVien.Add(nhomSinhVien);
                await _serviceNhom.Add(nhom);

                DeTai.TinhTrangDangKy = (int)StatusDangKyDeTai.Het;
                DeTai.IdNguoiDangKy = long.Parse(User.Identity.Name);
                DeTai.TinhTrangDeTai = (int)StatusDeTai.DaDangKy;
                await _service.Update(DeTai);
                return Json(new
                {
                    status = true,
                    data = DeTai,
                    mess = "Lưu đề tài thành công"
                });
            }
            else
                return Ok(new
                {
                    status = false,
                });
        }

        public async Task<IActionResult> LoadCurrentDeTai()
        {
            NhomSinhVien nhom = await _serviceNhomSV.GetEntity(x => x.IdsinhVien == long.Parse(User.Identity.Name) && x.IdnhomNavigation.Status == (int)BaseStatus.Active);
            if(nhom == null)
            {
                return Ok() ;
            }
            var DeTai = await _service.GetEntity(x => x.Id == nhom.IddeTai && x.TinhTrangDeTai == (int)StatusDeTai.DaDangKy);
            return PartialView("_LoadDeTaiDaDangKy",DeTai);
        }
        
        [HttpPost]
        public async Task<IActionResult> HuyDangKy(long id)
        {
            var DeTai = await _service.GetById(id);
            if (DeTai != null && DeTai.Loai == LoaiDeTai.CoSan)
            {
                var nhomSV = await _serviceNhomSV.GetAll(x => x.IddeTai == DeTai.Id);
                var nhom = nhomSV.First().IdnhomNavigation;
                if (nhomSV.Count() > 1)
                {
                    foreach(var item in nhomSV)
                    {
                        await _serviceNhomSV.Delete(item);
                    }
                    await _serviceNhom.Delete(nhom);
                }
                else
                {
                    await _serviceNhomSV.Delete(nhomSV.First());
                    await _serviceNhom.Delete(nhom);
                }
                DeTai.TinhTrangDangKy = (int)StatusDangKyDeTai.Con;
                DeTai.TinhTrangDeTai = (int)StatusDeTai.MoiTao;
                DeTai.IdNguoiDangKy = null;
                await _service.Update(DeTai);
                return Ok(new
                {
                    status = true,
                    mess = MessageResult.UpdateSuccess
                });
            }
            else
            {
                var nhomSV = await _serviceNhomSV.GetAll(x => x.IddeTai == DeTai.Id);
                var nhom = nhomSV.First().IdnhomNavigation;
                if (nhomSV.Count() > 1)
                {
                    foreach (var item in nhomSV)
                    {
                        await _serviceNhomSV.Delete(item);
                    }
                    await _serviceNhom.Delete(nhom);
                }
                else
                {
                    await _serviceNhomSV.Delete(nhomSV.First());
                    await _serviceNhom.Delete(nhom);
                }
                await _service.Delete(DeTai);
                return Ok(new
                {
                    status = true,
                    mess = MessageResult.UpdateSuccess
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DangKyNhom(long idDeTai, string mssv)
        {
            long masv = 0;
            long.TryParse(mssv, out masv);
            var SV = await _serviceSV.GetById(masv);
            if (SV == null)
            {
                return Ok(new
                {
                    status = false,
                    mess = MessageResult.NotFoundSV
                });
            }
            var checkSV = SV.NhomSinhVien.Where(x => x.IdnhomNavigation.Status == (int)BaseStatus.Active);
            if (checkSV.Any())
            {
                return Ok(new
                {
                    status = false,
                    mess = MessageResult.ExistDeTai
                });
            }
            var DeTai = await _service.GetById(idDeTai);
            var Nhom = DeTai.NhomSinhVien.SingleOrDefault(x => x.IddeTai == DeTai.Id);
            NhomSinhVien nhomSV = new NhomSinhVien { IddeTai = DeTai.Id, Idnhom = Nhom.Idnhom, IdsinhVien = SV.Mssv };
            await _serviceNhomSV.Add(nhomSV);
            return Ok(new
            {
                status = true,
                mess = MessageResult.RegisterSuccess
            });
        }

        [HttpPost]
        public async Task<IActionResult> CheckPopupNhom(long idDeTai)
        {
            var nhomSV = await _serviceNhomSV.GetAll(x => x.IddeTai == idDeTai);
            var DeTai = await _service.GetById(idDeTai);
            var SV = await _serviceNhomSV.GetEntity(x => x.IddeTai == idDeTai && x.IdsinhVien != long.Parse(User.Identity.Name));
            if (nhomSV.Count() > 1)
            {
                var model = _mapper.Map<Data.Models.SinhVien, SinhVienViewModel>(SV.IdsinhVienNavigation);
                model.IdNguoiDangKy = DeTai.IdNguoiDangKy.Value;
                return PartialView("_ThongTinThanhVien", model);
            }
            else
                return Ok(new { status=false });
        }

        [HttpPost]
        public async Task<IActionResult> HuyNhom(long idDeTai)
        {
            var nhomSV = await _serviceNhomSV.GetAll(x => x.IddeTai == idDeTai);
            if(nhomSV.Count() < 2)
                return Ok(new { status = false,mess=MessageResult.Fail });
            foreach (var item in nhomSV)
            {
                if (item.IdsinhVien != long.Parse(User.Identity.Name))
                    await _serviceNhomSV.Delete(item);
            }
            return Ok(new { status = true, mess=MessageResult.UpdateSuccess });
        }
    }
}