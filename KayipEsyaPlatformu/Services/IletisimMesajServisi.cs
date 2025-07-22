using KayipEsyaPlatformu.Data;
using KayipEsyaPlatformu.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KayipEsyaPlatformu.Services
{
    public class IletisimMesajServisi
    {
        private readonly ApplicationDbContext _db;
        public IletisimMesajServisi(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task EkleAsync(IletisimMesaj mesaj)
        {
            _db.IletisimMesajlari.Add(mesaj);
            await _db.SaveChangesAsync();
        }

        public async Task<List<IletisimMesaj>> TumunuGetirAsync()
        {
            return await _db.IletisimMesajlari.OrderByDescending(m => m.Tarih).ToListAsync();
        }

        public async Task<IletisimMesaj?> GetirAsync(int id)
        {
            return await _db.IletisimMesajlari.FindAsync(id);
        }

        public async Task OkunduYapAsync(int id)
        {
            var mesaj = await _db.IletisimMesajlari.FindAsync(id);
            if (mesaj != null && !mesaj.OkunduMu)
            {
                mesaj.OkunduMu = true;
                await _db.SaveChangesAsync();
            }
        }
    }
} 