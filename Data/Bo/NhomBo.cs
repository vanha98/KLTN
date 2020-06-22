using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Bo
{
    public class NhomBo : Repository<Nhom>, INhom
    {
        public NhomBo(KLTNContext context) : base(context)
        {
        }
    }
}
