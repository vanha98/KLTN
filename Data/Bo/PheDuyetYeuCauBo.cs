using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Bo
{
    public class PheDuyetYeuCauBo : Repository<YeuCauPheDuyet>, IPheDuyetYeuCau
    {
        public PheDuyetYeuCauBo(KLTNContext context) : base(context)
        {
        }
    }
}
