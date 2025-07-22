namespace KayipEsyaPlatformu.Models
{
    public class Eslesme
    {
        public int Id { get; set; }

        public int IlkEsyaId { get; set; }
        public Esya IlkEsya { get; set; }

        public int EslesenEsyaId { get; set; }
        public Esya EslesenEsya { get; set; }

        public double BenzerlikOrani { get; set; }
        public DateTime EslesmeTarihi { get; set; }
        public bool OnaylandiMi { get; set; }
    }
}
