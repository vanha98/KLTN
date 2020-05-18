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
    public class BaiPostBo : Repository<BaiPost>, IBaiPost
    {
        private readonly KLTNContext _context;
        public BaiPostBo(KLTNContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Comments>> GetAllComments(int idbaipost)
        {
            return await _context.Comments.Where(x => x.IdbaiPost == idbaipost).ToListAsync();
        }
    }
}
