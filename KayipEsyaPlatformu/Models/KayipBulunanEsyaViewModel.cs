namespace KayipEsyaPlatformu.Models
{
    public class KayipBulunanEsyaViewModel
    {
        public int Id { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string Konum { get; set; }
        public DateTime Tarih { get; set; }
        public string Kategori { get; set; }
        public string Durum { get; set; } // Kayıp / Bulundu
        public bool BulunduMu { get; set; }
        public string ResimYolu { get; set; }
    }

}
