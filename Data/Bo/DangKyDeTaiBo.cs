using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Bo
{
    public class DangKyDeTaiBo : Repository<DeTaiNghienCuu>, IDangKyDeTai
    {

        public DangKyDeTaiBo(KLTNContext context) : base(context)
        {
        }
    }
}
