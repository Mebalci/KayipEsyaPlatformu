using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using KayipEsyaPlatformu.Data;
using KayipEsyaPlatformu.Models;

namespace KayipEsyaPlatformu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ilkKartlar = await _context.Esyalar
                .Include(e => e.Kategori)
                .Include(e => e.Durum)
                .OrderByDescending(e => e.Tarih)
                .Take(6)
                .Select(e => new KayipBulunanEsyaViewModel
                {
                    Id = e.Id,
                    Baslik = e.Baslik,
                    Aciklama = e.Aciklama,
                    Konum = e.Konum,
                    Tarih = e.Tarih,
                    Kategori = e.Kategori.Ad,
                    Durum = e.Durum.Ad,
                    BulunduMu = e.Durum.Ad.ToLower() == "bulundu",
                    ResimYolu = e.ResimYolu
                }).ToListAsync();

            return View(ilkKartlar);
        }

        [HttpGet]
        public async Task<IActionResult> EsyalariGetir(int baslangic = 0, int adet = 6)
        {
            var esyalar = await _context.Esyalar
                .Include(e => e.Kategori)
                .Include(e => e.Durum)
                .OrderByDescending(e => e.Tarih)
                .Skip(baslangic)
                .Take(adet)
                .Select(e => new KayipBulunanEsyaViewModel
                {
                    Id = e.Id,
                    Baslik = e.Baslik,
                    Aciklama = e.Aciklama.Length > 100 ? e.Aciklama.Substring(0, 100) + "..." : e.Aciklama,
                    Konum = e.Konum,
                    Tarih = e.Tarih,
                    Kategori = e.Kategori.Ad,
                    Durum = e.Durum.Ad,
                    BulunduMu = e.Durum.Ad != null && e.Durum.Ad.ToLower() == "bulundu",
                    ResimYolu = e.ResimYolu
                })
                .ToListAsync();

            return Json(esyalar);
        }

        [HttpGet]
        public async Task<IActionResult> EsyaAra(string arama, int baslangic = 0, int adet = 6)
        {
            if (string.IsNullOrWhiteSpace(arama))
            {
                // Eğer arama boşsa, normal EsyalariGetir gibi davran
                return await EsyalariGetir(baslangic, adet);
            }

            var esyalar = await _context.Esyalar
                .Include(e => e.Kategori)
                .Include(e => e.Durum)
                .Where(e => e.Baslik.Contains(arama) || e.Aciklama.Contains(arama))
                .OrderByDescending(e => e.Tarih)
                .Skip(baslangic)
                .Take(adet)
                .Select(e => new KayipBulunanEsyaViewModel
                {
                    Id = e.Id,
                    Baslik = e.Baslik,
                    Aciklama = e.Aciklama.Length > 100 ? e.Aciklama.Substring(0, 100) + "..." : e.Aciklama,
                    Konum = e.Konum,
                    Tarih = e.Tarih,
                    Kategori = e.Kategori.Ad,
                    Durum = e.Durum.Ad,
                    BulunduMu = e.Durum.Ad != null && e.Durum.Ad.ToLower() == "bulundu",
                    ResimYolu = e.ResimYolu
                })
                .ToListAsync();

            return Json(esyalar);
        }


        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
