using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KayipEsyaPlatformu.Models;
using Microsoft.AspNetCore.Identity;
using KayipEsyaPlatformu.Data;
using Microsoft.EntityFrameworkCore;
using KayipEsyaPlatformu.Services;

namespace KayipEsyaPlatformu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPaneliController : Controller
    {
        private readonly UserManager<Kullanici> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly BildirimServisi _bildirimServisi;
        private readonly IletisimMesajServisi _iletisimMesajServisi;
        
        public AdminPaneliController(UserManager<Kullanici> userManager, ApplicationDbContext db, BildirimServisi bildirimServisi, IletisimMesajServisi iletisimMesajServisi)
        {
            _userManager = userManager;
            _db = db;
            _bildirimServisi = bildirimServisi;
            _iletisimMesajServisi = iletisimMesajServisi;
        }

        public IActionResult Panel()
        {             
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Panel", "KullaniciPaneli");
            }
            return View();
        }

        public PartialViewResult Kullanicilar()
        {
            return PartialView("~/Views/AdminPaneli/Partials/_Kullanicilar.cshtml");
        }
        public PartialViewResult Esyalar()
        {
            return PartialView("~/Views/AdminPaneli/Partials/_Esyalar.cshtml");
        }
        public PartialViewResult Kategoriler()
        {
            return PartialView("~/Views/AdminPaneli/Partials/_Kategoriler.cshtml");
        }
        public PartialViewResult Renkler()
        {
            return PartialView("~/Views/AdminPaneli/Partials/_Renkler.cshtml");
        }
        public async Task<PartialViewResult> Bildirimler()
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("NameIdentifier")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return PartialView("~/Views/AdminPaneli/Partials/_Bildirimler.cshtml", new List<Bildirim>());
            }
            var bildirimler = await _bildirimServisi.Getir(userId);
            return PartialView("~/Views/AdminPaneli/Partials/_Bildirimler.cshtml", bildirimler);
        }

        [HttpPost]
        public async Task<IActionResult> BildirimOkundu(int id)
        {
            var result = await _bildirimServisi.OkunduYap(id);
            if (result)
                return Json(new { success = true, message = "Bildirim okundu olarak işaretlendi." });
            return Json(new { success = false, message = "Bildirim bulunamadı veya zaten okunmuş." });
        }

        [HttpPost]
        public async Task<IActionResult> TumunuOkunduYap()
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("NameIdentifier")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Kullanıcı kimliği bulunamadı." });
            }
            var result = await _bildirimServisi.TumunuOkunduYap(userId);
            if (result)
                return Json(new { success = true, message = "Tüm bildirimler okundu olarak işaretlendi." });
            return Json(new { success = false, message = "Okunmamış bildirim bulunamadı." });
        }

        [HttpGet]
        public async Task<IActionResult> BildirimSayisi()
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("NameIdentifier")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { sayi = 0 });
            }
            var sayi = await _bildirimServisi.OkunmamisSayisi(userId);
            return Json(new { sayi });
        }

         
        [HttpGet]
        public async Task<IActionResult> HakkimizdaGetir()
        {
            var hakkimizda = await _db.Hakkimizda.FirstOrDefaultAsync();
            return Json(hakkimizda ?? new Hakkimizda());
        }

        [HttpPost]
        public async Task<IActionResult> HakkimizdaKaydet(string baslik, string icerik, string misyon, string vizyon, string degerlerimiz, string ekibimiz, string tarihce)
        {
            var hakkimizda = await _db.Hakkimizda.FirstOrDefaultAsync();
            if (hakkimizda == null)
            {
                hakkimizda = new Hakkimizda();
                _db.Hakkimizda.Add(hakkimizda);
            }

            hakkimizda.Baslik = baslik;
            hakkimizda.Icerik = icerik;
            hakkimizda.Misyon = misyon;
            hakkimizda.Vizyon = vizyon;
            hakkimizda.Degerlerimiz = degerlerimiz;
            hakkimizda.Ekibimiz = ekibimiz;
            hakkimizda.Tarihce = tarihce;
            hakkimizda.GuncellemeTarihi = DateTime.Now;

            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Hakkımızda sayfası güncellendi." });
        }

        [HttpGet]
        public async Task<IActionResult> IletisimGetir()
        {
            var iletisim = await _db.Iletisim.FirstOrDefaultAsync();
            return Json(iletisim ?? new Iletisim());
        }

        [HttpPost]
        public async Task<IActionResult> IletisimKaydet(string baslik, string icerik, string adres, string telefon, string email, string calismaSaatleri, string haritaKoordinatlari, string sosyalMedyaLinkleri, string iletisimFormuMetni)
        {
            var iletisim = await _db.Iletisim.FirstOrDefaultAsync();
            if (iletisim == null)
            {
                iletisim = new Iletisim();
                _db.Iletisim.Add(iletisim);
            }

            iletisim.Baslik = baslik;
            iletisim.Icerik = icerik;
            iletisim.Adres = adres;
            iletisim.Telefon = telefon;
            iletisim.Email = email;
            iletisim.CalismaSaatleri = calismaSaatleri;
            iletisim.HaritaKoordinatlari = haritaKoordinatlari;
            iletisim.SosyalMedyaLinkleri = sosyalMedyaLinkleri;
            iletisim.IletisimFormuMetni = iletisimFormuMetni;
            iletisim.GuncellemeTarihi = DateTime.Now;

            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "İletişim sayfası güncellendi." });
        }
        public PartialViewResult Sehirler()
        {
            return PartialView("~/Views/AdminPaneli/Partials/_Sehirler.cshtml");
        }
        public PartialViewResult Ilceler()
        {
            return PartialView("~/Views/AdminPaneli/Partials/_Ilceler.cshtml");
        }
        public PartialViewResult Mahalleler()
        {
            return PartialView("~/Views/AdminPaneli/Partials/_Mahalleler.cshtml");
        }
        public PartialViewResult Sayfalar()
        {
            return PartialView("~/Views/AdminPaneli/Partials/_Sayfalar.cshtml");
        }
         

        [HttpGet]
        public async Task<IActionResult> KullanicilariGetir()
        {
            var kullanicilar = _userManager.Users.ToList();
            var result = new List<object>();
            foreach (var k in kullanicilar)
            {
                var roller = await _userManager.GetRolesAsync(k);
                result.Add(new {
                    k.Id,
                    k.Ad,
                    k.Soyad,
                    k.Email,
                    k.PhoneNumber,
                    Roller = string.Join(", ", roller),
                    k.AktifMi
                });
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> KullaniciSil(string id)
        {
            var kullanici = await _userManager.FindByIdAsync(id);
            if (kullanici == null)
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            if (await _userManager.IsInRoleAsync(kullanici, "Admin"))
                return Json(new { success = false, message = "Admin kullanıcı silinemez." });
            var result = await _userManager.DeleteAsync(kullanici);
            if (result.Succeeded)
                return Json(new { success = true, message = "Kullanıcı silindi." });
            return Json(new { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) });
        }

        [HttpGet]
        public async Task<IActionResult> EsyalariGetir()
        {
            var esyalar = await _db.Esyalar.Include(e => e.Kategori).Include(e => e.Durum).Include(e => e.Kullanici).ToListAsync();
            var result = esyalar.Select(e => new {
                e.Id,
                e.Baslik,
                Kategori = e.Kategori != null ? e.Kategori.Ad : "",
                Durum = e.Durum != null ? e.Durum.Ad : "",
                KullaniciId = e.KullaniciId,
                KullaniciAdSoyad = e.Kullanici != null ? e.Kullanici.Ad + " " + e.Kullanici.Soyad : "",
                Tarih = e.Tarih.ToString("yyyy-MM-dd"),
            }).ToList();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> EsyaSil(int id)
        {
            var esya = await _db.Esyalar.FindAsync(id);
            if (esya == null)
                return Json(new { success = false, message = "Eşya bulunamadı." });
            _db.Esyalar.Remove(esya);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Eşya silindi." });
        }

        [HttpGet]
        public async Task<IActionResult> KullaniciDetay(string id)
        {
            var kullanici = await _userManager.FindByIdAsync(id);
            if (kullanici == null) return PartialView("Partials/_KullaniciDetay", null);
            var ilanlar = await _db.Esyalar.Where(e => e.KullaniciId == id).ToListAsync();
            var roller = await _userManager.GetRolesAsync(kullanici);
            ViewBag.Ilanlar = ilanlar;
            ViewBag.Roller = roller;
            return PartialView("Partials/_KullaniciDetay", kullanici);
        }

        [HttpGet]
        public async Task<IActionResult> EsyaDuzenle(int id)
        {
            var esya = await _db.Esyalar.FindAsync(id);
            if (esya == null) return NotFound();

            ViewBag.Kategoriler = await _db.Kategoriler.ToListAsync();
            ViewBag.Durumlar = await _db.EsyaDurumlari.ToListAsync();
            ViewBag.Renkler = await _db.Renkler.ToListAsync();
            ViewBag.Sehirler = await _db.Sehirler.ToListAsync();
            ViewBag.Ilceler = await _db.Ilceler.Where(x => x.SehirId == esya.SehirId).ToListAsync();
            ViewBag.Mahalleler = await _db.Mahalleler.Where(x => x.IlceId == esya.IlceId).ToListAsync();

            return PartialView("Partials/_EsyaEditForm", esya);
        }

         
        [HttpGet]
        public async Task<IActionResult> KategorileriGetir()
        {
            var kategoriler = await _db.Kategoriler.ToListAsync();
            var result = new List<object>();
            
            foreach (var kategori in kategoriler)
            {
                var esyaSayisi = await _db.Esyalar.CountAsync(e => e.KategoriId == kategori.Id);
                result.Add(new {
                    kategori.Id,
                    kategori.Ad,
                    EsyaSayisi = esyaSayisi
                });
            }
            
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> KategoriEkle(string ad)
        {
            if (string.IsNullOrWhiteSpace(ad))
                return Json(new { success = false, message = "Kategori adı boş olamaz." });

            var mevcutKategori = await _db.Kategoriler.FirstOrDefaultAsync(k => k.Ad.ToLower() == ad.ToLower());
            if (mevcutKategori != null)
                return Json(new { success = false, message = "Bu kategori adı zaten mevcut." });

            var kategori = new Kategori { Ad = ad.Trim() };
            _db.Kategoriler.Add(kategori);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Kategori başarıyla eklendi." });
        }

        [HttpPost]
        public async Task<IActionResult> KategoriSil(int id)
        {
            var kategori = await _db.Kategoriler.Include(k => k.Esyalar).FirstOrDefaultAsync(k => k.Id == id);
            if (kategori == null)
                return Json(new { success = false, message = "Kategori bulunamadı." });

            if (kategori.Esyalar != null && kategori.Esyalar.Any())
                return Json(new { success = false, message = "Bu kategoriye ait eşyalar bulunduğu için silinemez." });

            _db.Kategoriler.Remove(kategori);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Kategori başarıyla silindi." });
        }

        [HttpPost]
        public async Task<IActionResult> KategoriDuzenle(int id, string ad)
        {
            if (string.IsNullOrWhiteSpace(ad))
                return Json(new { success = false, message = "Kategori adı boş olamaz." });

            var kategori = await _db.Kategoriler.FindAsync(id);
            if (kategori == null)
                return Json(new { success = false, message = "Kategori bulunamadı." });

            var mevcutKategori = await _db.Kategoriler.FirstOrDefaultAsync(k => k.Ad.ToLower() == ad.ToLower() && k.Id != id);
            if (mevcutKategori != null)
                return Json(new { success = false, message = "Bu kategori adı zaten mevcut." });

            kategori.Ad = ad.Trim();
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Kategori başarıyla güncellendi." });
        }

        // Renkler için action'lar
        [HttpGet]
        public async Task<IActionResult> RenkleriGetir()
        {
            var renkler = await _db.Renkler.ToListAsync();
            var result = new List<object>();
            
            foreach (var renk in renkler)
            {
                var esyaSayisi = await _db.Esyalar.CountAsync(e => e.RenkId == renk.Id);
                result.Add(new {
                    renk.Id,
                    renk.Ad,
                    EsyaSayisi = esyaSayisi
                });
            }
            
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> RenkEkle(string ad)
        {
            if (string.IsNullOrWhiteSpace(ad))
                return Json(new { success = false, message = "Renk adı boş olamaz." });

            var mevcutRenk = await _db.Renkler.FirstOrDefaultAsync(r => r.Ad.ToLower() == ad.ToLower());
            if (mevcutRenk != null)
                return Json(new { success = false, message = "Bu renk adı zaten mevcut." });

            var renk = new Renk { Ad = ad.Trim() };
            _db.Renkler.Add(renk);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Renk başarıyla eklendi." });
        }

        [HttpPost]
        public async Task<IActionResult> RenkSil(int id)
        {
            var renk = await _db.Renkler.FirstOrDefaultAsync(r => r.Id == id);
            if (renk == null)
                return Json(new { success = false, message = "Renk bulunamadı." });

            var esyaSayisi = await _db.Esyalar.CountAsync(e => e.RenkId == id);
            if (esyaSayisi > 0)
                return Json(new { success = false, message = "Bu renge ait eşyalar bulunduğu için silinemez." });

            _db.Renkler.Remove(renk);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Renk başarıyla silindi." });
        }

        [HttpPost]
        public async Task<IActionResult> RenkDuzenle(int id, string ad)
        {
            if (string.IsNullOrWhiteSpace(ad))
                return Json(new { success = false, message = "Renk adı boş olamaz." });

            var renk = await _db.Renkler.FindAsync(id);
            if (renk == null)
                return Json(new { success = false, message = "Renk bulunamadı." });

            var mevcutRenk = await _db.Renkler.FirstOrDefaultAsync(r => r.Ad.ToLower() == ad.ToLower() && r.Id != id);
            if (mevcutRenk != null)
                return Json(new { success = false, message = "Bu renk adı zaten mevcut." });

            renk.Ad = ad.Trim();
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Renk başarıyla güncellendi." });
        }

        // Şehirler için action'lar
        [HttpGet]
        public async Task<IActionResult> SehirleriGetir()
        {
            var sehirler = await _db.Sehirler.ToListAsync();
            var result = sehirler.Select(s => new {
                s.Id,
                s.Ad,
                IlceSayisi = _db.Ilceler.Count(i => i.SehirId == s.Id)
            }).ToList();
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> SehirEkle(string ad)
        {
            if (string.IsNullOrWhiteSpace(ad))
                return Json(new { success = false, message = "Şehir adı boş olamaz." });
            var mevcut = await _db.Sehirler.FirstOrDefaultAsync(s => s.Ad.ToLower() == ad.ToLower());
            if (mevcut != null)
                return Json(new { success = false, message = "Bu şehir zaten var." });
            _db.Sehirler.Add(new Sehir { Ad = ad.Trim() });
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Şehir eklendi." });
        }
        [HttpPost]
        public async Task<IActionResult> SehirSil(int id)
        {
            var sehir = await _db.Sehirler.Include(s => s.Ilceler).FirstOrDefaultAsync(s => s.Id == id);
            if (sehir == null)
                return Json(new { success = false, message = "Şehir bulunamadı." });
            if (sehir.Ilceler != null && sehir.Ilceler.Any())
                return Json(new { success = false, message = "İlçesi olan şehir silinemez." });
            _db.Sehirler.Remove(sehir);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Şehir silindi." });
        }
        [HttpPost]
        public async Task<IActionResult> SehirDuzenle(int id, string ad)
        {
            if (string.IsNullOrWhiteSpace(ad))
                return Json(new { success = false, message = "Şehir adı boş olamaz." });
            var sehir = await _db.Sehirler.FindAsync(id);
            if (sehir == null)
                return Json(new { success = false, message = "Şehir bulunamadı." });
            var mevcut = await _db.Sehirler.FirstOrDefaultAsync(s => s.Ad.ToLower() == ad.ToLower() && s.Id != id);
            if (mevcut != null)
                return Json(new { success = false, message = "Bu şehir zaten var." });
            sehir.Ad = ad.Trim();
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Şehir güncellendi." });
        }

        // İlçeler için action'lar
        [HttpGet]
        public async Task<IActionResult> IlceleriGetir()
        {
            var ilceler = await _db.Ilceler.Include(i => i.Sehir).ToListAsync();
            var result = ilceler.Select(i => new {
                i.Id,
                i.Ad,
                SehirId = i.SehirId,
                SehirAd = i.Sehir != null ? i.Sehir.Ad : "",
                MahalleSayisi = _db.Mahalleler.Count(m => m.IlceId == i.Id)
            }).ToList();
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> IlceEkle(string ad, int sehirId)
        {
            if (string.IsNullOrWhiteSpace(ad))
                return Json(new { success = false, message = "İlçe adı boş olamaz." });
            var mevcut = await _db.Ilceler.FirstOrDefaultAsync(i => i.Ad.ToLower() == ad.ToLower() && i.SehirId == sehirId);
            if (mevcut != null)
                return Json(new { success = false, message = "Bu ilde bu ilçe zaten var." });
            _db.Ilceler.Add(new Ilce { Ad = ad.Trim(), SehirId = sehirId });
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "İlçe eklendi." });
        }
        [HttpPost]
        public async Task<IActionResult> IlceSil(int id)
        {
            var ilce = await _db.Ilceler.Include(i => i.Mahalleler).FirstOrDefaultAsync(i => i.Id == id);
            if (ilce == null)
                return Json(new { success = false, message = "İlçe bulunamadı." });
            if (ilce.Mahalleler != null && ilce.Mahalleler.Any())
                return Json(new { success = false, message = "Mahallesi olan ilçe silinemez." });
            _db.Ilceler.Remove(ilce);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "İlçe silindi." });
        }
        [HttpPost]
        public async Task<IActionResult> IlceDuzenle(int id, string ad, int sehirId)
        {
            if (string.IsNullOrWhiteSpace(ad))
                return Json(new { success = false, message = "İlçe adı boş olamaz." });
            var ilce = await _db.Ilceler.FindAsync(id);
            if (ilce == null)
                return Json(new { success = false, message = "İlçe bulunamadı." });
            var mevcut = await _db.Ilceler.FirstOrDefaultAsync(i => i.Ad.ToLower() == ad.ToLower() && i.SehirId == sehirId && i.Id != id);
            if (mevcut != null)
                return Json(new { success = false, message = "Bu ilde bu ilçe zaten var." });
            ilce.Ad = ad.Trim();
            ilce.SehirId = sehirId;
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "İlçe güncellendi." });
        }

        // Mahalleler için action'lar
        [HttpGet]
        public async Task<IActionResult> MahalleleriGetir()
        {
            var mahalleler = await _db.Mahalleler.Include(m => m.Sehir).Include(m => m.Ilce).ToListAsync();
            var result = mahalleler.Select(m => new {
                m.Id,
                m.Ad,
                SehirId = m.SehirId,
                SehirAd = m.Sehir != null ? m.Sehir.Ad : "",
                IlceId = m.IlceId,
                IlceAd = m.Ilce != null ? m.Ilce.Ad : ""
            }).ToList();
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> MahalleEkle(string ad, int sehirId, int ilceId)
        {
            if (string.IsNullOrWhiteSpace(ad))
                return Json(new { success = false, message = "Mahalle adı boş olamaz." });
            var mevcut = await _db.Mahalleler.FirstOrDefaultAsync(m => m.Ad.ToLower() == ad.ToLower() && m.SehirId == sehirId && m.IlceId == ilceId);
            if (mevcut != null)
                return Json(new { success = false, message = "Bu il ve ilçede bu mahalle zaten var." });
            _db.Mahalleler.Add(new Mahalle { Ad = ad.Trim(), SehirId = sehirId, IlceId = ilceId });
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Mahalle eklendi." });
        }
        [HttpPost]
        public async Task<IActionResult> MahalleSil(int id)
        {
            var mahalle = await _db.Mahalleler.FindAsync(id);
            if (mahalle == null)
                return Json(new { success = false, message = "Mahalle bulunamadı." });
            _db.Mahalleler.Remove(mahalle);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Mahalle silindi." });
        }
        [HttpPost]
        public async Task<IActionResult> MahalleDuzenle(int id, string ad, int sehirId, int ilceId)
        {
            if (string.IsNullOrWhiteSpace(ad))
                return Json(new { success = false, message = "Mahalle adı boş olamaz." });
            var mahalle = await _db.Mahalleler.FindAsync(id);
            if (mahalle == null)
                return Json(new { success = false, message = "Mahalle bulunamadı." });
            var mevcut = await _db.Mahalleler.FirstOrDefaultAsync(m => m.Ad.ToLower() == ad.ToLower() && m.SehirId == sehirId && m.IlceId == ilceId && m.Id != id);
            if (mevcut != null)
                return Json(new { success = false, message = "Bu il ve ilçede bu mahalle zaten var." });
            mahalle.Ad = ad.Trim();
            mahalle.SehirId = sehirId;
            mahalle.IlceId = ilceId;
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Mahalle güncellendi." });
        }

        // Dinamik ilçe ve mahalle getirme
        [HttpGet]
        public async Task<IActionResult> IlceleriGetirBySehir(int sehirId)
        {
            var ilceler = await _db.Ilceler.Where(i => i.SehirId == sehirId).ToListAsync();
            var result = ilceler.Select(i => new { i.Id, i.Ad }).ToList();
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> MahalleleriGetirByIlce(int ilceId)
        {
            var mahalleler = await _db.Mahalleler.Where(m => m.IlceId == ilceId).ToListAsync();
            var result = mahalleler.Select(m => new { m.Id, m.Ad }).ToList();
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> RolleriGetir()
        {
            var roller = await _db.Roles.ToListAsync();
            var result = roller.Select(r => new { r.Id, r.Name, ad = r.Name }).ToList();
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> KullaniciDuzenle(string kullaniciId, string yeniRol)
        {
            var kullanici = await _userManager.FindByIdAsync(kullaniciId);
            if (kullanici == null)
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            
            try
            {
                var mevcutRoller = await _userManager.GetRolesAsync(kullanici);
                await _userManager.RemoveFromRolesAsync(kullanici, mevcutRoller);
                await _userManager.AddToRoleAsync(kullanici, yeniRol);
                await _userManager.UpdateAsync(kullanici);
                return Json(new { success = true, message = "Kullanıcı rolü güncellendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Rol güncellenirken hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IletisimMesajGonder(string adSoyad, string email, string konu, string mesaj)
        {
            if (string.IsNullOrWhiteSpace(adSoyad) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(konu) || string.IsNullOrWhiteSpace(mesaj))
            {
                return Json(new { success = false, message = "Tüm alanları doldurmanız gerekmektedir." });
            }
            var yeniMesaj = new IletisimMesaj
            {
                AdSoyad = adSoyad,
                Email = email,
                Konu = konu,
                Mesaj = mesaj,
                Tarih = DateTime.Now,
                OkunduMu = false
            };
            await _iletisimMesajServisi.EkleAsync(yeniMesaj);
            return Json(new { success = true, message = "Mesajınız başarıyla gönderildi!" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> IletisimMesajlariGetir()
        {
            var mesajlar = await _iletisimMesajServisi.TumunuGetirAsync();
            return Json(mesajlar);
        }

        [Authorize(Roles = "Admin")]
        public PartialViewResult IletisimMesajlari()
        {
            var mesajlar = _iletisimMesajServisi.TumunuGetirAsync().Result;
            return PartialView("~/Views/AdminPaneli/Partials/_IletisimMesajlari.cshtml", mesajlar);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> MesajDurumToggle(int id, bool okunduMu)
        {
            var mesaj = await _db.IletisimMesajlari.FindAsync(id);
            if (mesaj == null)
                return Json(new { success = false });
            mesaj.OkunduMu = !okunduMu;
            await _db.SaveChangesAsync();
            return Json(new { success = true, yeniDurum = mesaj.OkunduMu });
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> MesajDetay(int id)
        {
            var mesaj = await _iletisimMesajServisi.GetirAsync(id);
            if (mesaj == null)
                return Json(new { mesaj = "Mesaj bulunamadı!" });
            return Json(new { mesaj = mesaj.Mesaj });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> MesajOkunduYap(int id)
        {
            var mesaj = await _db.IletisimMesajlari.FindAsync(id);
            if (mesaj != null && !mesaj.OkunduMu)
            {
                mesaj.OkunduMu = true;
                await _db.SaveChangesAsync();
            }
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> MesajSil(int id)
        {
            var mesaj = await _db.IletisimMesajlari.FindAsync(id);
            if (mesaj == null)
                return Json(new { success = false });
            _db.IletisimMesajlari.Remove(mesaj);
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> KullaniciMesajlari(string kullaniciId)
        {
            var eslesmeler = await _db.Eslesmeler
                .Include(e => e.IlkEsya).ThenInclude(e => e.Kullanici)
                .Include(e => e.EslesenEsya).ThenInclude(e => e.Kullanici)
                .Where(e => e.IlkEsya.KullaniciId == kullaniciId || e.EslesenEsya.KullaniciId == kullaniciId)
                .OrderByDescending(e => e.EslesmeTarihi)
                .ToListAsync();

            var okunmamislar = _db.Mesajlar
                .Where(m => m.AliciId == kullaniciId && !m.OkunduMu)
                .GroupBy(m => m.EslesmeId)
                .Select(g => new { EslesmeId = g.Key, Sayi = g.Count() })
                .ToDictionary(x => x.EslesmeId, x => x.Sayi);
            ViewBag.Okunmamislar = okunmamislar;

            return PartialView("~/Views/KullaniciPaneli/Partials/_Mesajlarim.cshtml", eslesmeler);
        }

        [Authorize(Roles = "Admin")]
        public async Task<PartialViewResult> Mesajlar()
        {
            var eslesmeler = await _db.Eslesmeler
                .Include(e => e.IlkEsya).ThenInclude(e => e.Kullanici)
                .Include(e => e.EslesenEsya).ThenInclude(e => e.Kullanici)
                .OrderByDescending(e => e.EslesmeTarihi)
                .ToListAsync();

            var okunmamislar = _db.Mesajlar
                .Where(m => !m.OkunduMu)
                .GroupBy(m => m.EslesmeId)
                .Select(g => new { EslesmeId = g.Key, Sayi = g.Count() })
                .ToDictionary(x => x.EslesmeId, x => x.Sayi);
            ViewBag.Okunmamislar = okunmamislar;

            return PartialView("~/Views/KullaniciPaneli/Partials/_Mesajlarim.cshtml", eslesmeler);
        }




    }
} 