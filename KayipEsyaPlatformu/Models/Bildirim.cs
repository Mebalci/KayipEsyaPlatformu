namespace KayipEsyaPlatformu.Models
{
    public class Bildirim
    {
        public int Id { get; set; }

        public string KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }

        public string Icerik { get; set; }

        public bool OkunduMu { get; set; }

        public DateTime Tarih { get; set; }
    }
}
