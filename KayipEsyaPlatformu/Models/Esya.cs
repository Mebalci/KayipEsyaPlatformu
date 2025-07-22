using System.ComponentModel.DataAnnotations;

namespace KayipEsyaPlatformu.Models
{
    public class Esya
    {
        public int Id { get; set; }
        
        public string Baslik { get; set; }
               
        public string Aciklama { get; set; }

        public string Konum { get; set; }
       
        public DateTime Tarih { get; set; }
       
        public string ResimYolu { get; set; }

        public bool SahipUlasildiMi { get; set; } = false;
        
        public int DurumId { get; set; }
        public EsyaDurumu Durum { get; set; }
        
        public int KategoriId { get; set; }
        public Kategori Kategori { get; set; }
        
        public string KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }

        public double? Enlem { get; set; }
        public double? Boylam { get; set; }
        
        public int SehirId { get; set; }
        public Sehir Sehir { get; set; }        
        public int IlceId { get; set; }
        public Ilce Ilce { get; set; }
        
        public int MahalleId { get; set; }
        public Mahalle Mahalle { get; set; }

        public string Marka { get; set; }
        public string Model { get; set; }
        
        public int RenkId { get; set; }
        public Renk Renk { get; set; }
    }
}
