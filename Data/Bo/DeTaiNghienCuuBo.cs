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
    public class DeTaiNghienCuuBo : Repository<DeTaiNghienCuu>, IDeTaiNghienCuu
    {
        public DeTaiNghienCuuBo(KLTNContext context) : base(context)
        {
        }

        public long KhoiTaoMa(string maht)
        {
            string nam = DateTime.Now.Year.ToString();
            string kq = nam + maht;
            return long.Parse(kq);
        }
    }
}
