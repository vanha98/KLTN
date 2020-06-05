using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Bo
{
    public class ImgBaiPostBo : Repository<ImgBaiPost>, IImgBaiPost
    {
        public ImgBaiPostBo(KLTNContext context) : base(context)
        {
        }
    }
}
