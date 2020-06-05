using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Bo
{
    public class KenhThaoLuanBo : Repository<KenhThaoLuan>, IKenhThaoLuan
    {
        public KenhThaoLuanBo(KLTNContext context) : base(context)
        {
        }
    }
}
