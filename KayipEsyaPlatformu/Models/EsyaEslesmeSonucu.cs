namespace KayipEsyaPlatformu.Models
{
    public class EsyaEslesmeSonucu
    {
        public int id { get; set; }
        public string eşya { get; set; }
        public string marka { get; set; }
        public string model { get; set; }
        public string renk { get; set; }
        public string şehir { get; set; }
        public string ilçe { get; set; }
        public string mahalle { get; set; }
        public string açıklama { get; set; }
        public string konum { get; set; }

        public double bert_skor { get; set; }
        public double toplam_skor { get; set; }
    }
}
