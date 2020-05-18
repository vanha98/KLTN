using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Bo
{
    public class GiangVienBo : Repository<GiangVien>, IGiangVien
    {
        public GiangVienBo(KLTNContext context) : base(context)
        {
        }
    }
}
