using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Enum
{
    public static class MessageResult
    {
        public const string CreateSuccess = "Thêm mới thành công!";
        public const string UpdateSuccess = "Cập nhật thành công!";
        public const string NotFoundObject = "Không tìm thấy đối tượng";
        public const string AccessDenied = "Không có quyền thực hiện thao tác này";
        public const string Fail = "Xử lý thất bại";
        public const string NotSelectDeTai = "Chưa chọn đề tài";
        public const string UpLoadFileFail = "Tệp sai định dạng hoặc kích thước quá lớn";
    }
}
