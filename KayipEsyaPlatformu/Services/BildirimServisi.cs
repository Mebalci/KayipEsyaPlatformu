using KayipEsyaPlatformu.Data;
using KayipEsyaPlatformu.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace KayipEsyaPlatformu.Services
{
    public class BildirimServisi
    {
        private readonly IHubContext<BildirimHub> _hub;
        private readonly ApplicationDbContext _db;

        public BildirimServisi(IHubContext<BildirimHub> hub, ApplicationDbContext db)
        {
            _hub = hub;
            _db = db;
        }

        public async Task Gonder(string kullaniciId, string mesaj)
        {            
            var bildirim = new Bildirim
            {
                KullaniciId = kullaniciId,
                Icerik = mesaj,
                OkunduMu = false,
                Tarih = DateTime.Now
            };

            _db.Bildirimler.Add(bildirim);
            await _db.SaveChangesAsync();

            await _hub.Clients.Group(kullaniciId).SendAsync("BildirimAl", new
            {
                id = bildirim.Id,
                icerik = bildirim.Icerik,
                tarih = bildirim.Tarih.ToString("dd.MM.yyyy HH:mm"),
                okunduMu = bildirim.OkunduMu
            });
        }

        public async Task<List<Bildirim>> Getir(string kullaniciId)
        {
            return await _db.Bildirimler
                .Where(b => b.KullaniciId == kullaniciId)
                .OrderByDescending(b => b.Tarih)
                .ToListAsync();
        }

        public async Task<bool> OkunduYap(int id)
        {
            try
            {
                var bildirim = await _db.Bildirimler.FindAsync(id);
                if (bildirim != null && !bildirim.OkunduMu)
                {
                    bildirim.OkunduMu = true;
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {                 
                Console.WriteLine($"BildirimOkunduYap hatası: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> TumunuOkunduYap(string kullaniciId)
        {
            try
            {
                var okunmayanlar = await _db.Bildirimler
                    .Where(b => b.KullaniciId == kullaniciId && !b.OkunduMu)
                    .ToListAsync();

                if (okunmayanlar.Any())
                {
                    foreach (var bildirim in okunmayanlar)
                    {
                        bildirim.OkunduMu = true;
                    }
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {                 
                Console.WriteLine($"TumunuOkunduYap hatası: {ex.Message}");
                return false;
            }
        }

        public async Task<int> OkunmamisSayisi(string kullaniciId)
        {
            return await _db.Bildirimler
                .CountAsync(b => b.KullaniciId == kullaniciId && !b.OkunduMu);
        }



    }

}
