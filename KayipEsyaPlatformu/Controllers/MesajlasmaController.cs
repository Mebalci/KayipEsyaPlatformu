using KayipEsyaPlatformu.Data;
using KayipEsyaPlatformu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class MesajlasmaController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<Kullanici> _userManager;
    private readonly IHubContext<BildirimHub> _hubContext;

    public MesajlasmaController(ApplicationDbContext db, UserManager<Kullanici> userManager, IHubContext<BildirimHub> hubContext)
    {
        _db = db;
        _userManager = userManager;
        _hubContext = hubContext;
    }

    // Belirli bir eşleşmenin mesajlarını getir
    [HttpGet]
    public async Task<IActionResult> EslesmeMesajlari(int eslesmeId)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        var eslesme = await _db.Eslesmeler
            .Include(e => e.IlkEsya)
            .Include(e => e.EslesenEsya)
            .FirstOrDefaultAsync(e => e.Id == eslesmeId);
        if (eslesme == null)
            return Unauthorized();
        if (eslesme.IlkEsya.KullaniciId != userId && eslesme.EslesenEsya.KullaniciId != userId)
            return Forbid();
        var mesajlar = await _db.Mesajlar
            .Where(m => m.EslesmeId == eslesmeId)
            .OrderBy(m => m.Tarih)
            .ToListAsync();
        return Json(mesajlar);
    }

    // Belirli bir eşleşmeye mesaj gönder
    [HttpPost]
    public async Task<IActionResult> MesajGonder(int eslesmeId, string icerik)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        var eslesme = await _db.Eslesmeler
            .Include(e => e.IlkEsya).ThenInclude(e => e.Kullanici)
            .Include(e => e.EslesenEsya).ThenInclude(e => e.Kullanici)
            .FirstOrDefaultAsync(e => e.Id == eslesmeId);
        if (eslesme == null)
            return Unauthorized();
        if (eslesme.IlkEsya.KullaniciId != userId && eslesme.EslesenEsya.KullaniciId != userId)
            return Forbid();
        var digerKullanici = eslesme.IlkEsya.KullaniciId == userId ? eslesme.EslesenEsya.Kullanici : eslesme.IlkEsya.Kullanici;
        var mesaj = new Mesaj
        {
            EslesmeId = eslesmeId,
            GonderenId = userId,
            AliciId = digerKullanici.Id,
            Icerik = icerik,
            Tarih = DateTime.Now,
            OkunduMu = false
        };
        _db.Mesajlar.Add(mesaj);
        await _db.SaveChangesAsync();
        // SignalR ile karşı tarafa anlık mesaj bildirimi gönder
        await _hubContext.Clients.Group(digerKullanici.Id).SendAsync("MesajBildirimAl", eslesmeId, User.Identity.Name, icerik, mesaj.Tarih);
        return Json(new { success = true, mesaj });
    }

    // Sohbet modalı için action
    [HttpGet]
    public async Task<IActionResult?> Sohbet(int eslesmeId)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        var eslesme = await _db.Eslesmeler
            .Include(e => e.IlkEsya).ThenInclude(e => e.Kullanici)
            .Include(e => e.EslesenEsya).ThenInclude(e => e.Kullanici)
            .FirstOrDefaultAsync(e => e.Id == eslesmeId);
        if (eslesme == null)
            return Unauthorized();
        if (eslesme.IlkEsya.KullaniciId != userId && eslesme.EslesenEsya.KullaniciId != userId)
            return Forbid();
        var digerKullanici = eslesme.IlkEsya.KullaniciId == userId ? eslesme.EslesenEsya.Kullanici : eslesme.IlkEsya.Kullanici;
        var kendiKullanici = await _userManager.GetUserAsync(User);
        var mesajlar = await _db.Mesajlar
            .Where(m => m.EslesmeId == eslesmeId)
            .OrderBy(m => m.Tarih)
            .ToListAsync();
       
        var okunmamislar = mesajlar.Where(m => m.AliciId == userId && !m.OkunduMu).ToList();
        if (okunmamislar.Any())
        {
            foreach (var m in okunmamislar) m.OkunduMu = true;
            await _db.SaveChangesAsync();
        }
        ViewBag.DigerKullanici = digerKullanici;
        ViewBag.KendiAd = kendiKullanici?.Ad;
        ViewBag.KendiSoyad = kendiKullanici?.Soyad;
        ViewBag.EslesmeId = eslesmeId;
        return PartialView("~/Views/KullaniciPaneli/Partials/_Sohbet.cshtml", mesajlar);
    }

    public async Task<PartialViewResult> Mesajlarim()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return null; // veya uygun bir hata/Unauthorized
        var eslesmeler = await _db.Eslesmeler
            .Include(e => e.IlkEsya).ThenInclude(e => e.Kullanici)
            .Include(e => e.EslesenEsya).ThenInclude(e => e.Kullanici)
            .Where(e => e.IlkEsya.KullaniciId == userId || e.EslesenEsya.KullaniciId == userId)
            .OrderByDescending(e => e.EslesmeTarihi)
            .ToListAsync();
        // Her eşleşme için okunmamış mesaj sayısı
        var okunmamislar = _db.Mesajlar
            .Where(m => m.AliciId == userId && !m.OkunduMu)
            .GroupBy(m => m.EslesmeId)
            .Select(g => new { EslesmeId = g.Key, Sayi = g.Count() })
            .ToDictionary(x => x.EslesmeId, x => x.Sayi);
        ViewBag.Okunmamislar = okunmamislar;
        return PartialView("Partials/_Mesajlarim", eslesmeler);
    }
} 