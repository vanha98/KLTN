using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Bo
{
    public class XetDuyetVaDanhGiaBo : Repository<XetDuyetVaDanhGia>, IXetDuyetVaDanhGia
    {
        private readonly KLTNContext _context;
        public XetDuyetVaDanhGiaBo(KLTNContext context) : base(context)
        {
            _context = context;
        }
    }
}
