using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Bo
{
    public class MoDotBo : Repository<MoDot>, IMoDot
    {
        private readonly KLTNContext _context;
        public MoDotBo(KLTNContext context) : base(context)
        {
            _context = context;
        }
    }
}
