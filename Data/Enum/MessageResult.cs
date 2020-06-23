using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Enum
{
    public static class MessageResult
    {
        public const string CreateSuccess = "Thêm mới thành công!";
        public const string RegisterSuccess = "Đăng ký thành công!";
        public const string UpdateSuccess = "Cập nhật thành công!";

        public const string NotFoundObject = "Không tìm thấy đối tượng";
        public const string NotFoundSV = "Không tìm thấy sinh viên";

        public const string AccessDenied = "Không có quyền thực hiện thao tác này";
        public const string Fail = "Xử lý thất bại";
        public const string NotSelectDeTai = "Chưa chọn đề tài";
        public const string UpLoadFileFail = "Tệp sai định dạng hoặc kích thước quá lớn";

        public const string ExistDeTai = "Sinh viên này đã đăng ký đề tài";
        public const string ChuaDenTGThucHien = "Chưa đến thời gian thực hiện đề tài";
        public const string DaNopBaoCao = "Bạn đã nộp báo cáo của tuần ";
    }

    public static class DefaultValue
    {
        public const long IddeTaiNghienCuu = 1111111;
        
    }
}
