using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Bo
{
    public class DeTaiNghienCuuBo : Repository<DeTaiNghienCuu>, IDeTaiNghienCuu
    {

        public DeTaiNghienCuuBo(KLTNContext context) : base(context)
        {
        }
    }
}
