using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Bo
{
    public class BoNhiemBo : Repository<BoNhiem>, IBoNhiem
    {
        public BoNhiemBo(KLTNContext context) : base(context)
        {
        }
    }
}
