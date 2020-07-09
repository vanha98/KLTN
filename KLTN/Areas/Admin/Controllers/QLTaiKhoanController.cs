using Data.Interfaces;
using Data.Models;
using KLTN.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace KLTN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QLTaiKhoanController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly KLTNContext _context;
        public QLTaiKhoanController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, KLTNContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> Index(string mess, string status)
        {
            if (mess != "")
            {
                ViewBag.mess = mess;
                ViewBag.status = status;
            }

            List<ThongTinTaiKhoan> TaiKhoanQuanLyVaQuanTri = new List<ThongTinTaiKhoan>();
            List<ThongTinTaiKhoan> TaiKhoanSinhVienVaGiangVien = new List<ThongTinTaiKhoan>();

            IList<AppUser> listGiangVien = await _userManager.GetUsersInRoleAsync("GVHD");
            foreach(var item in listGiangVien)
            {
                ThongTinTaiKhoan thongtin = await LayThongTin("GVHD", long.Parse(item.UserName));
                if(item.IsEnabled != null)
                {
                    thongtin.TrangThai = (bool)item.IsEnabled;
                }
                else
                {
                    thongtin.TrangThai = false;
                }

                TaiKhoanSinhVienVaGiangVien.Add(thongtin);
            }

            IList<AppUser> listSinhVien = await _userManager.GetUsersInRoleAsync("SinhVien");
            foreach (var item in listSinhVien)
            {
                ThongTinTaiKhoan thongtin = await LayThongTin("SinhVien", long.Parse(item.UserName));
                if (item.IsEnabled != null)
                {
                    thongtin.TrangThai = (bool)item.IsEnabled;
                }
                else
                {
                    thongtin.TrangThai = false;
                }

                TaiKhoanSinhVienVaGiangVien.Add(thongtin);
            }

            IList<AppUser> listQuanLy = await _userManager.GetUsersInRoleAsync("QuanLy");
            foreach (var item in listQuanLy)
            {
                ThongTinTaiKhoan thongtin = await LayThongTin("QuanLy", long.Parse(item.UserName));
                if (item.IsEnabled != null)
                {
                    thongtin.TrangThai = (bool)item.IsEnabled;
                }
                else
                {
                    thongtin.TrangThai = false;
                }

                TaiKhoanQuanLyVaQuanTri.Add(thongtin);
            }

            IList<AppUser> listAdmin = await _userManager.GetUsersInRoleAsync("Administrators");
            foreach (var item in listAdmin)
            {
                ThongTinTaiKhoan thongtin = await LayThongTin("Admin", long.Parse(item.UserName));
                if (item.IsEnabled != null)
                {
                    thongtin.TrangThai = (bool)item.IsEnabled;
                }
                else
                {
                    thongtin.TrangThai = false;
                }

                TaiKhoanQuanLyVaQuanTri.Add(thongtin);
            }

            ViewBag.TaiKhoanSinhVienVaGiangVien = TaiKhoanSinhVienVaGiangVien;
            ViewBag.TaiKhoanQuanLyVaQuanTri = TaiKhoanQuanLyVaQuanTri;
            return View();
        }

        [HttpPost]
        public IActionResult Create(long Id, string Ho, string Ten, int GioiTinh
                                                , DateTime NgaySinh, string DiaChi, string Sdt
                                                , string Email, string role)
        {
            TaoNguoiDung(Id, Ho, Ten, GioiTinh, NgaySinh, DiaChi, Sdt, Email, role);
            
            return RedirectToAction("Index");
        }

        public async Task<ThongTinTaiKhoan> LayThongTin(string nguoidung, long id)
        {
            if (nguoidung == "GVHD")
            {
                GiangVien giangVien = await _context.GiangVien.FindAsync(id);
                if (giangVien != null)
                {
                    ThongTinTaiKhoan thongTinTaiKhoan = new ThongTinTaiKhoan()
                    {
                        TenTaiKhoan = giangVien.Id.ToString(),
                        HoTen = giangVien.Ho + " " + giangVien.Ten,
                        Sdt = giangVien.Sdt,
                        Email = giangVien.Email,
                        VaiTro = "Giảng viên",
                        TrangThai = false
                    };
                    return thongTinTaiKhoan;
                }
                return null;
            }
            else if (nguoidung == "SinhVien")
            {
                Data.Models.SinhVien sinhVien = await _context.SinhVien.FindAsync(id);
                if (sinhVien != null)
                {
                    ThongTinTaiKhoan thongTinTaiKhoan = new ThongTinTaiKhoan()
                    {
                        TenTaiKhoan = sinhVien.Mssv.ToString(),
                        HoTen = sinhVien.Ho + " " + sinhVien.Ten,
                        Sdt = sinhVien.Sdt,
                        Email = sinhVien.Email,
                        VaiTro = "Sinh viên",
                        TrangThai = false
                    };
                    return thongTinTaiKhoan;
                }
                return null;
            }
            else if (nguoidung == "QuanLy")
            {
                QuanLy quanLy = await _context.QuanLy.FindAsync(id);
                if (quanLy != null)
                {
                    ThongTinTaiKhoan thongTinTaiKhoan = new ThongTinTaiKhoan()
                    {
                        TenTaiKhoan = quanLy.Id.ToString(),
                        HoTen = quanLy.Ho + " " + quanLy.Ten,
                        Sdt = quanLy.Sdt,
                        Email = quanLy.Email,
                        VaiTro = "Quản lý",
                        TrangThai = false
                    };
                    return thongTinTaiKhoan;
                }
                return null;
            }
            else
            {
                QuanLy quanLy = await _context.QuanLy.FindAsync(id);
                if (quanLy != null)
                {
                    ThongTinTaiKhoan thongTinTaiKhoan = new ThongTinTaiKhoan()
                    {
                        TenTaiKhoan = quanLy.Id.ToString(),
                        HoTen = quanLy.Ho + " " + quanLy.Ten,
                        Sdt = quanLy.Sdt,
                        Email = quanLy.Email,
                        VaiTro = "Quản trị",
                        TrangThai = false
                    };
                    return thongTinTaiKhoan;
                }
                return null;
            }
        }

        public async Task<IActionResult> ChangStatus(string id)
        {
            var user = _userManager.FindByNameAsync(id);
            if ((bool)user.Result.IsEnabled)
            {
                user.Result.IsEnabled = false;
            }
            else
            {
                user.Result.IsEnabled = true;
            }

            await _userManager.UpdateAsync(user.Result);

            return RedirectToAction("Index", new { mess = "Thay đổi trạng thái thành công", status = "success" });
        }
        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile Files, CancellationToken cancellationToken)
        {
            if (Files == null || Files.Length <= 0)
            {
                return RedirectToAction("Index", new {  mess = "Tệp excel trống", status = "fail"});
            }

            if (!Path.GetExtension(Files.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", new { mess = "Phiên bản tệp excel không được hỗ trợ", status = "fail" });
            }

            //var list = new List<UserInfo>();

            using (var stream = new MemoryStream())
            {
                await Files.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.End.Row;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        if(worksheet.Cells[row, 1].Value != null)
                        {
                            long Ma = long.Parse(worksheet.Cells[row, 1].Value.ToString().Trim());
                            string Ho = worksheet.Cells[row, 2].Value.ToString().Trim();
                            string Ten = worksheet.Cells[row, 3].Value.ToString().Trim();
                            string valueGioiTinh = worksheet.Cells[row, 4].Value.ToString().Trim();
                            int GioiTinh = 0;
                            if (valueGioiTinh == "Nam")
                            {
                                GioiTinh = 1;
                            }
                            else
                            {
                                GioiTinh = 0;
                            }
                            string valueNgaySinh = worksheet.Cells[row, 5].Value.ToString().Trim().Substring(0, 10);

                            DateTime NgaySinh = DateTime.ParseExact(valueNgaySinh, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            string DiaChi = worksheet.Cells[row, 6].Value.ToString().Trim();
                            string Sdt = worksheet.Cells[row, 7].Value.ToString().Trim();
                            string Email = worksheet.Cells[row, 8].Value.ToString().Trim();
                            string VaiTro = worksheet.Cells[row, 9].Value.ToString().Trim();

                            TaoNguoiDung(Ma, Ho, Ten, GioiTinh, NgaySinh, DiaChi, Sdt, Email, VaiTro);
                        }
                        
                    }
                }
            }

            // add list to db ..  
            // here just read and return  

            return RedirectToAction("Index", new { mess = "Tạo tài khoản thành công", status = "success" });
        }

        public void TaoNguoiDung(long ma, string ho, string ten, int gioitinh, DateTime ngaysinh,
                                       string diachi, string sdt, string email, string vaitro)
        {
            try
            {
                if (vaitro == "SinhVien")
                {
                    Data.Models.SinhVien sinhVienExist = _context.SinhVien.Find(ma);
                    if(sinhVienExist == null)
                    {
                        Data.Models.SinhVien sinhVien = new Data.Models.SinhVien()
                        {
                            Mssv = ma,
                            Ho = ho,
                            Ten = ten,
                            GioiTinh = gioitinh,
                            NgaySinh = ngaysinh,
                            DiaChi = diachi,
                            Sdt = sdt,
                            Email = email,
                            Status = 1
                        };
                        _context.SinhVien.Add(sinhVien);
                        _context.SaveChanges();
                    }
                }
                else if (vaitro == "GVHD")
                {
                    GiangVien giangVienExist = _context.GiangVien.Find(ma);
                    if(giangVienExist == null)
                    {
                        GiangVien giangVien = new GiangVien()
                        {
                            Id = ma,
                            Ho = ho,
                            Ten = ten,
                            GioiTinh = gioitinh,
                            NgaySinh = ngaysinh,
                            DiaChi = diachi,
                            Sdt = sdt,
                            Email = email,
                            Status = 1
                        };
                        _context.GiangVien.Add(giangVien);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    QuanLy quanLyExist = _context.QuanLy.Find(ma);
                    if(quanLyExist == null)
                    {
                        QuanLy quanLy = new QuanLy()
                        {
                            Id = ma,
                            Ho = ho,
                            Ten = ten,
                            GioiTinh = gioitinh,
                            NgaySinh = ngaysinh,
                            DiaChi = diachi,
                            Sdt = sdt,
                            Email = email,
                            Status = 1
                        };
                        _context.QuanLy.Add(quanLy);
                        _context.SaveChanges();
                    }
                }

                var user = _userManager.FindByNameAsync(ma.ToString()).Result;
                if (user == null)
                {
                    user = new AppUser
                    {
                        UserName = ma.ToString(),
                        IsEnabled = true
                    };
                    var result = _userManager.CreateAsync(user, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    result = _userManager.AddToRoleAsync(user, vaitro).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = _userManager.AddClaimsAsync(user, new Claim[]{
                    new Claim("Name", ho + " " + ten) }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                }
            }
            catch
            {

            }
        }
    }
}