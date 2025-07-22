using KayipEsyaPlatformu.Data;
using KayipEsyaPlatformu.Models;
using KayipEsyaPlatformu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

[Authorize]
public class EsyaController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<Kullanici> _userManager;
    private readonly IEsyaEslesmeServisi _eslesmeServisi;
    private readonly IEslesmeQueue _eslesmeQueue;

    public EsyaController(ApplicationDbContext db, UserManager<Kullanici> userManager, IEsyaEslesmeServisi eslesmeServisi, IEslesmeQueue eslesmeQueue)
    {
        _db = db;
        _userManager = userManager;
        _eslesmeServisi = eslesmeServisi;
        _eslesmeQueue = eslesmeQueue;
    }       
    public async Task<IActionResult> Detay(int id)
    {
        var esya = await _db.Esyalar
            .Include(e => e.Kategori)
            .Include(e => e.Durum)
            .FirstOrDefaultAsync(e => e.Id == id);        

        if (esya == null)
            return NotFound();

        var eslesmeler = await _db.Eslesmeler
            .Where(e => e.IlkEsyaId == id || e.EslesenEsyaId == id)
            .Include(e => e.IlkEsya)
            .Include(e => e.EslesenEsya)
            .OrderByDescending(e => e.BenzerlikOrani)
            .ToListAsync();

        ViewBag.EslesenEsyalar = eslesmeler;
        return View(esya);
    }
        
    [HttpGet]
    public async Task<IActionResult> Ekle()
    {
        //await _eslesmeServisi.TümEsyalarIcinTopluEslesmeAsync();
        await LoadFormData();
        return View();
    }
        
    [HttpPost]
    public async Task<IActionResult> Ekle(Esya esya, IFormFile resim)
    {
        if (esya == null)
            return BadRequest(new { success = false, message = "Form verileri alınamadı." });

        try
        {
            var rawEnlem = Request.Form["Enlem"];
            var rawBoylam = Request.Form["Boylam"];

            esya.Enlem = FixCoordinate(rawEnlem);
            esya.Boylam = FixCoordinate(rawBoylam);

            esya.KullaniciId = _userManager.GetUserId(User);
            esya.Tarih = DateTime.Now;
                        
            esya.Marka = string.IsNullOrWhiteSpace(esya.Marka) ? "" : esya.Marka;
            esya.Model = string.IsNullOrWhiteSpace(esya.Model) ? "" : esya.Model;
                        
            if (string.IsNullOrEmpty(esya.Baslik) || string.IsNullOrEmpty(esya.Aciklama)
                || esya.KategoriId <= 0 || esya.DurumId <= 0
                || esya.SehirId <= 0 || esya.IlceId <= 0 || esya.MahalleId <= 0
                || esya.RenkId <= 0 || string.IsNullOrEmpty(esya.Konum)
                || esya.Enlem == null || esya.Boylam == null)
            {
                return BadRequest(new { success = false, message = "Tüm alanları doldurmanız gerekmektedir." });
            }
                        
            if (resim != null && resim.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(resim.FileName);
                var path = Path.Combine("wwwroot/uploads", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                using var stream = new FileStream(path, FileMode.Create);
                await resim.CopyToAsync(stream);

                esya.ResimYolu = "/uploads/" + fileName;
            }
                        
            if (string.IsNullOrEmpty(esya.Konum))
            {
                var sehirAd = await _db.Sehirler.Where(x => x.Id == esya.SehirId).Select(x => x.Ad).FirstOrDefaultAsync();
                var ilceAd = await _db.Ilceler.Where(x => x.Id == esya.IlceId).Select(x => x.Ad).FirstOrDefaultAsync();
                var mahalleAd = await _db.Mahalleler.Where(x => x.Id == esya.MahalleId).Select(x => x.Ad).FirstOrDefaultAsync();
                esya.Konum = $"{mahalleAd}, {ilceAd}, {sehirAd}";
            }
           
            _db.Esyalar.Add(esya);
            await _db.SaveChangesAsync();

            var guncelEsya = await _db.Esyalar
                .Include(e => e.Renk)
                .Include(e => e.Sehir)
                .Include(e => e.Ilce)
                .Include(e => e.Mahalle)
                .Include(e => e.Kullanici)
                .FirstOrDefaultAsync(e => e.Id == esya.Id);

            //EŞLEŞTİRME
            //await _eslesmeServisi.EsyaIcinEslesmeleriHesaplaAsync(esya);
            
            if (guncelEsya != null)
            {
                _eslesmeQueue.Enqueue(guncelEsya);
            }


            return Ok(new { success = true, message = "Eşya kaydı başarıyla oluşturuldu." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = "Kayıt sırasında hata oluştu: " + ex.Message });
        }
    }

    private async Task LoadFormData()
    {
        ViewBag.Kategoriler = await _db.Kategoriler.ToListAsync();
        ViewBag.Durumlar = await _db.EsyaDurumlari.ToListAsync();
        ViewBag.Renkler = await _db.Renkler.ToListAsync();
        ViewBag.Sehirler = await _db.Sehirler.ToListAsync();
    }

    private double FixCoordinate(string raw)
    {        
        raw = new string(raw.Where(char.IsDigit).ToArray());
               
        while (raw.Length < 9)
            raw += "0";
               
        if (raw.Length > 9)
            raw = raw.Substring(0, 9);
                
        var formatted = raw.Insert(2, ".");
                
        return double.TryParse(formatted, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var result)
            ? result
            : 0;
    }

}
