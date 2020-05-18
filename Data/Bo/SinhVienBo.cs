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
    public class SinhVienBo : Repository<SinhVien>, ISinhVien
    {
        private readonly KLTNContext _context;

        public SinhVienBo(KLTNContext context) : base(context)
        {
            _context = context;
        }

        public async Task<NhomSv> GetGroupInfo(int idgroup)
        {
            return await _context.NhomSv.FindAsync(idgroup);
        }

        public async Task<IEnumerable<SinhVien>> GetMemberInfo(int idgroup)
        {
            return await _context.SinhVien.Where(x => x.Idnhom == idgroup).ToListAsync();
        }
    }
}
