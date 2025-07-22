namespace KayipEsyaPlatformu.Models
{
    public class Mesaj
    {
        public int Id { get; set; }

        public string? GonderenId { get; set; }
        public Kullanici? Gonderen { get; set; }

        public string? AliciId { get; set; }
        public Kullanici? Alici { get; set; }

        public string? Icerik { get; set; }
        public DateTime Tarih { get; set; }
        public bool OkunduMu { get; set; }

        public int EslesmeId { get; set; }
        public Eslesme? Eslesme { get; set; }
    }
}
