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

        public long KhoiTaoMa(DeTaiNghienCuu deTaiNghienCuu)
        {
            long id = deTaiNghienCuu.Id;
            string nam = id.ToString().Substring(0, 4);// xac dinh 4 ki tu dau
            int stt = int.Parse(id.ToString().Substring(4));
            string namht = DateTime.Now.Year.ToString();
            if (nam == namht && stt != 999)
                return id + 1;
            else
            {
                if (stt == 999)
                    return long.Parse(namht + "001");
                stt++;
                if (stt >= 10)
                    return long.Parse(namht + "0" + stt.ToString());
                else if (stt >= 100)
                    return long.Parse(namht + stt.ToString());
                else
                    return long.Parse(namht + "00" + stt.ToString());
            }
            
        }
    }
}
