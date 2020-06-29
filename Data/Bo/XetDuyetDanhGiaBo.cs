using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Bo
{
    public class XetDuyetDanhGiaBo : Repository<XetDuyetVaDanhGia>, IXetDuyetDanhGia
    {
        public XetDuyetDanhGiaBo(KLTNContext context) : base(context)
        {
        }
    }
}
