using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Bo
{
    public class NhomSinhVienBo : Repository<NhomSinhVien>, INhomSinhVien
    {
        public NhomSinhVienBo(KLTNContext context) : base(context)
        {
        }
    }
}
