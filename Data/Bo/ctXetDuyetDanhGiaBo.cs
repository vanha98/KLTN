using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Bo
{
    public class CtXetDuyetDanhGiaBo : Repository<CtxetDuyetVaDanhGia>, IctXetDuyetDanhGia
    {
        public CtXetDuyetDanhGiaBo(KLTNContext context) : base(context)
        {
        }
    }
}
