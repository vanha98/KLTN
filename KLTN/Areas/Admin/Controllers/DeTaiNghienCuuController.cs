using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Enum;
using Data.Interfaces;
using Data.Models;
using KLTN.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;

namespace KLTN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DeTaiNghienCuuController : Controller
    {
        private readonly IDeTaiNghienCuu _service;
        private readonly IMoDot _serviceMoDot;
        private readonly INhomSinhVien _serviceNhomSV;
        private readonly IXetDuyetDanhGia _serviceXetDuyet;
        private readonly IctXetDuyetDanhGia _serviceCTXetDuyetVaDanhGia;
        private readonly IGiangVien _serviceGiangVien;
        public DeTaiNghienCuuController(IDeTaiNghienCuu service, IMoDot serviceMoDot, INhomSinhVien serviceNhomSV
                                        , IXetDuyetDanhGia serviceXetDuyet, IctXetDuyetDanhGia serviceCTXetDuyetVaDanhGia
                                        , IGiangVien serviceGiangVien)
        {
            _service = service;
            _serviceMoDot = serviceMoDot;
            _serviceNhomSV = serviceNhomSV;
            _serviceXetDuyet = serviceXetDuyet;
            _serviceCTXetDuyetVaDanhGia = serviceCTXetDuyetVaDanhGia;
            _serviceGiangVien = serviceGiangVien;
        }
        public async Task<IActionResult> Index(string mess)
        {
            IEnumerable<MoDot> listDotDangKy = await _serviceMoDot.GetAll(x => x.Loai == (int)MoDotLoai.DangKy);
            MoDot DotDangKyMoiNhat =  listDotDangKy.ToList().Last();

            IEnumerable<DeTaiNghienCuu> listDeTaiHienTai = await _service.GetAll(x => x.TinhTrangDeTai == (int)StatusDeTai.DaDuyet 
                                                                                 || x.TinhTrangDeTai == (int)StatusDeTai.DaDangKy
                                                                                 || x.TinhTrangDeTai == (int)StatusDeTai.HoanThanh
                                                                                 || x.TinhTrangDeTai == (int)StatusDeTai.Huy
                                                                                 && (x.NgayDangKy > DotDangKyMoiNhat.ThoiGianBd && x.NgayDangKy < DotDangKyMoiNhat.ThoiGianKt)
                                                                                 || (x.NgayDangKy == null && x.TinhTrangDeTai == (int)StatusDeTai.DaDuyet));
            IEnumerable<DeTaiNghienCuu> listDeTaiDeXuatHienTai = await _service.GetAll(x => x.Loai == LoaiDeTai.DeXuat
                                                                                       && x.NgayDangKy > DotDangKyMoiNhat.ThoiGianBd
                                                                                       && x.NgayDangKy < DotDangKyMoiNhat.ThoiGianKt);
            IEnumerable<GiangVien> listGiangVien = await _serviceGiangVien.GetAll();

            DeTaiNghienCuuAdminViewModel viewModel = new DeTaiNghienCuuAdminViewModel() {
                DotDangKyHienTai = DotDangKyMoiNhat,
                listDeTaiHienTai = listDeTaiHienTai,
                listDeTaiDeXuatHienTai = listDeTaiDeXuatHienTai,
                listGiangVien = listGiangVien.Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.Ho + " " + a.Ten
                }).ToList()
            };

            if (mess != "")
            {
                ViewBag.mess = mess;
            }

            return View(viewModel);
        }

        public async Task<IActionResult> XuatBaoCaoTongQuat()
        {
            IEnumerable<MoDot> listDotDangKy = await _serviceMoDot.GetAll(x => x.Loai == (int)MoDotLoai.DangKy);
            MoDot DotDangKyMoiNhat = listDotDangKy.ToList().Last();

            IEnumerable<DeTaiNghienCuu> listDeTaiHienTai = await _service.GetAll(x => x.TinhTrangDeTai == (int)StatusDeTai.DaDuyet
                                                                                 || x.TinhTrangDeTai == (int)StatusDeTai.DaDangKy
                                                                                 && (x.NgayDangKy > DotDangKyMoiNhat.ThoiGianBd
                                                                                 && x.NgayDangKy < DotDangKyMoiNhat.ThoiGianKt)
                                                                                 || (x.NgayDangKy == null && x.TinhTrangDeTai == (int)StatusDeTai.DaDuyet));
            
            List<DeTaiNghienCuu> listDeTaiDaDangKy = listDeTaiHienTai.Where(x => x.TinhTrangDeTai == (int)StatusDeTai.DaDangKy).ToList();

            List<DeTaiNghienCuu> listDeTaiChuaDangKy = listDeTaiHienTai.Where(x => x.TinhTrangDeTai == (int)StatusDeTai.DaDuyet).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                #region -- Danh sách đề tài được mở --
                    var Sheet1 = package.Workbook.Worksheets.Add("Danh sách đề tài được mở");

                    #region -- Header --

                    Sheet1.Cells["A1"].Style.Font.Bold = true;
                    Sheet1.Cells["A1"].Value = "TRƯỜNG ĐẠI HỌC SÀI GÒN";
                    Sheet1.Cells["E1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    Sheet1.Cells["E1"].Value = DateTime.Now.ToString("HH:mm, dd/MM/yyyy");
                    Sheet1.Cells["A3:E3"].Merge = true;
                    Sheet1.Cells["A3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    Sheet1.Cells["A3"].Style.Font.Bold = true;
                    Sheet1.Cells["A3"].Value = "BÁO CÁO TỔNG QUÁT ĐỀ TÀI NGHIÊN CỨU";

                    Sheet1.Cells["A5"].Style.Font.Bold = true;
                    Sheet1.Cells["A5"].Value = "Danh sách đề tài được mở";

                    Sheet1.Cells["A7:E7"].Style.Font.Bold = true;
                    Sheet1.Cells["A7:E7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    Sheet1.Cells["A7"].Value = "Mã đề tài";
                    Sheet1.Cells["B7"].Value = "Tên đề tài";
                    Sheet1.Cells["C7"].Value = "Mô tả";
                    Sheet1.Cells["D7"].Value = "Ngày lập";
                    Sheet1.Cells["E7"].Value = "Giảng viên";

                    #endregion

                    #region -- Details --
                    int rowSheet1 = 8;
                    foreach (var item in listDeTaiHienTai)
                    {
                        Sheet1.Cells[string.Format("A{0}", rowSheet1)].Value = item.Id;
                        Sheet1.Cells[string.Format("B{0}", rowSheet1)].Value = item.TenDeTai;
                        Sheet1.Cells[string.Format("C{0}", rowSheet1)].Value = item.MoTa;
                        Sheet1.Cells[string.Format("D{0}", rowSheet1)].Value = item.NgayLap.Value.ToString("dd/MM/yyyy");
                        if(item.IdgiangVien != null)
                        {
                            Sheet1.Cells[string.Format("E{0}", rowSheet1)].Value = item.IdgiangVienNavigation.Ho + " " + item.IdgiangVienNavigation.Ten;
                        }
                        
                        rowSheet1++;
                    }
                    #endregion

                    #region -- Footer --


                    Sheet1.Cells["A7" + ":" + string.Format("E{0}", rowSheet1 - 1)].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    Sheet1.Cells["A7" + ":" + string.Format("E{0}", rowSheet1 - 1)].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    Sheet1.Cells["A7" + ":" + string.Format("E{0}", rowSheet1 - 1)].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    Sheet1.Cells["A7" + ":" + string.Format("E{0}", rowSheet1 - 1)].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    Sheet1.Cells["A:AZ"].AutoFitColumns();
                    #endregion
                #endregion

                #region -- Danh sách đề tài đã đăng ký --
                var Sheet2 = package.Workbook.Worksheets.Add("Danh sách đề tài đã đăng ký");

                #region -- Header --

                Sheet2.Cells["A1"].Style.Font.Bold = true;
                Sheet2.Cells["A1"].Value = "Danh sách đề tài đã đăng ký";

                Sheet2.Cells["A3:I3"].Style.Font.Bold = true;
                Sheet1.Cells["A3:I3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                Sheet2.Cells["A3"].Value = "Mã đề tài";
                Sheet2.Cells["B3"].Value = "Tên đề tài";
                Sheet2.Cells["C3"].Value = "Mô tả";
                Sheet2.Cells["D3"].Value = "Ngày lập";
                Sheet2.Cells["E3"].Value = "Sinh viên đăng ký";
                Sheet2.Cells["F3"].Value = "Giảng viên";
                Sheet2.Cells["G3"].Value = "Hội đồng xét duyệt";
                Sheet2.Cells["H3"].Value = "Hội đồng đánh giá nghiệm thu";
                Sheet2.Cells["I3"].Value = "Trạng thái";

                #endregion

                #region -- Details --
                int rowSheet2 = 4;
                foreach (var item in listDeTaiDaDangKy)
                {
                    Sheet2.Cells[string.Format("A{0}", rowSheet2)].Value = item.Id;
                    Sheet2.Cells[string.Format("B{0}", rowSheet2)].Value = item.TenDeTai;
                    Sheet2.Cells[string.Format("C{0}", rowSheet2)].Value = item.MoTa;
                    Sheet2.Cells[string.Format("D{0}", rowSheet2)].Value = item.NgayLap.Value.ToString("dd/MM/yyyy");
                    Sheet2.Cells[string.Format("E{0}", rowSheet2)].Value = ThanhVienNhom(item.Id).Result;
                    if(item.IdgiangVien != null)
                    {
                        Sheet2.Cells[string.Format("F{0}", rowSheet2)].Value = item.IdgiangVienNavigation.Ho + " " + item.IdgiangVienNavigation.Ten;
                    }
                    Sheet2.Cells[string.Format("G{0}", rowSheet2)].Value = HoiDong("XetDuyet",item.Id).Result;
                    Sheet2.Cells[string.Format("H{0}", rowSheet2)].Value = HoiDong("DanhGiaNghiemThu", item.Id).Result;
                    if (item.TinhTrangDeTai == (int)StatusDeTai.HoanThanh)
                    {
                        Sheet2.Cells[string.Format("I{0}", rowSheet2)].Value = "Đạt";
                    }
                    else
                    {
                        Sheet2.Cells[string.Format("I{0}", rowSheet2)].Value = "Chưa đạt";
                    }
                    rowSheet2++;
                }
                #endregion

                #region -- Footer --


                Sheet2.Cells["A3" + ":" + string.Format("I{0}", rowSheet2 - 1)].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                Sheet2.Cells["A3" + ":" + string.Format("I{0}", rowSheet2 - 1)].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                Sheet2.Cells["A3" + ":" + string.Format("I{0}", rowSheet2 - 1)].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                Sheet2.Cells["A3" + ":" + string.Format("I{0}", rowSheet2 - 1)].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                
                Sheet2.Cells["A:AZ"].AutoFitColumns();
                #endregion
                #endregion

                #region -- Danh sách đề tài chưa đăng ký --
                var Sheet3 = package.Workbook.Worksheets.Add("Danh sách đề tài chưa đăng ký");

                #region -- Header --

                Sheet3.Cells["A1"].Style.Font.Bold = true;
                Sheet3.Cells["A1"].Value = "Danh sách đề tài chưa đăng ký";

                Sheet3.Cells["A3:I3"].Style.Font.Bold = true;
                Sheet3.Cells["A3:I3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                Sheet3.Cells["A3"].Value = "Mã đề tài";
                Sheet3.Cells["B3"].Value = "Tên đề tài";
                Sheet3.Cells["C3"].Value = "Mô tả";
                Sheet3.Cells["D3"].Value = "Ngày lập";
                Sheet3.Cells["E3"].Value = "Giảng viên";

                #endregion

                #region -- Details --
                int rowSheet3 = 4;
                foreach (var item in listDeTaiChuaDangKy)
                {
                    Sheet3.Cells[string.Format("A{0}", rowSheet3)].Value = item.Id;
                    Sheet3.Cells[string.Format("B{0}", rowSheet3)].Value = item.TenDeTai;
                    Sheet3.Cells[string.Format("C{0}", rowSheet3)].Value = item.MoTa;
                    Sheet3.Cells[string.Format("D{0}", rowSheet3)].Value = item.NgayLap.Value.ToString("dd/MM/yyyy");
                    if(item.IdgiangVien != null)
                    {
                        Sheet3.Cells[string.Format("E{0}", rowSheet3)].Value = item.IdgiangVienNavigation.Ho + " " + item.IdgiangVienNavigation.Ten;
                    }
                    rowSheet3++;
                }
                #endregion

                #region -- Footer --


                Sheet3.Cells["A3" + ":" + string.Format("E{0}", rowSheet3 - 1)].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                Sheet3.Cells["A3" + ":" + string.Format("E{0}", rowSheet3 - 1)].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                Sheet3.Cells["A3" + ":" + string.Format("E{0}", rowSheet3 - 1)].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                Sheet3.Cells["A3" + ":" + string.Format("E{0}", rowSheet3 - 1)].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                Sheet3.Cells["A:AZ"].AutoFitColumns();
                #endregion
                #endregion

                package.Save();
            }

            stream.Position = 0;
            var filename = $"BaoCaoTongQuat.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }

        public async Task<IActionResult> XuatBaoCaoChiTietDeTai(long id)
        {
            #region -- Lấy dữ liệu --
            DeTaiNghienCuu deTaiNghienCuu = await _service.GetById(id);

            //Dữ liệu nhóm
            var NhomSV = await _serviceNhomSV.GetAll(x => x.IddeTai == id);
            IEnumerable<Data.Models.SinhVien> sinhViens = NhomSV.Select(x => x.IdsinhVienNavigation);

            //Dữ liệu hội đồng xét duyệt
            IEnumerable<XetDuyetVaDanhGia> hoidongXetDuyet = await _serviceXetDuyet.GetAll(x => x.IddeTai == id && x.IdmoDotNavigation.Loai == (int)MoDotLoai.XetDuyetDeTai);
            

            //Dữ liệu hội đồng đánh giá nghiệm thu
            IEnumerable<XetDuyetVaDanhGia> hoidongNghiemThu = await _serviceXetDuyet.GetAll(x => x.IddeTai == id && x.IdmoDotNavigation.Loai == (int)MoDotLoai.NghiemThuDeTai);
            #endregion

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var Sheet1 = package.Workbook.Worksheets.Add("Báo cáo chi tiết đề tài");

                

                Sheet1.Cells["A1"].Style.Font.Bold = true;
                Sheet1.Cells["A1"].Value = "TRƯỜNG ĐẠI HỌC SÀI GÒN";
                Sheet1.Cells["F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                Sheet1.Cells["F1"].Value = DateTime.Now.ToString("HH:mm, dd/MM/yyyy");
                Sheet1.Cells["A3:F3"].Merge = true;
                Sheet1.Cells["A3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                Sheet1.Cells["A3"].Style.Font.Bold = true;
                Sheet1.Cells["A3"].Value = "BÁO CÁO CHI TIẾT ĐỀ TÀI " + deTaiNghienCuu.TenDeTai.ToUpper();

                if(sinhViens.Any())
                {
                    #region -- Header Thông tin sinh viên --
                    Sheet1.Cells["A5"].Style.Font.Bold = true;
                    Sheet1.Cells["A5"].Value = "Thông tin sinh viên";

                    Sheet1.Cells["A7:E7"].Style.Font.Bold = true;
                    Sheet1.Cells["A7:E7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    Sheet1.Cells["A7"].Value = "MSSV";
                    Sheet1.Cells["B7"].Value = "Họ tên";
                    Sheet1.Cells["C7"].Value = "SĐT";
                    Sheet1.Cells["D7"].Value = "Email";

                    #endregion

                    #region -- Details Thông tin sinh viên --
                    int rowSheet1 = 8;
                    foreach (var item in sinhViens)
                    {
                        Sheet1.Cells[string.Format("A{0}", rowSheet1)].Value = item.Mssv;
                        Sheet1.Cells[string.Format("B{0}", rowSheet1)].Value = item.Ho + " " + item.Ten;
                        Sheet1.Cells[string.Format("C{0}", rowSheet1)].Value = item.Sdt;
                        Sheet1.Cells[string.Format("D{0}", rowSheet1)].Value = item.Email;
                        rowSheet1++;
                    }

                    Sheet1.Cells["A7" + ":" + string.Format("D{0}", rowSheet1 - 1)].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    Sheet1.Cells["A7" + ":" + string.Format("D{0}", rowSheet1 - 1)].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    Sheet1.Cells["A7" + ":" + string.Format("D{0}", rowSheet1 - 1)].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    Sheet1.Cells["A7" + ":" + string.Format("D{0}", rowSheet1 - 1)].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    #endregion


                    //Phần xét duyệt đề tài
                    
                    int rowSheet2 = rowSheet1 + 1;
                    Sheet1.Cells["A" + (rowSheet2)].Style.Font.Bold = true;
                    Sheet1.Cells["A" + (rowSheet2)].Value = "XÉT DUYỆT ĐỀ TÀI";

                    if(hoidongXetDuyet.Any())
                    {
                        foreach (var dot in hoidongXetDuyet)
                        {
                            #region -- Header Hội đồng xét duyệt --
                            rowSheet2 += 2;
                            Sheet1.Cells["A" + (rowSheet2)].Value = "Hội đồng: ";
                            Sheet1.Cells["B" + (rowSheet2)].Value = dot.IdhoiDongNavigation.TenHoiDong;

                            rowSheet2 += 1;
                            int rowStart = rowSheet2;
                            int rowEnd = rowSheet2;
                            Sheet1.Cells[string.Format("A{0}:F{1}", rowSheet2, rowSheet2)].Style.Font.Bold = true;
                            Sheet1.Cells[string.Format("A{0}:F{1}", rowSheet2, rowSheet2)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            Sheet1.Cells["A" + (rowSheet2)].Value = "Thành viên";
                            Sheet1.Cells["B" + (rowSheet2)].Value = "Vai trò";
                            Sheet1.Cells["C" + (rowSheet2)].Value = "Câu hỏi";
                            Sheet1.Cells["D" + (rowSheet2)].Value = "Câu trả lời";
                            Sheet1.Cells["E" + (rowSheet2)].Value = "Nhận xét";
                            Sheet1.Cells["F" + (rowSheet2)].Value = "Điểm";
                            #endregion

                            #region -- Details Hội đồng xét duyệt --
                            rowSheet2 += 1;
                            IEnumerable<CtxetDuyetVaDanhGia> ctXetDuyet = await _serviceCTXetDuyetVaDanhGia.GetAll(x => x.IdxetDuyetNavigation.IddeTai == id
                                                                                                                   && x.IdxetDuyetNavigation.IdmoDotNavigation.Loai == (int)MoDotLoai.XetDuyetDeTai
                                                                                                                   && x.IdxetDuyet == dot.Id);
                            foreach (var item in ctXetDuyet)
                            {
                                Sheet1.Cells[string.Format("A{0}", rowSheet2)].Value = item.IdgiangVienNavigation.Ho + " " + item.IdgiangVienNavigation.Ten;
                                if (item.VaiTro == 1)
                                {
                                    Sheet1.Cells[string.Format("B{0}", rowSheet2)].Value = "Chủ tịch";
                                }
                                else if (item.VaiTro == 2)
                                {
                                    Sheet1.Cells[string.Format("B{0}", rowSheet2)].Value = "Ủy viên";
                                }
                                else if (item.VaiTro == 3)
                                {
                                    Sheet1.Cells[string.Format("B{0}", rowSheet2)].Value = "Thư ký";
                                }
                                else
                                {
                                    Sheet1.Cells[string.Format("B{0}", rowSheet2)].Value = "Phản biện";
                                }
                                Sheet1.Cells[string.Format("C{0}", rowSheet2)].Value = item.CauHoi;
                                Sheet1.Cells[string.Format("D{0}", rowSheet2)].Value = item.CauTraLoi;
                                Sheet1.Cells[string.Format("E{0}", rowSheet2)].Value = item.NhanXet;
                                Sheet1.Cells[string.Format("F{0}", rowSheet2)].Value = item.Diem;
                                rowSheet2++;
                            }
                            Sheet1.Cells[string.Format("A{0}:E{1}", rowSheet2, rowSheet2)].Merge = true;
                            Sheet1.Cells["A" + rowSheet2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            Sheet1.Cells["A" + rowSheet2].Style.Font.Bold = true;
                            Sheet1.Cells["A" + rowSheet2].Value = "Điểm trung bình";
                            Sheet1.Cells["F" + rowSheet2].Value = TinhDiemTrungBinh(dot.Id).Result;

                            rowEnd = rowSheet2;

                            rowSheet2 += 1;
                            Sheet1.Cells["A" + rowSheet2].Value = "KẾT QUẢ:";
                            if (TinhDiemTrungBinh(dot.Id).Result > dot.IdmoDotNavigation.DiemToiDa)
                            {
                                Sheet1.Cells["B" + rowSheet2].Value = "ĐẠT";
                            }
                            else if (TinhDiemTrungBinh(dot.Id).Result < dot.IdmoDotNavigation.DiemToiThieu)
                            {
                                Sheet1.Cells["B" + rowSheet2].Value = "KHÔNG ĐẠT";
                            }
                            else
                            {
                                Sheet1.Cells["B" + rowSheet2].Value = "DUYỆT LẠI";
                            }

                            Sheet1.Cells[string.Format("A{0}:F{1}", rowStart, rowEnd)].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            Sheet1.Cells[string.Format("A{0}:F{1}", rowStart, rowEnd)].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            Sheet1.Cells[string.Format("A{0}:F{1}", rowStart, rowEnd)].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            Sheet1.Cells[string.Format("A{0}:F{1}", rowStart, rowEnd)].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            #endregion
                        }
                    }
                    else
                    {
                        Sheet1.Cells["A" + (rowSheet2)].Style.Font.Bold = true;
                        Sheet1.Cells["A" + (rowSheet2)].Value = "ĐỀ TÀI CHƯA ĐƯỢC XÉT DUYỆT";
                    }
                    

                    //Phần đánh giá nghiệm thu đề tài
                    int rowSheet3 = rowSheet2 + 2;
                    Sheet1.Cells["A" + (rowSheet3)].Style.Font.Bold = true;
                    Sheet1.Cells["A" + (rowSheet3)].Value = "ĐÁNH GIÁ NGHIỆM THU ĐỀ TÀI";

                    if (hoidongNghiemThu.Any())
                    {
                        foreach (var dot in hoidongNghiemThu)
                        {
                            #region -- Header Hội đồng nghiệm thu --
                            rowSheet3 += 2;
                            Sheet1.Cells["A" + (rowSheet3)].Value = "Hội đồng: ";
                            Sheet1.Cells["B" + (rowSheet3)].Value = dot.IdhoiDongNavigation.TenHoiDong;

                            rowSheet3 += 1;
                            int rowStart = rowSheet3;
                            int rowEnd = rowSheet3;
                            Sheet1.Cells[string.Format("A{0}:F{1}", rowSheet3, rowSheet3)].Style.Font.Bold = true;
                            Sheet1.Cells[string.Format("A{0}:F{1}", rowSheet3, rowSheet3)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            Sheet1.Cells["A" + (rowSheet3)].Value = "Thành viên";
                            Sheet1.Cells["B" + (rowSheet3)].Value = "Vai trò";
                            Sheet1.Cells["C" + (rowSheet3)].Value = "Câu hỏi";
                            Sheet1.Cells["D" + (rowSheet3)].Value = "Câu trả lời";
                            Sheet1.Cells["E" + (rowSheet3)].Value = "Nhận xét";
                            Sheet1.Cells["F" + (rowSheet3)].Value = "Điểm";
                            #endregion

                            #region -- Details Hội đồng nghiệm thu --
                            rowSheet3 += 1;
                            IEnumerable<CtxetDuyetVaDanhGia> ctXetDuyet = await _serviceCTXetDuyetVaDanhGia.GetAll(x => x.IdxetDuyetNavigation.IddeTai == id
                                                                                                                   && x.IdxetDuyetNavigation.IdmoDotNavigation.Loai == (int)MoDotLoai.NghiemThuDeTai
                                                                                                                   && x.IdxetDuyet == dot.Id);
                            foreach (var item in ctXetDuyet)
                            {
                                Sheet1.Cells[string.Format("A{0}", rowSheet3)].Value = item.IdgiangVienNavigation.Ho + " " + item.IdgiangVienNavigation.Ten;
                                if (item.VaiTro == 1)
                                {
                                    Sheet1.Cells[string.Format("B{0}", rowSheet3)].Value = "Chủ tịch";
                                }
                                else if (item.VaiTro == 2)
                                {
                                    Sheet1.Cells[string.Format("B{0}", rowSheet3)].Value = "Ủy viên";
                                }
                                else if (item.VaiTro == 3)
                                {
                                    Sheet1.Cells[string.Format("B{0}", rowSheet3)].Value = "Thư ký";
                                }
                                else
                                {
                                    Sheet1.Cells[string.Format("B{0}", rowSheet3)].Value = "Phản biện";
                                }
                                Sheet1.Cells[string.Format("C{0}", rowSheet3)].Value = item.CauHoi;
                                Sheet1.Cells[string.Format("D{0}", rowSheet3)].Value = item.CauTraLoi;
                                Sheet1.Cells[string.Format("E{0}", rowSheet3)].Value = item.NhanXet;
                                Sheet1.Cells[string.Format("F{0}", rowSheet3)].Value = item.Diem;
                                rowSheet3++;
                            }
                            Sheet1.Cells[string.Format("A{0}:E{1}", rowSheet3, rowSheet3)].Merge = true;
                            Sheet1.Cells["A" + rowSheet3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            Sheet1.Cells["A" + rowSheet3].Style.Font.Bold = true;
                            Sheet1.Cells["A" + rowSheet3].Value = "Điểm trung bình";
                            Sheet1.Cells["F" + rowSheet3].Value = TinhDiemTrungBinh(dot.Id).Result;

                            rowEnd = rowSheet3;

                            rowSheet3 += 1;
                            Sheet1.Cells["A" + rowSheet3].Value = "KẾT QUẢ:";
                            if (TinhDiemTrungBinh(dot.Id).Result > dot.IdmoDotNavigation.DiemToiDa)
                            {
                                Sheet1.Cells["B" + rowSheet3].Value = "ĐẠT";
                            }
                            else if (TinhDiemTrungBinh(dot.Id).Result < dot.IdmoDotNavigation.DiemToiThieu)
                            {
                                Sheet1.Cells["B" + rowSheet3].Value = "KHÔNG ĐẠT";
                            }
                            else
                            {
                                Sheet1.Cells["B" + rowSheet3].Value = "DUYỆT LẠI";
                            }

                            Sheet1.Cells[string.Format("A{0}:F{1}", rowStart, rowEnd)].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            Sheet1.Cells[string.Format("A{0}:F{1}", rowStart, rowEnd)].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            Sheet1.Cells[string.Format("A{0}:F{1}", rowStart, rowEnd)].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            Sheet1.Cells[string.Format("A{0}:F{1}", rowStart, rowEnd)].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            #endregion
                        }
                    }
                    else
                    {
                        Sheet1.Cells["A" + (rowSheet3)].Style.Font.Bold = true;
                        Sheet1.Cells["A" + (rowSheet3)].Value = "ĐỀ TÀI CHƯA ĐƯỢC ĐÁNH GIÁ NGHIỆM THU";
                    }
                }
                else
                {
                    Sheet1.Cells["A5"].Style.Font.Bold = true;
                    Sheet1.Cells["A5"].Value = "ĐỀ TÀI CHƯA ĐƯỢC THỰC HIỆN";
                }

                #region -- Footer --
                Sheet1.Cells["A:AZ"].AutoFitColumns();
                #endregion

                package.Save();
            }

            stream.Position = 0;
            var filename = $"BaoCaoChiTietDeTai.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }

        public async Task<IActionResult> XemLichSuDeTai()
        {
            IEnumerable<DeTaiNghienCuu> listLichSuDetai = await _service.GetAll(x => x.TinhTrangDeTai == (int)StatusDeTai.HoanThanh || x.TinhTrangDeTai == (int)StatusDeTai.Huy);
            return View(listLichSuDetai);
        }

        [HttpPost]
        public async Task<IActionResult> PhanCongGiangVien(long IdGiangVien, long IdDeTai)
        {
            DeTaiNghienCuu deTaiNghienCuu = await _service.GetById(IdDeTai);
            if(deTaiNghienCuu != null)
            {
                deTaiNghienCuu.IdgiangVien = IdGiangVien;
                await _service.Update(deTaiNghienCuu);
                return RedirectToAction("Index", new { mess = "Phân công thành công" });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<string> ThanhVienNhom(long id)
        {
            string tenThanhVien = "";
            var DeTai = await _service.GetById(id);
            var NhomSV = await _serviceNhomSV.GetAll(x => x.IddeTai == DeTai.Id);
            IEnumerable<Data.Models.SinhVien> sinhViens =  NhomSV.Select(x => x.IdsinhVienNavigation);
            foreach(var item in sinhViens)
            {
                tenThanhVien += item.Ho + " " + item.Ten + ", ";
            }
            tenThanhVien = tenThanhVien.TrimEnd(',',' ');
            return tenThanhVien;
        }

        public async Task<string> HoiDong(string loai,long id)
        {
            string tenHoiDong = "";
            if(loai == "XetDuyet")
            {
                XetDuyetVaDanhGia xetDuyetVaDanhGia = await _serviceXetDuyet.GetEntity(x => x.IddeTai == id && x.IdmoDotNavigation.Loai == (int)MoDotLoai.XetDuyetDeTai);
                if (xetDuyetVaDanhGia != null)
                {
                    tenHoiDong += xetDuyetVaDanhGia.IdhoiDongNavigation.TenHoiDong;
                }
                else
                {
                    tenHoiDong += "Chưa xét duyệt";
                }
                return tenHoiDong;
            }
            else
            {
                XetDuyetVaDanhGia xetDuyetVaDanhGia = await _serviceXetDuyet.GetEntity(x => x.IddeTai == id && x.IdmoDotNavigation.Loai == (int)MoDotLoai.NghiemThuDeTai);
                if (xetDuyetVaDanhGia != null)
                {
                    tenHoiDong += xetDuyetVaDanhGia.IdhoiDongNavigation.TenHoiDong;
                }
                else
                {
                    tenHoiDong += "Chưa đánh giá nghiệm thu";
                }
                return tenHoiDong;
            }
        }

        public async Task<double> TinhDiemTrungBinh(int id)
        {
            IEnumerable<CtxetDuyetVaDanhGia> ctXetDuyet = await _serviceCTXetDuyetVaDanhGia.GetAll(x => x.IdxetDuyet == id);
            double diemtb = 0;
            int chia = 0;
            foreach (var item in ctXetDuyet)
            {
                if (item.Diem.HasValue)
                {
                    if (item.VaiTro == (int)LoaiVaiTro.PhanBien)
                    {
                        diemtb = diemtb + (2 * item.Diem.Value);
                        chia = chia + 2;
                    }
                    else
                    {
                        diemtb = diemtb + item.Diem.Value;
                        chia++;
                    }
                }
            }
            if (chia == 0)
            {
                return  0;
            }
            else
            {
                return diemtb / chia * 1.0;
            }
        }

        public async Task<IActionResult> XemNhom(long id)
        {
            var DeTai = await _service.GetById(id);
            var NhomSV = await _serviceNhomSV.GetAll(x => x.IddeTai == DeTai.Id);
            return ViewComponent("ToggleThongTinSinhVien", NhomSV.Select(x => x.IdsinhVienNavigation));
        }
    }
}