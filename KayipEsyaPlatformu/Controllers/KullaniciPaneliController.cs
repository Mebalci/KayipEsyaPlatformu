using KayipEsyaPlatformu.Data;
using KayipEsyaPlatformu.Models;
using KayipEsyaPlatformu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KayipEsyaPlatformu.Controllers
{
    [Authorize]
    public class KullaniciPaneliController : Controller
    {
        private readonly BildirimServisi _bildirimServisi;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Kullanici> _userManager;
        private readonly SignInManager<Kullanici> _signInManager;
        private readonly IEmailSender _emailSender;
        public KullaniciPaneliController(BildirimServisi bildirimServisi, 
            ApplicationDbContext db, 
            UserManager<Kullanici> userManager,
            SignInManager<Kullanici> signInManager,
            IEmailSender emailSender)
        {
            _bildirimServisi = bildirimServisi;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public IActionResult Panel()
        {
            return View();
        }

        public async Task<PartialViewResult?> Profil()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return null;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;
            var model = new ProfilViewModel
            {
                Id = user.Id,
                Ad = user.Ad,
                Soyad = user.Soyad,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AktifMi = user.AktifMi
            };
            return PartialView("Partials/_Profil", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfilGuncelle(ProfilViewModel model)
        {
            if (!ModelState.IsValid)
            {                 
                var hata = ModelState.Values.SelectMany(x => x.Errors).FirstOrDefault()?.ErrorMessage ?? "Bilinmeyen hata.";
                return Json(new { success = false, message = hata });
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });

             
            if (user.Email != model.Email)
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                var result = await _userManager.ChangeEmailAsync(user, model.Email, token);

                if (!result.Succeeded)
                    return Json(new { success = false, message = "E-posta değiştirilemedi: " + string.Join("<br>", result.Errors.Select(x => x.Description)) });

                 
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationUrl = Url.Action("ConfirmEmail", "Account",
                    new { userId = user.Id, token = confirmationToken }, Request.Scheme);

                 
                await _emailSender.SendEmailAsync(model.Email, "E-posta Adresinizi Doğrulayın",
                    $"E-posta adresinizi doğrulamak için <a href='{confirmationUrl}'>buraya tıklayın</a>");

                 
                await _signInManager.SignOutAsync();
                return Json(new { success = true, message = "E-posta değiştirildi. Lütfen e-posta adresinizi doğrulayın ve tekrar giriş yapın.", reload = true });
            }

             
            user.Ad = model.Ad;
            user.Soyad = model.Soyad;
            user.PhoneNumber = model.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
                return Json(new { success = true, message = "Profil başarıyla güncellendi!" });

            return Json(new { success = false, message = string.Join("<br>", updateResult.Errors.Select(x => x.Description)) });
        }
                
        public PartialViewResult Sifre()
        {
            var model = new ChangePasswordViewModel();
            return PartialView("Partials/_Sifre");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SifreGuncelle(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var hata = ModelState.Values.SelectMany(x => x.Errors).FirstOrDefault()?.ErrorMessage ?? "Bilinmeyen hata.";
                return Json(new { success = false, message = hata });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
                return Json(new { success = false, message = "Mevcut şifre bulunamadı." });

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                var errors = string.Join("<br>", changePasswordResult.Errors.Select(x => x.Description));
                return Json(new { success = false, message = errors });
            }

            await _signInManager.RefreshSignInAsync(user);
            return Json(new { success = true, message = "Şifreniz başarıyla değiştirildi." });
        }
        public async Task<PartialViewResult?> Esyalarim()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return null;
            var esyalar = await _db.Esyalar
                .Include(e => e.Durum)
                .Where(e => e.KullaniciId == userId)
                .OrderByDescending(e => e.Tarih)
                .ToListAsync();

            return PartialView("Partials/_Esyalarim", esyalar);
        }

        [HttpPost]
        public async Task<IActionResult> EsyaSil(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var esya = await _db.Esyalar.FirstOrDefaultAsync(e => e.Id == id && e.KullaniciId == userId);
            if (esya == null) return NotFound();
            _db.Esyalar.Remove(esya);
            await _db.SaveChangesAsync();
            return Ok();

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

        [HttpPost]
        public async Task<IActionResult> EsyaDuzenleKaydet(IFormCollection form)
        {
            int id = int.TryParse(form["Id"], out var _id) ? _id : 0;
            var esya = await _db.Esyalar.FindAsync(id);
            if (esya == null) return NotFound();

            esya.Baslik = form["Baslik"];
            esya.Aciklama = form["Aciklama"];
            esya.KategoriId = int.TryParse(form["KategoriId"], out var kategoriId) ? kategoriId : 0;
            esya.DurumId = int.TryParse(form["DurumId"], out var durumId) ? durumId : 0;
            esya.RenkId = int.TryParse(form["RenkId"], out var renkId) ? renkId : 0;
            esya.Marka = form["Marka"];
            esya.Model = form["Model"];
            esya.Konum = form["Konum"];
            esya.Enlem = double.TryParse(form["Enlem"].ToString().Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var enlem) ? enlem : 0;
            esya.Boylam = double.TryParse(form["Boylam"].ToString().Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var boylam) ? boylam : 0;
            esya.SehirId = int.TryParse(form["SehirId"], out var sehirId) ? sehirId : 0;
            esya.IlceId = int.TryParse(form["IlceId"], out var ilceId) ? ilceId : 0;
            esya.MahalleId = int.TryParse(form["MahalleId"], out var mahalleId) ? mahalleId : 0;
            if (!string.IsNullOrEmpty(form["Tarih"]))
                esya.Tarih = DateTime.TryParse(form["Tarih"], out var tarih) ? tarih : DateTime.Now;
             
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var path = Path.Combine("wwwroot/uploads", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);
                esya.ResimYolu = "/uploads/" + fileName;
            }
            await _db.SaveChangesAsync();
            return Ok(new { success = true, message = "Eşya başarıyla güncellendi!" });
        }


        public async Task<PartialViewResult?> Bildirimler()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return null;
            var bildirimler = await _bildirimServisi.Getir(userId);
            return PartialView("Partials/_Bildirimler", bildirimler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BildirimOkundu(int id)
        {
            try
            {
                var result = await _bildirimServisi.OkunduYap(id);
                if (result)
                {
                    return Json(new { success = true, message = "Bildirim okundu olarak işaretlendi." });
                }
                return Json(new { success = false, message = "Bildirim bulunamadı veya zaten okunmuş." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TumunuOkunduYap()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Json(new { success = false, message = "Kullanıcı bulunamadı." });
                var result = await _bildirimServisi.TumunuOkunduYap(userId!);

                if (result)
                {
                    return Json(new { success = true, message = "Tüm bildirimler okundu olarak işaretlendi." });
                }
                return Json(new { success = false, message = "Okunmamış bildirim bulunamadı." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> BildirimSayisi()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Json(new { okunmamis = 0 });
            var sayi = await _bildirimServisi.OkunmamisSayisi(userId);
            return Json(new { okunmamis = sayi });
        }

        public async Task<PartialViewResult?> Mesajlarim()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return null;
            var eslesmeler = await _db.Eslesmeler
                .Include(e => e.IlkEsya).ThenInclude(e => e.Kullanici)
                .Include(e => e.EslesenEsya).ThenInclude(e => e.Kullanici)
                .Where(e => e.IlkEsya.KullaniciId == userId || e.EslesenEsya.KullaniciId == userId)
                .OrderByDescending(e => e.EslesmeTarihi)
                .ToListAsync();
            var okunmamislar = _db.Mesajlar
                .Where(m => m.AliciId == userId && !m.OkunduMu)
                .GroupBy(m => m.EslesmeId)
                .Select(g => new { EslesmeId = g.Key, Sayi = g.Count() })
                .ToDictionary(x => x.EslesmeId, x => x.Sayi);
            ViewBag.Okunmamislar = okunmamislar;
            return PartialView("Partials/_Mesajlarim", eslesmeler);
        }


    }
}
