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

        public async Task<Nhom> GetGroupInfo(int idgroup)
        {
            return await _context.Nhom.FindAsync(idgroup);
        }

        public async Task<IEnumerable<SinhVien>> GetMemberInfo(int idgroup)
        {
            return await GetListMemberFrom(idgroup);
        }

        private async Task<List<SinhVien>> GetListMemberFrom(int idgroup)
        {
            List<NhomSinhVien> nhomSinhViens = await _context.NhomSinhVien.Where(x => x.Idnhom == idgroup).ToListAsync();
            List<SinhVien> listSV = new List<SinhVien>();

            foreach (var item in nhomSinhViens)
            {
                SinhVien sinhVien =  await _context.SinhVien.FindAsync(item.IdsinhVien);
                listSV.Add(sinhVien);
            }

            return listSV;
        }
    }
}
