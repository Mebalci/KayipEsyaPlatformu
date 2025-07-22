using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KayipEsyaPlatformu.Data;
using KayipEsyaPlatformu.Models;

namespace KayipEsyaPlatformu.Controllers
{
    public class SayfaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SayfaController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Hakkimizda()
        {
            var hakkimizda = await _db.Hakkimizda.FirstOrDefaultAsync();
            if (hakkimizda == null)
            {                
                hakkimizda = new Hakkimizda
                {
                    Baslik = "Hakkımızda",
                    Icerik = "Kayıp Eşya Platformu olarak, kayıp eşyaların sahiplerine ulaştırılması konusunda hizmet vermekteyiz.",
                    Misyon = "Kayıp eşyaları bulmak ve sahiplerine ulaştırmak",
                    Vizyon = "Türkiye'nin en güvenilir kayıp eşya platformu olmak",
                    Degerlerimiz = "Güvenilirlik, Şeffaflık, Hızlı Hizmet",
                    Ekibimiz = "Deneyimli ve uzman ekibimizle hizmetinizdeyiz",
                    Tarihce = "2024 yılında kurulan platformumuz, binlerce kayıp eşyayı sahiplerine ulaştırmıştır."
                };
            }
            return View(hakkimizda);
        }

        public async Task<IActionResult> Iletisim()
        {
            var iletisim = await _db.Iletisim.FirstOrDefaultAsync();
            if (iletisim == null)
            {                
                iletisim = new Iletisim
                {
                    Baslik = "İletişim",
                    Icerik = "Bizimle iletişime geçmek için aşağıdaki bilgileri kullanabilirsiniz.",
                    Adres = "İstanbul, Türkiye",
                    Telefon = "+90 (538) 080 05 49",
                    Email = "info@kayipesyaplatformu.com",
                    CalismaSaatleri = "Pazartesi - Cuma: 09:00 - 18:00",
                    HaritaKoordinatlari = "41.0082,28.9784",
                    SosyalMedyaLinkleri = "{\"facebook\":\"https://facebook.com/kayipesya\",\"twitter\":\"https://twitter.com/kayipesya\",\"instagram\":\"https://instagram.com/kayipesya\"}",
                    IletisimFormuMetni = "Sorularınız için aşağıdaki formu doldurabilirsiniz."
                };
            }
            return View(iletisim);
        }
    }
} 