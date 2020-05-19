using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Bo
{
    public class BaoCaoTienDoBo : Repository<BaoCaoTienDo>, IBaoCaoTienDo
    {
        public BaoCaoTienDoBo(KLTNContext context) : base(context)
        {
        }


    }
}
