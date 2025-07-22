using KayipEsyaPlatformu.Data;
using KayipEsyaPlatformu.Models;
using KayipEsyaPlatformu.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;



public class EsyaEslesmeServisi : IEsyaEslesmeServisi 
{
    private readonly ApplicationDbContext _db;
    private readonly EslesmeServisi _bertServisi;
    private readonly IEmailSender _mailServisi;
    private readonly BildirimServisi _bildirimServisi;

    public EsyaEslesmeServisi(ApplicationDbContext db, IEmailSender mailServisi, BildirimServisi bildirimServisi)
    {
        _db = db;
        _bertServisi = new EslesmeServisi(new HttpClient());
        _mailServisi = mailServisi;
        _bildirimServisi = bildirimServisi;
    }

    public async Task EsyaIcinEslesmeleriHesaplaAsync(Esya esya)
    {
        var adaylar = await _db.Esyalar
            .Where(e => e.Id != esya.Id && e.DurumId != esya.DurumId)
            .Include(e => e.Renk)
            .Include(e => e.Sehir)
            .Include(e => e.Ilce)
            .Include(e => e.Mahalle)
            .Include(e => e.Kullanici)
            .ToListAsync();

        var sonuc = await _bertServisi.EslestirAsync(esya, adaylar);

        foreach (var match in sonuc)
        {
            var eslesen = adaylar.FirstOrDefault(e => e.Id == match.id);

            if (eslesen != null)
            {
                var mevcut = await _db.Eslesmeler.FirstOrDefaultAsync(e =>
                    (e.IlkEsyaId == esya.Id && e.EslesenEsyaId == eslesen.Id) ||
                    (e.IlkEsyaId == eslesen.Id && e.EslesenEsyaId == esya.Id));

                if (mevcut == null)
                {
                    _db.Eslesmeler.Add(new Eslesme
                    {
                        IlkEsyaId = esya.Id,
                        EslesenEsyaId = eslesen.Id,
                        BenzerlikOrani = match.toplam_skor,
                        EslesmeTarihi = DateTime.Now
                    });

                    if (match.toplam_skor >= 0.90)
                    {
                        var alici = eslesen.Kullanici?.Email;
                        var konu = "Yeni Eşya Eşleşmesi!";
                        var mesaj = $"<p>Yeni eşleşme bulundu: <strong>{esya.Baslik}</strong><br>" +
                                    $"<a href='https://localhost:7151/Esya/Detay/{esya.Id}'>Detayları Gör</a></p>";

                        if (!string.IsNullOrEmpty(alici))
                        {
                            await _mailServisi.SendEmailAsync(alici, konu, mesaj);
                        }

                        await _bildirimServisi.Gonder(esya.KullaniciId!, $"%90 üzeri eşleşme: {eslesen.Baslik}");
                    }
                }
            }
        }

        await _db.SaveChangesAsync();
    }

    public async Task TümEsyalarIcinTopluEslesmeAsync()
    {
        var esyalar = await _db.Esyalar
            .Include(e => e.Renk)
            .Include(e => e.Sehir)
            .Include(e => e.Ilce)
            .Include(e => e.Mahalle)
            .ToListAsync();

        foreach (var esya in esyalar)
        {
            await EsyaIcinEslesmeleriHesaplaAsync(esya);
        }
    }
}
