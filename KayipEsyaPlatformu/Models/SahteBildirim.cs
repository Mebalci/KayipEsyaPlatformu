namespace KayipEsyaPlatformu.Models
{
    public class SahteBildirim
    {
        public int Id { get; set; }

        public int EsyaId { get; set; }

        public string KullaniciId { get; set; }

        public DateTime TespitTarihi { get; set; }

        public string NedenSupheli { get; set; }

        public double ModelSkoru { get; set; }
    }
}
