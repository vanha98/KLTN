﻿using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Bo
{
    public class HoiDongBo : Repository<HoiDong>, IHoiDong
    {
        public HoiDongBo(KLTNContext context) : base(context)
        {
        }
    }
}
